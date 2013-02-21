using System;
using System.Collections.Generic;
using VertexArmy.Levels;

namespace ICGame.LevelGenerator
{
	public class TutorialGenerator
	{
		public static Level GenerateTutorial()
		{
			Console.WriteLine( "Generating tutorial ..." );

			Level tutorialLevel = new Level
			{
				Name = "Tutorial",
				Chunks = new List<LevelChunk>()
			};

			return tutorialLevel;
		}
	}
}
