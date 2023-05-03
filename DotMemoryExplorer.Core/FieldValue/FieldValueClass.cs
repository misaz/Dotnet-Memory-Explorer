namespace DotMemoryExplorer.Core.FieldValue {

	public class FieldValueClass : FieldContent {
		public DotnetObjectMetadata ReferencedObject { get; }
		public DotnetTypeMetadata ReferencedObjectType { get; }

		public FieldValueClass(DotnetObjectMetadata referencedObject, DotnetTypeMetadata referencedObjectType) {
			ReferencedObject = referencedObject;
			ReferencedObjectType = referencedObjectType;
		}

		public override string ToString() {
			return $"0x{ReferencedObject.Address.ToString("X16")} ({ReferencedObjectType.TypeName})";
		}
	}
}