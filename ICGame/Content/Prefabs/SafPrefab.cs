using Microsoft.Xna.Framework;
using VertexArmy.GameWorld.Prefabs;
using VertexArmy.GameWorld.Prefabs.Structs;

namespace VertexArmy.Content.Prefabs
{
	class SafPrefab
	{
		public static PrefabEntity CreatePrefab()
		{
			PrefabEntity mesh = new PrefabEntity();

			MeshSceneNodePrefab one = new MeshSceneNodePrefab
			{
				Material = "SafMaterial",
				Mesh = "models/saf",
				Name = "Mesh",
				LocalRotation = Quaternion.Identity,
				LocalScale = Vector3.One,
				Invisible = true,
				DrawsDepth =  false
			};

			mesh.RegisterMeshSceneNode( one );
			return mesh;
		}
	}
}
