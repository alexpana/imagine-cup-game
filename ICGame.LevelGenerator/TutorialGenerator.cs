using System;
using Microsoft.Xna.Framework;
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
				BasePhysicsEntity = new PhysicsEntityRobot( 0.7f ),
				SceneNode = new SceneNode()
			};

			BlockEntity robotHolder = new BlockEntity
			{
				BasePhysicsEntity = new PhysicsEntityBasic( PhysicsEntityType.Rectangle, 50, 10 )
				{
					Position = new Vector2( 0, 5f )
				},
				SceneNode = new SceneNode()
			};

			// add entities
			mainChunk.Entities.Add( robot );
			mainChunk.Entities.Add( robotHolder );

			return tutorialLevel;
		}
	}
}
