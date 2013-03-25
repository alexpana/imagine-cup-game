using Microsoft.Xna.Framework.Graphics;
using VertexArmy.Global;
using VertexArmy.Global.Managers;
using VertexArmy.Graphics;

namespace VertexArmy.GameWorld.Prefabs.Structs
{
	public struct ArrayMeshSceneNodePrefab
	{
		public string Name { get; set; }
		public string Path;
		public string Mesh;
		public string Material;

		public int StartIndex, EndIndex;

		public Material GetMaterial()
		{
			return MaterialRepository.Instance.GetMaterial( Material );
		}

		public SceneNode GetSceneNode()
		{
			SceneNode scn = new SceneNode();
			scn.AddAttachable( new MeshAttachable( Platform.Instance.Content.Load<Model>( Mesh ), GetMaterial() ) );

			return scn;
		}
	}
}