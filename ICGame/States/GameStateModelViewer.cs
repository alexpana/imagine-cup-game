using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using VertexArmy.Global;
using VertexArmy.Graphics;

namespace VertexArmy.States
{
	class GameStateModelViewer : IGameState
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

		public void OnEnter()
		{
			SceneManager.Instance.Clear();
			CursorManager.Instance.SetActiveCursor( CursorType.Arrow );
			CursorManager.Instance.SetVisible( true );
		}

		public void OnClose()
		{
			SceneManager.Instance.Clear( );
		}
	}
}
