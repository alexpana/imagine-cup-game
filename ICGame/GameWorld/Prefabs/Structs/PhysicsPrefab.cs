using System.Collections.Generic;

namespace VertexArmy.GameWorld.Prefabs.Structs
{
	public class PhysicsPrefab
	{
		public Dictionary<string, BodyPrefab> Bodies;
		public Dictionary<string, JointPrefab> Joints;
		public Dictionary<string, PathPrefab> Paths;
	}
}