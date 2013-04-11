using System.Collections.Generic;
using Microsoft.Xna.Framework;
using VertexArmy.GameWorld.Prefabs;
using VertexArmy.GameWorld.Prefabs.Structs;

namespace VertexArmy.Content.Prefabs
{
	public class FloorBridgePrefab
	{
		public static PrefabEntity CreatePrefab()
		{
			PrefabEntity floor = new PrefabEntity { Name = "FloorBridge" };

			ShapePrefab floorShape = new ShapePrefab
			{
				Density = 1f,
				Width = 240f,
				Height = 10f,
				Offset = Vector2.Zero,
				Type = ShapeType.Rectangle
			};

			BodyPrefab floorBody = new BodyPrefab
			{
				Friction = 1f,
				LocalPosition = Vector2.Zero,
				Name = "FloorBody",
				Shapes = new List<ShapePrefab> { floorShape },
				Restitution = 0.1f,
				Static = false,
			};

			floor.RegisterBody( floorBody, true );


			MeshSceneNodePrefab mesh = new MeshSceneNodePrefab
			{
				Name = "FloorMesh",
				Mesh = "models/floor_tile_1",
				Material = "FloorBridgeMaterial",
			};

			floor.RegisterMeshSceneNode( mesh );
			ControllerPrefab meshController = new ControllerPrefab
			{
				Name = "FloorController",
				Type = ControllerType.BodyController,
				Body = "FloorBody",
				Transformable = "FloorMesh"
			};

			floor.RegisterController( meshController );

			return floor;
		}
	}
}
