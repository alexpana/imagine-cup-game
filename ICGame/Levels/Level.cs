using System.Collections.Generic;
using System.Runtime.Serialization;

namespace VertexArmy.Levels
{
	[DataContract]
	public class Level
	{
		[DataMember]
		public string Name { get; set; }

		[DataMember]
		public List<LevelChunk> Chunks { get; set; }

		public Level()
		{
			Chunks = new List<LevelChunk>( );
		}
	}
}
