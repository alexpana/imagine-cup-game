using System;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
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

			GameWorldManager.Instance.SpawnEntity( "camera", new Vector3( 0, 0, -300 ), "camera1" );
			GameWorldManager.Instance.SpawnEntity( "mesh", new Vector3( 0f, 0f, 0f ), "mesh1" );

			GameWorld.GameEntity mesh = GameWorldManager.Instance.GetEntity( "mesh1" );
		}

		public override void OnClose()
		{
			SceneManager.Instance.Clear( );
		}
	}
}
