using System.Collections.Generic;
using Microsoft.Xna.Framework;
using VertexArmy.GameWorld.Prefabs;
using VertexArmy.GameWorld.Prefabs.Structs;

namespace VertexArmy.Content.Prefabs
{
	public class WallPrefab
	{
		public static PrefabEntity CreatePrefab()
		{
			PrefabEntity wall = new PrefabEntity { Name = "Wall" };

			ShapePrefab wallShape = new ShapePrefab
			{
				Density = 1f,
				Width = 10f,
				Height = 60f,
				Offset = Vector2.Zero,
				Type = ShapeType.Rectangle
			};

			BodyPrefab wallBody = new BodyPrefab
			{
				Friction = 1f,
				LocalPosition = Vector2.Zero,
				Name = "WallBody",
				Shapes = new List<ShapePrefab> { wallShape },
				Restitution = 0.1f,
				Static = true,
			};

			wall.RegisterBody( wallBody, true );

			MeshSceneNodePrefab mesh = new MeshSceneNodePrefab
			{
				Name = "WallMesh",
				Mesh = "models/wall_tile",
				Material = "TileMaterial",
			};

			wall.RegisterMeshSceneNode( mesh );
			ControllerPrefab meshController = new ControllerPrefab
			{
				Name = "WallController",
				Type = ControllerType.BodyController,
				Body = "WallBody",
				Transformable = "WallMesh"
			};

			wall.RegisterController( meshController );

			return wall;
		}
	}
}
