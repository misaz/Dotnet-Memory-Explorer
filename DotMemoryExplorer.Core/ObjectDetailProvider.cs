using Microsoft.Diagnostics.Tracing.Parsers.MicrosoftWindowsWPF;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
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

		private ulong ReadLiveMemory64(ulong address) {
			var mem = _owningHeapDump.OwningProcess.ProcessMemoryManger.GetMemory(address, 8);
			return BitConverter.ToUInt64(mem);
		}

		private ulong GetPointerSize() {
			if (_owningHeapDump.OwningProcess.Bitness == 64) {
				return 8;
			} else {
				return 4;
			}
		}

		private ulong ReadLiveMemoryPointer(ulong address) {
			var mem = _owningHeapDump.OwningProcess.ProcessMemoryManger.GetMemory(address, GetPointerSize());

			if (_owningHeapDump.OwningProcess.Bitness == 64) {
				return BitConverter.ToUInt64(mem);
			} else {
				return (ulong)BitConverter.ToUInt32(mem);
			}
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

				if (fieldOwningClassMethodTable == _object.TypeId) {
					uint fieldMetadata1 = (uint)(fieldMetadata & 0xFFFFFFFF);
					uint fieldMetadata2 = (uint)((fieldMetadata & 0xFFFFFFFF00000000) >> 32);

					ulong mb = 0x04000000 | fieldMetadata1 & 0xFFFFFF;
					ulong isStatic = fieldMetadata1 & (1 << 24);

					ulong offset = (fieldMetadata2 & 0x7FFFFFF);
					ulong type = fieldMetadata2 >> 27;

					output.Add(new FieldMetadata(methodTablePointer, mb, offset, isStatic, type, index, null));

					index++;
				}
			} while (fieldOwningClassMethodTable == _object.TypeId);

			return output;
		}

		public IEnumerable<FieldMetadata> GetFields() {
			var classInheritanceStack = GetInheritanceStack();

			List<FieldMetadata> fields = new List<FieldMetadata>();

			while (classInheritanceStack.Any()) {
				fields.AddRange(GetFieldsForMethodTable(classInheritanceStack.Pop()));
			}

			return fields.AsReadOnly();
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
