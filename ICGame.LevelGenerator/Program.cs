
using System;
using System.IO;

namespace ICGame.LevelGenerator
{
	public class Program
	{
		public static void Main( string[] args )
		{
			string targetPath = args.Length > 0 ? args[0] : "../../../ICGame/ICGame/Content/Levels/";

			Console.WriteLine( "Generating levels in " + targetPath );

			GenerateLevels( targetPath );
		}

		private static void GenerateLevels( string targetPath )
		{
			GenerateTutorial( targetPath );
		}

		private static void GenerateTutorial( string targetPath )
		{
			string tutorialPath = targetPath + "tutorial.level";
			using ( StreamWriter streamWriter = new StreamWriter( tutorialPath ) )
			{
			}
		}
	}
}
