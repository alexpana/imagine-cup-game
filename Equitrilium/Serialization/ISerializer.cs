using System.IO;

namespace VertexArmy.Serialization
{
	public interface ISerializer<TType>
	{
		void WriteObject( TType obj, Stream stream );
		TType ReadObject( Stream stream );
	}
}
