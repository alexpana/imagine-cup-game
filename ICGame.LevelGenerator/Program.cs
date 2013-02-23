
using System;
using FarseerPhysics.Dynamics;
using Microsoft.Xna.Framework;
using VertexArmy.Global;

namespace ICGame.LevelGenerator
{
	public class Program
	{
		public static void Main( string[] args )
		{
			SetupPlatform();

			string targetPath = args.Length > 0 ? args[0] : AppDomain.CurrentDomain.BaseDirectory + "/../../../../ICGame/Content/Levels/";
			string serializationType = args.Length > 1 ? args[1] : "json";

			Generator generator = new Generator( targetPath, serializationType );
			generator.GenerateLevels();
		}

		private static void SetupPlatform()
		{
			Platform.Instance.PhysicsWorld = new World( Vector2.Zero );
		}
	}

	class DummyGame : Game
	{

	}
}
