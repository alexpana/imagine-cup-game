using Microsoft.Xna.Framework;
using VertexArmy.GameWorld.Prefabs;
using VertexArmy.GameWorld.Prefabs.Structs;
using VertexArmy.Global.Managers;

namespace VertexArmy.Content.Prefabs
{
	class SafPrefab
	{
		public static PrefabEntity CreatePrefab()
		{
			PrefabEntity mesh = new PrefabEntity();
			SafSceneNodePrefab crateSceneNode = new SafSceneNodePrefab
			{
				Material = "SafMaterial",
				Mesh = "models/saf",
				Name = "Mesh",
				LocalRotation = Quaternion.Identity
			};

			mesh.RegisterSafSceneNode( crateSceneNode );
			return mesh;
		}
	}
}
