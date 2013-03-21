using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
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

		public void OnUpdate( GameTime dt )
		{
		}

		public void OnRender( GameTime dt )
		{
		}

		public void OnEnter()
		{
			SceneManager.Instance.Clear();
		}

		public void OnClose()
		{
			SceneManager.Instance.Clear( );
		}
	}
}
