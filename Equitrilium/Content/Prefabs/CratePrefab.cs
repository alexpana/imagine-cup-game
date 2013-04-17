using System.Collections.Generic;
using Microsoft.Xna.Framework;
using VertexArmy.GameWorld.Prefabs;
using VertexArmy.GameWorld.Prefabs.Structs;

namespace VertexArmy.Content.Prefabs
{
	public class CratePrefab
	{
		public static PrefabEntity CreatePrefab()
		{
			PrefabEntity crate = new PrefabEntity { Name = "Crate" };

			ShapePrefab crateShape = new ShapePrefab
			{
				Density = 0.4f,
				Width = 18f,
				Height = 18f,
				Offset = Vector2.Zero,
				Type = ShapeType.Rectangle
			};

			BodyPrefab crateBody = new BodyPrefab
			{
				Friction = 0.3f,
				LocalPosition = Vector2.Zero,
				Name = "CrateBody",
				Shapes = new List<ShapePrefab> { crateShape },
				Restitution = 0.1f,
				Static = false,
				CollisionSounds = new List<string>() { "sounds/hit1", "sounds/hit2", "sounds/hit3", "sounds/hit4" }
			};

			crate.RegisterBody( crateBody, true );

			MeshSceneNodePrefab crateSceneNode = new MeshSceneNodePrefab
			{
				Material = "CrateMaterial",
				Mesh = "models/crate00",
				Name = "CrateNode"
			};


			crate.RegisterMeshSceneNode( crateSceneNode );

			ControllerPrefab crateController = new ControllerPrefab
			{
				Name = "CrateBodyController",
				Type = ControllerType.BodyController,
				Body = "CrateBody",
				Transformable = "CrateNode"
			};

			crate.RegisterController( crateController );

			return crate;
		}
	}
}
