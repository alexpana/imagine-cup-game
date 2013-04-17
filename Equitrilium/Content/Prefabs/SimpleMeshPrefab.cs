using VertexArmy.GameWorld.Prefabs;
using VertexArmy.GameWorld.Prefabs.Structs;

namespace VertexArmy.Content.Prefabs
{
	public class SimpleMeshPrefab
	{
		public static PrefabEntity CreatePrefab()
		{
			PrefabEntity mesh = new PrefabEntity { Name = "Mesh" };


			MeshSceneNodePrefab crateSceneNode = new MeshSceneNodePrefab
			{
				Material = "RobotMaterial",
				Mesh = "models/robo_wheel",
				Name = "Mesh"
			};

			mesh.RegisterMeshSceneNode( crateSceneNode );

			return mesh;
		}

		public static PrefabEntity CreatePrefab( string mesh, string material, string name )
		{
			PrefabEntity prf = new PrefabEntity { Name = name };

			MeshSceneNodePrefab crateSceneNode = new MeshSceneNodePrefab
			{
				Material = material,
				Mesh = mesh,
				Name = name
			};

			prf.RegisterMeshSceneNode( crateSceneNode );

			return prf;
		}
	}
}
