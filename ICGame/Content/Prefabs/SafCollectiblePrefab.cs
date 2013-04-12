using Microsoft.Xna.Framework;
using VertexArmy.GameWorld.Prefabs;
using VertexArmy.GameWorld.Prefabs.Structs;

namespace VertexArmy.Content.Prefabs
{
	class SafCollectiblePrefab
	{
		public static PrefabEntity CreatePrefab()
		{
			PrefabEntity mesh = new PrefabEntity { Name = "SafCollectible" };


			MeshSceneNodePrefab one = new MeshSceneNodePrefab
			{
				Material = "SafMaterial",
				Mesh = "models/saf",
				Name = "M11",
				LocalRotation = Quaternion.CreateFromAxisAngle(Vector3.UnitZ, MathHelper.PiOver4),
				LocalScale = 3 * Vector3.One
			};


			MeshSceneNodePrefab two = new MeshSceneNodePrefab
			{
				Material = "SafMaterial",
				Mesh = "models/saf",
				Name = "M12",
				LocalRotation = Quaternion.CreateFromAxisAngle( Vector3.UnitZ, -3 * MathHelper.PiOver4 ),
				LocalScale = 3 * Vector3.One
			};
			
			mesh.RegisterMeshSceneNode( one );
			mesh.RegisterMeshSceneNode( two );

			return mesh;
		}
	}
}
