using System.Collections.Generic;
using Microsoft.Xna.Framework;
using VertexArmy.GameWorld.Prefabs;
using VertexArmy.GameWorld.Prefabs.Structs;

namespace VertexArmy.Content.Prefabs
{
	internal class PipePrefab
	{
		public static PrefabEntity CreatePrefab()
		{
			PrefabEntity pipe = new PrefabEntity { Name = "Pipe" };

			ShapePrefab pipeWall1 = new ShapePrefab
			{
				Density = 1f,
				Width = 20f,
				Height = 950f,
				Offset = new Vector2( -101f, 425f ),
				Type = ShapeType.Rectangle
			};

			ShapePrefab pipeWall2 = new ShapePrefab
			{
				Density = 1f,
				Width = 20f,
				Height = 950f,
				Offset = new Vector2( 101f, 425f ),
				Type = ShapeType.Rectangle
			};

			BodyPrefab pipeBody = new BodyPrefab
			{
				Friction = 0.3f,
				LocalPosition = Vector2.Zero,
				Name = "PipeBody",
				Shapes = new List<ShapePrefab> { pipeWall1, pipeWall2 },
				Restitution = 0.1f,
				Static = true,
			};

			pipe.RegisterBody( pipeBody, true );

			MeshSceneNodePrefab pipeModel = new MeshSceneNodePrefab { Mesh = "models/pipe", Material = "PipeMaterial", Name = "PipeModel" };

			pipe.RegisterMeshSceneNode( pipeModel );

			ControllerPrefab controller = new ControllerPrefab { Body = "PipeBody", Transformable = "PipeModel", Name = "PipeBodyController", Type = ControllerType.BodyController };

			pipe.RegisterController( controller );

			return pipe;
		}
	}
}
