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
	}
}
