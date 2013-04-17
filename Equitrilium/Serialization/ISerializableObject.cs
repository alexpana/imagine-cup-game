namespace VertexArmy.Serialization
{
	/// <summary>
	/// Interface to define a point that can be called after deserialization 
	/// to properly initialize the object
	/// </summary>
	public interface ISerializableObject
	{
		void PreSerialize();
		void PostDeserialize();
	}
}
