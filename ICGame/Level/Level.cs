using System.Collections.Generic;

namespace VertexArmy.Level
{
	public class Level
	{
		public string Name { get; set; }
		public List<LevelChunk> Chunks { get; private set; }
	}
}
