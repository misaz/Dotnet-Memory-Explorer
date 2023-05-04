using DotMemoryExplorer.Core.FieldValue;
using Microsoft.Diagnostics.Tracing.Parsers.MicrosoftWindowsWPF;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Net;
using System.Reflection.Metadata;
using System.Text;
using System.Threading.Tasks;

namespace DotMemoryExplorer.Core {
	public class ObjectDetailProvider {

		private readonly DotnetObjectMetadata _object;
		private readonly HeapDump _owningHeapDump;

		public ObjectDetailProvider(DotnetObjectMetadata obj, HeapDump owningHeapDump) {
			if (owningHeapDump == null) {
				throw new ArgumentNullException(nameof(owningHeapDump));
			}

			_object = obj;
			_owningHeapDump = owningHeapDump;
		}

		/// <summary>
		/// Gets address of EEClass related to analyzed object
		/// </summary>
		/// <returns></returns>
		public ulong GetEEClassAddress() {
			return GetEEClassAddress(_object.TypeId);
		}

		/// <summary>
		/// Gets object header field of the object
		/// </summary>
		/// <returns></returns>
		public ulong GetObjectHeader() {
			return ReadLiveMemoryPointer(_object.Address - GetPointerSize());
		}

		/// <summary>
		/// Gets address of EEClass related to class specified by MethodTable at specified address.
		/// </summary>
		/// <param name="methodTablePointer">Address of MethodTable</param>
		/// <returns>Address of EEClass</returns>
		public ulong GetEEClassAddress(ulong methodTablePointer) {
			// EE Class address is stored inside MethodTable after 16 B of fields and three
			// pointers (their sizes are 32/64 bit system specific)

			// TypeId is in fact pointer to MethodTable
			ulong address = methodTablePointer + 16 + 3 * GetPointerSize();

			return ReadLiveMemoryPointer(address);
		}

		/// <summary>
		/// Gets address of parent MethodTable for specified MethodTable
		/// </summary>
		/// <param name="methodTablePointer">Address of MethodTable</param>
		/// <returns>Address of parent MethodTable</returns>
		private ulong GetParentMethodTablePointer(ulong methodTablePointer) {
			// parent MT pointer is stored inside MethodTable after 16 B of fields

			// TypeId is in fact pointer to MethodTable
			ulong address = methodTablePointer + 16;

			return ReadLiveMemoryPointer(address);
		}

		private ulong GetPointerSize() {
			if (_owningHeapDump.OwningProcess.Bitness == 64) {
				return 8;
			} else {
				return 4;
			}
		}

		private ulong MemoryToPointer(ReadOnlySpan<byte> mem) {
			if (_owningHeapDump.OwningProcess.Bitness == 64) {
				return BitConverter.ToUInt64(mem);
			} else {
				return (ulong)BitConverter.ToUInt32(mem);
			}
		}

		private ulong MemoryToNativeUInt(ReadOnlySpan<byte> mem) {
			if (_owningHeapDump.OwningProcess.Bitness == 64) {
				return BitConverter.ToUInt64(mem);
			} else {
				return (ulong)BitConverter.ToUInt32(mem);
			}
		}

		private long MemoryToNativeInt(ReadOnlySpan<byte> mem) {
			if (_owningHeapDump.OwningProcess.Bitness == 64) {
				return BitConverter.ToInt64(mem);
			} else {
				return (long)BitConverter.ToInt32(mem);
			}
		}

		private ulong ReadLiveMemory64(ulong address) {
			var mem = _owningHeapDump.OwningProcess.ProcessMemoryManger.GetMemory(address, 8);
			return BitConverter.ToUInt64(mem);
		}

		private ulong ReadLiveMemoryPointer(ulong address) {
			var mem = _owningHeapDump.OwningProcess.ProcessMemoryManger.GetMemory(address, GetPointerSize());
			return MemoryToPointer(mem);
		}

		private ReadOnlySpan<byte> ReadFieldMemory(FieldMetadata meta, ulong length) {
			return _owningHeapDump.MemoryDump.GetMemory(GetFieldContentAddress(meta), length);
		}

		private IEnumerable<FieldMetadata> GetFieldsForMethodTable(ulong methodTablePointer) {
			List<FieldMetadata> output = new List<FieldMetadata>();

			// fields are listed in array which is pointed in 3th pointer-size field in EE Class
			ulong eeClass = GetEEClassAddress(methodTablePointer);
			ulong fieldsArrayPointer = ReadLiveMemoryPointer(eeClass + 3 * GetPointerSize());

			ulong fieldOwningClassMethodTable;

			int index = 0;

			do {
				if (fieldsArrayPointer == 0) {
					break;
				}

				fieldOwningClassMethodTable = ReadLiveMemoryPointer(fieldsArrayPointer);
				fieldsArrayPointer += GetPointerSize();
				ulong fieldMetadata = ReadLiveMemory64(fieldsArrayPointer);
				fieldsArrayPointer += 8;

				uint fieldMetadata1 = (uint)(fieldMetadata & 0xFFFFFFFF);
				uint fieldMetadata2 = (uint)((fieldMetadata & 0xFFFFFFFF00000000) >> 32);

				ulong mb = 0x04000000 | fieldMetadata1 & 0xFFFFFF;
				ulong isStatic = fieldMetadata1 & (1 << 24);

				ulong offset = (fieldMetadata2 & 0x7FFFFFF);
				ulong type = fieldMetadata2 >> 27;

				if (fieldOwningClassMethodTable == _object.TypeId && offset < _object.Size && isStatic == 0) {
					FieldId fId = new FieldId(methodTablePointer, mb);
					FieldMetadata metaWithoutContent = new FieldMetadata(fId, offset, isStatic, type, index, null);
					FieldContent content = ParseFieldValue(metaWithoutContent);

					output.Add(new FieldMetadata(fId, offset, isStatic, type, index, content));

					index++;
				}
			} while (fieldOwningClassMethodTable == _object.TypeId);

			return output;
		}

		private FieldContent ParseFieldValue(FieldMetadata meta) {
			switch ((CorElementType)meta.FieldType) {
				case CorElementType.ELEMENT_TYPE_BOOLEAN:
					return new FieldValueBoolean(BitConverter.ToBoolean(ReadFieldMemory(meta, 1)));
				case CorElementType.ELEMENT_TYPE_CHAR:
					return new FieldValueChar(BitConverter.ToChar(ReadFieldMemory(meta, 2)));
				case CorElementType.ELEMENT_TYPE_I1:
					return new FieldValueInt8(unchecked((sbyte)ReadFieldMemory(meta, 1)[0]));
				case CorElementType.ELEMENT_TYPE_U1:
					return new FieldValueUint8(ReadFieldMemory(meta, 1)[0]);
				case CorElementType.ELEMENT_TYPE_I2:
					return new FieldValueInt16(BitConverter.ToInt16(ReadFieldMemory(meta, 2)));
				case CorElementType.ELEMENT_TYPE_U2:
					return new FieldValueUInt16(BitConverter.ToUInt16(ReadFieldMemory(meta, 2)));
				case CorElementType.ELEMENT_TYPE_I4:
					return new FieldValueInt32(BitConverter.ToInt32(ReadFieldMemory(meta, 4)));
				case CorElementType.ELEMENT_TYPE_U4:
					return new FieldValueUInt32(BitConverter.ToUInt32(ReadFieldMemory(meta, 4)));
				case CorElementType.ELEMENT_TYPE_I8:
					return new FieldValueInt64(BitConverter.ToInt64(ReadFieldMemory(meta, 8)));
				case CorElementType.ELEMENT_TYPE_U8:
					return new FieldValueUInt64(BitConverter.ToUInt64(ReadFieldMemory(meta, 8)));
				case CorElementType.ELEMENT_TYPE_R4:
					return new FieldValueFloat(BitConverter.ToSingle(ReadFieldMemory(meta, 4)));
				case CorElementType.ELEMENT_TYPE_R8:
					return new FieldValueDouble(BitConverter.ToDouble(ReadFieldMemory(meta, 8)));
				case CorElementType.ELEMENT_TYPE_PTR:
					return new FieldValuePointer(MemoryToPointer(ReadFieldMemory(meta, GetPointerSize())));
				case CorElementType.ELEMENT_TYPE_VALUETYPE:
					return new FieldValueValueType();
				case CorElementType.ELEMENT_TYPE_CLASS:
					ulong objAddr = MemoryToPointer(ReadFieldMemory(meta, GetPointerSize()));
					DotnetObjectMetadata obj;
					DotnetTypeMetadata type;
					try {
						obj = _owningHeapDump.GetObjectByAddress(objAddr);
						type = _owningHeapDump.GetTypeById(obj.TypeId);
						return new FieldValueClass(obj, type);
					} catch (Exception) {
						return new FieldValueClassNull();
					}
				case CorElementType.ELEMENT_TYPE_TYPEDBYREF:
					return new FieldValueTypeByReference(MemoryToPointer(ReadFieldMemory(meta, GetPointerSize())));
				case CorElementType.ELEMENT_TYPE_I:
					return new FieldValueNativeInt(MemoryToNativeInt(ReadFieldMemory(meta, GetPointerSize())));
				case CorElementType.ELEMENT_TYPE_U:
					return new FieldValueNativeUint(MemoryToNativeUInt(ReadFieldMemory(meta, GetPointerSize())));
				case CorElementType.ELEMENT_TYPE_FNPTR:
					return new FieldValueFunctionPointer(MemoryToNativeUInt(ReadFieldMemory(meta, GetPointerSize())));
				default:
					return new FieldValueUnknownType();
			}
		}

		public IEnumerable<FieldMetadata> GetFields() {
			var classInheritanceStack = GetInheritanceStack();

			List<FieldMetadata> fields = new List<FieldMetadata>();

			while (classInheritanceStack.Any()) {
				fields.AddRange(GetFieldsForMethodTable(classInheritanceStack.Pop()));
			}

			return fields.AsReadOnly();
		}

		public ulong GetFieldContentAddress(FieldMetadata meta) {
			// field content is located at offset which do not incluide size of pointer to
			// MethodTable which is at the begining of every field.

			return _object.Address + GetPointerSize() + meta.Offset;
		}

		private Stack<ulong> GetInheritanceStack() {
			Stack<ulong> methodTableInheritanceStack = new Stack<ulong>();
			ulong currentMt = _object.TypeId;

			while (currentMt != 0) {
				methodTableInheritanceStack.Push(currentMt);
				currentMt = GetParentMethodTablePointer(currentMt);
			}

			return methodTableInheritanceStack;
		}

	}
}
