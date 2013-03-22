using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using VertexArmy.GameWorld.Prefabs;
using VertexArmy.Global;
using VertexArmy.Global.Managers;
using VertexArmy.Graphics;

namespace VertexArmy.States
{
	class GameStateModelViewer : PlayableGameState
	{
		private ContentManager _cm;
		private SceneManager _sceneManager;
		public GameStateModelViewer( ContentManager content )
		{
			_cm = content;
		}

		public void OnUpdate( GameTime gameTime )
		{
		}

		public void OnRender( GameTime gameTime )
		{
		}

		public override void OnEnter()
		{
			SceneManager.Instance.Clear( );
			CursorManager.Instance.SetActiveCursor( CursorType.Arrow );
			CursorManager.Instance.SetVisible( true );

			GameWorldManager.Instance.SpawnEntity( "camera", "camera1", new Vector3( 0, -1300, -300 ) );


			PrefabEntity mesh = new PrefabEntity( );

			MeshSceneNodePrefab crateSceneNode = new MeshSceneNodePrefab
			{
				Material = "RobotMaterial",
				Mesh = "models/robo_wheel",
				Name = "Mesh",
				LocalRotation = new Quaternion( new Vector3( 0f, 1f, 0f ), 1f )
			};

			mesh.RegisterMeshSceneNode( crateSceneNode );

			GameWorldManager.Instance.SpawnEntity( mesh, "mesh1", new Vector3( 0f, -1300f, 0f ) );
		}

		public override void OnClose()
		{
			SceneManager.Instance.Clear( );
		}
	}
}
