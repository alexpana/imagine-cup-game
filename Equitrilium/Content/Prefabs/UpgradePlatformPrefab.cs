using System.Collections.Generic;
using Microsoft.Xna.Framework;
using VertexArmy.GameWorld.Prefabs;
using VertexArmy.GameWorld.Prefabs.Structs;

namespace VertexArmy.Content.Prefabs
{
	public class UpgradePlatformPrefab
	{
		public static PrefabEntity CreatePrefab()
		{
			PrefabEntity entity = new PrefabEntity { Name = "UpradePlatform" };

			ShapePrefab shape = new ShapePrefab
			{
				Density = 1f,
				Width = 180f,
				Height = 10f,
				Offset = Vector2.Zero,
				Type = ShapeType.Rectangle
			};

			BodyPrefab body = new BodyPrefab
			{
				Friction = 1f,
				LocalPosition = Vector2.Zero,
				Name = "UpgradePlatformBody",
				Shapes = new List<ShapePrefab> { shape },
				Restitution = 0.1f,
				Static = true,
			};

			entity.RegisterBody( body, true );


			MeshSceneNodePrefab mesh = new MeshSceneNodePrefab
			{
				Name = "UpgradePlatformMesh",
				Mesh = "models/upgrade_platform",
				Material = "UPlatformMaterial",
			};

			entity.RegisterMeshSceneNode( mesh );
			ControllerPrefab meshController = new ControllerPrefab
			{
				Name = "UpgradePlatformController",
				Type = ControllerType.BodyController,
				Body = "UpgradePlatformBody",
				Transformable = "UpgradePlatformMesh"
			};

			entity.RegisterController( meshController );

			return entity;
		}
	}
}
