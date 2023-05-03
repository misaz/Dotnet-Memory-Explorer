using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DotMemoryExplorer.Core {

	/// <summary>
	/// List of Field Type values as defined in .NET runtime CorHdr.h and used in field.cpp file.
	/// </summary>
	public enum CorElementType {
		ELEMENT_TYPE_BOOLEAN = 0x02,
		ELEMENT_TYPE_CHAR = 0x03,
		ELEMENT_TYPE_I1 = 0x04,
		ELEMENT_TYPE_U1 = 0x05,
		ELEMENT_TYPE_I2 = 0x06,
		ELEMENT_TYPE_U2 = 0x07,
		ELEMENT_TYPE_I4 = 0x08,
		ELEMENT_TYPE_U4 = 0x09,
		ELEMENT_TYPE_I8 = 0x0a,
		ELEMENT_TYPE_U8 = 0x0b,
		ELEMENT_TYPE_R4 = 0x0c,
		ELEMENT_TYPE_R8 = 0x0d,
		ELEMENT_TYPE_PTR = 0x0f,
		ELEMENT_TYPE_BYREF = 0x10,
		ELEMENT_TYPE_VALUETYPE = 0x11,
		ELEMENT_TYPE_CLASS = 0x12,
		ELEMENT_TYPE_TYPEDBYREF = 0x16,
		ELEMENT_TYPE_I = 0x18,
		ELEMENT_TYPE_U = 0x19,
		ELEMENT_TYPE_FNPTR = 0x1b
	}
}
