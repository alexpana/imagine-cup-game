using System;
using VertexArmy.Entities;
using VertexArmy.Entities.Physics;
using VertexArmy.Global;
using VertexArmy.Graphics;
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
			};

			LevelChunk mainChunk = new LevelChunk();
			tutorialLevel.Chunks.Add( mainChunk );

			// create entities
			RobotEntity robot = new RobotEntity
			{
				BasePhysicsEntity = new PhysicsEntityTank( Platform.Instance.PhysicsWorld ),
				SceneNode = new SceneNode()
			};

			// add entities
			mainChunk.Entities.Add( robot );

			return tutorialLevel;
		}
	}
}
