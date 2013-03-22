using System.Collections.Generic;
using Microsoft.Xna.Framework;
using VertexArmy.GameWorld.Prefabs;

namespace VertexArmy.Content.Prefabs
{
	public class CratePrefab
	{
		public static PrefabEntity CreatePrefab()
		{
			PrefabEntity crate = new PrefabEntity { Name = "Crate", PhysicsScale = 1f };

			const float PhysicsInternalScale = 0.2f;

			ShapePrefab crateShape = new ShapePrefab
									 {
										 Density = 1f,
										 Width = 1f * PhysicsInternalScale,
										 Height = 1f * PhysicsInternalScale,
										 Offset = Vector2.Zero,
										 Type = ShapeType.Rectangle
									 };

			BodyPrefab crateBody = new BodyPrefab
								   {
									   Friction = 1f,
									   LocalPosition = Vector2.Zero,
									   Name = "CrateBody",
									   Shapes = new List<ShapePrefab> { crateShape },
									   Restitution = 0.1f,
									   Static = false
								   };

			crate.RegisterBody( crateBody, true );

			MeshSceneNodePrefab crateSceneNode = new MeshSceneNodePrefab
												 {
													 Body = "CrateBody",
													 Material = "RobotMaterial",
													 Mesh = "models/crate00",
													 Name = "CrateNode"
												 };

			crate.RegisterMeshSceneNode( crateSceneNode );

			return crate;
		}
	}
}
