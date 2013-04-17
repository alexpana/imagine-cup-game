using System.Collections.Generic;
using System.Runtime.Serialization;
using VertexArmy.GameWorld;

namespace VertexArmy.Levels
{
	[DataContract]
	public class LevelChunk
	{
		[DataMember]
		public List<GameEntity> Entities { get; set; }

		public LevelChunk()
		{
			Entities = new List<GameEntity>();
		}
	}
}
