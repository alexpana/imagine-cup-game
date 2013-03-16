using System;
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

			LevelChunk mainChunk = new LevelChunk( );
			tutorialLevel.Chunks.Add( mainChunk );


			/*
			BlockEntity robotHolder = new BlockEntity
			{
				BasePhysicsEntity = new PhysicsEntityBasic( PhysicsEntityType.Rectangle, 50, 10 )
				{
					Position = new Vector2( 0, 5f )
				},
				SceneNode = new SceneNode()
			};
			 

			// add entities
			mainChunk.Entities.Add( robotHolder );
			 */

			return tutorialLevel;
		}
	}
}
