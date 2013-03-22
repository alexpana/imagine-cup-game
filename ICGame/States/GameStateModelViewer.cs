using System;
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

		public override void OnUpdate( GameTime gameTime )
		{
			float angle = (float)Math.Cos( 0.1 );
			Vector3 unity = new Vector3( 0, 1, 0 );
			unity *= (float)Math.Sin( 0.1 );
			Quaternion q = new Quaternion(unity, angle);

			GameWorld.GameEntity mesh = GameWorldManager.Instance.GetEntity( "mesh1" );
			//mesh.SetRotation( mesh.GetRotation() * q );

			//GameWorldManager.Instance.GetEntity( "mesh1" ).setR
			mesh.SetScale( new Vector3( 10, 10, 10 ) );
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

			crateSceneNode.LocalRotation.Normalize( );

			mesh.RegisterMeshSceneNode( crateSceneNode );

			GameWorldManager.Instance.SpawnEntity( mesh, "mesh1", new Vector3( 0f, -1300f, 0f ) );
		}

		public override void OnClose()
		{
			SceneManager.Instance.Clear( );
		}
	}
}
