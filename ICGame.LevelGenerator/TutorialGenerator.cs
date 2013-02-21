using System;
using System.Collections.Generic;
using VertexArmy.Entities;
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

			LevelChunk mainChunk = new LevelChunk();
			tutorialLevel.Chunks.Add( mainChunk );

			// create entities
			RobotEntity robot = new RobotEntity();

			// add entities
			mainChunk.Entities.Add( robot );

			return tutorialLevel;
		}
	}
}
