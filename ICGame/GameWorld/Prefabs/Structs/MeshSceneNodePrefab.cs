using Microsoft.Xna.Framework;
using VertexArmy.Global.Managers;
using VertexArmy.Graphics;

namespace VertexArmy.GameWorld.Prefabs.Structs
{
	public struct MeshSceneNodePrefab
	{
		public string Name { get; set; }
		public string Body;
		public string Mesh;
		public string Material;
		public Vector3 LocalPosition;
		public Quaternion LocalRotation;

		public Material GetMaterial()
		{
			return MaterialRepository.Instance.GetMaterial( Material );
		}
	}
}