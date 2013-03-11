using System.Collections.Generic;
using FarseerPhysics.Collision.Shapes;
using FarseerPhysics.Factories;

namespace VertexArmy.GameWorld
{
	public class PrefabEntity
	{
		public string Name { get; set; };
		public GameEntityFlags Flags { get; set; }

		private PhysicsPrefab _physicsPrefab;
		private SceneNodesPrefab _sceneNodesPrefab;
		private ControllerPrefab _controllerPrefab;

		internal struct PhysicsPrefab
		{
			
		}

		internal struct SceneNodesPrefab
		{
			
		}

		internal struct ControllerPrefab
		{
			
		}

		internal struct BodyPrefab
		{
			public string Name { get; set; }
			public bool Static { get; set; }
			public float Friction { get; set; }
			public float Restitution { get; set; }

			public List<ShapePrefab> Shapes;
		}

		internal struct ShapePrefab
		{
		}
	}
}