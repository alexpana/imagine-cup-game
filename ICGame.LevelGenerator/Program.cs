
namespace ICGame.LevelGenerator
{
	public class Program
	{
		public static void Main( string[] args )
		{
			string targetPath = args.Length > 0 ? args[0] : "../../../../ICGame/Content/Levels/";
			string serializationType = args.Length > 1 ? args[1] : "xml";

			Generator generator = new Generator( targetPath, serializationType );
			generator.GenerateLevels();
		}
	}
}
