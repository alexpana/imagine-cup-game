using System.Collections.Generic;

namespace VertexArmy.Levels
{
	public class Level
	{
		public string Name { get; set; }
		public List<LevelChunk> Chunks { get; set; }
	}
}
