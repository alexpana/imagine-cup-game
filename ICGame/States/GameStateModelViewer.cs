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

			GameWorldManager.Instance.SpawnEntity( "camera", new Vector3( 0, -1300, -300 ), "camera1" );
			GameWorldManager.Instance.SpawnEntity( "mesh", new Vector3( 0f, -1300f, 0f ), "mesh1" );
		}

		public override void OnClose()
		{
			SceneManager.Instance.Clear( );
		}
	}
}
