using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using VertexArmy.Global;
using VertexArmy.Global.Managers;
using VertexArmy.Graphics;

namespace VertexArmy.States
{
	internal class GameStatePaused : IGameState
	{
		private readonly ContentManager _contentManager;
		private SpriteFont _font;
		private SpriteBatch _spriteBatch;

		private Texture2D _background;
		private readonly InGameMenuManager _manager;

		public GameStatePaused( ContentManager content )
		{
			_contentManager = content;
			_manager = new InGameMenuManager( OnContinueAction, OnExitAction );
		}

		public void OnUpdate( GameTime gameTime )
		{
			if ( Platform.Instance.Input.IsKeyPressed( Keys.Escape, false ) )
			{
				OnContinueAction();
			}
			_manager.Update( gameTime );
		}

		public void OnContinueAction()
		{
			StateManager.Instance.PopState();
		}

		public void OnExitAction()
		{
			StateManager.Instance.PopState();
			StateManager.Instance.ChangeState( GameState.Menu );
		}

		public void OnRender( GameTime gameTime )
		{
			_spriteBatch.Begin( SpriteSortMode.Immediate, BlendState.Opaque );

			// Draw game background
			_spriteBatch.Draw( _background, new Rectangle( 0, 0, Platform.Instance.Device.Viewport.Width, Platform.Instance.Device.Viewport.Height ), Color.White );
			_spriteBatch.End();

			_manager.Render();
		}

		public void OnEnter()
		{
			_font = _contentManager.Load<SpriteFont>( "fonts/SpriteFont1" );
			_spriteBatch = new SpriteBatch( Platform.Instance.Device );

			_background = Renderer.Instance.LastFrame;
		}

		public void OnClose()
		{
			_contentManager.Unload();
		}
	}
}
