using System.Collections.Generic;
using Microsoft.Xna.Framework;
using VertexArmy.GameWorld.Prefabs;
using VertexArmy.GameWorld.Prefabs.Structs;

namespace VertexArmy.Content.Prefabs
{
	public class MenuCubePrefab
	{
		public static PrefabEntity CreatePrefab()
		{
			PrefabEntity cube = new PrefabEntity { Name = "Menu Cube" };

			ShapePrefab cubeShape = new ShapePrefab
									 {
										 Density = 1f,
										 Width = 20f,
										 Height = 20f,
										 Offset = Vector2.Zero,
										 Type = ShapeType.Rectangle
									 };

			BodyPrefab cubeBody = new BodyPrefab
								   {
									   Friction = 1f,
									   LocalPosition = Vector2.Zero,
									   Name = "MenuCubeBody",
									   Shapes = new List<ShapePrefab> { cubeShape },
									   Restitution = 0.25f,
									   Static = false
								   };

			cube.RegisterBody( cubeBody, true );

			MeshSceneNodePrefab cubeSceneNode = new MeshSceneNodePrefab
												 {
													 Material = "MenuCubeMaterial",
													 Mesh = "models/menu_cube",
													 Name = "MenuCubeNode"
												 };

			cube.RegisterMeshSceneNode( cubeSceneNode );


			ControllerPrefab cubeController = new ControllerPrefab
			{
				Name = "MenuCubeBodyController",
				Type = ControllerType.BodyController,
				Body = "MenuCubeBody",
				Transformable = "MenuCubeNode"
			};

			cube.RegisterController( cubeController );

			return cube;
		}
	}
}
