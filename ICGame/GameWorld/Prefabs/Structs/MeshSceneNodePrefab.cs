using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using VertexArmy.Global;
using VertexArmy.Global.Managers;
using VertexArmy.Graphics;

namespace VertexArmy.GameWorld.Prefabs.Structs
{
	public class MeshSceneNodePrefab
	{
		public string Name { get; set; }
		public string Mesh;
		public string Material;
		public Vector3 LocalPosition;
		public Quaternion LocalRotation;

		public Material GetMaterial()
		{
			return MaterialRepository.Instance.GetMaterial( Material );
		}

		public SceneNode GetSceneNode()
		{
			SceneNode scn = new SceneNode();
			scn.AddAttachable( new MeshAttachable( Platform.Instance.Content.Load<Model>( Mesh ), GetMaterial() ) );

			if ( LocalPosition != null )
			{
				scn.SetPosition( LocalPosition );
			}

			if ( LocalRotation != null )
			{
				scn.SetRotation( LocalRotation );
			}

			return scn;
		}
	}
}