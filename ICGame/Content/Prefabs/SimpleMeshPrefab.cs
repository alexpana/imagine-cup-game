using VertexArmy.GameWorld.Prefabs;

namespace VertexArmy.Content.Prefabs
{
	public class SimpleMeshPrefab
	{
		public static PrefabEntity CreatePrefab()
		{
			PrefabEntity mesh = new PrefabEntity { Name = "Mesh", PhysicsScale = 1f };


			MeshSceneNodePrefab crateSceneNode = new MeshSceneNodePrefab
												 {
													 Material = "RobotMaterial",
													 Mesh = "models/crate00",
													 Name = "CrateNode"
												 };

			mesh.RegisterMeshSceneNode( crateSceneNode );

			return mesh;
		}
	}
}
