
using System.IO;

namespace VertexArmy.Utilities.Serialization
{
	public interface ISerializer<TType>
	{
		void WriteObject( TType obj, Stream stream );
		TType ReadObject( Stream stream );
	}
}
