namespace VertexArmy.Serialization
{
	public class SerializerFactory
	{
		public static ISerializer<T> CreateSerializer<T>()
		{
			return new JsonSerializer<T>();
		}
	}
}
