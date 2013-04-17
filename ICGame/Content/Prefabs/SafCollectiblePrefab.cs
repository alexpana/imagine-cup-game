using Microsoft.Xna.Framework;
using VertexArmy.GameWorld.Prefabs;
using VertexArmy.GameWorld.Prefabs.Structs;

namespace VertexArmy.Content.Prefabs
{
	internal class SafCollectiblePrefab
	{
		public static PrefabEntity CreatePrefab()
		{
			PrefabEntity mesh = new PrefabEntity { Name = "SafCollectible" };


			MeshSceneNodePrefab one = new MeshSceneNodePrefab
			{
				Material = "SafMaterial",
				Mesh = "models/saf",
				Name = "M11",
				LocalRotation = Quaternion.CreateFromAxisAngle( Vector3.UnitZ, MathHelper.PiOver4 ),
				LocalScale = 4 * Vector3.One
			};


			MeshSceneNodePrefab two = new MeshSceneNodePrefab
			{
				Material = "SafMaterial",
				Mesh = "models/saf",
				Name = "M12",
				LocalRotation = Quaternion.CreateFromAxisAngle( Vector3.UnitZ, -3 * MathHelper.PiOver4 ),
				LocalScale = 4 * Vector3.One
			};


			MeshSceneNodePrefab three = new MeshSceneNodePrefab
			{
				Material = "PowerupSphereMaterial",
				Mesh = "models/sphere",
				Name = "M13",
				LocalScale = Vector3.One
			};

			mesh.RegisterMeshSceneNode( one );
			mesh.RegisterMeshSceneNode( two );
			mesh.RegisterMeshSceneNode( three );

			return mesh;
		}
	}
}
