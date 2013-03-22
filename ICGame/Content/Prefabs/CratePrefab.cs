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
										 Density = 1f,
										 Width = 20f,
										 Height = 20f,
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
