using System.Collections.Generic;
using System.Runtime.Serialization;
using VertexArmy.Entities;

namespace VertexArmy.Levels
{
	[DataContract]
	public class LevelChunk
	{
		[DataMember]
		public List<BaseEntity> Entities { get; set; }

		public LevelChunk()
		{
			Entities = new List<BaseEntity>();
		}
	}
}
