using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using VertexArmy.Global;

namespace VertexArmy.States
{
	internal class GameStatePaused : IGameState
	{
		private readonly ContentManager _contentManager;
		private SpriteFont _font;
		private SpriteBatch _spriteBatch;

		public GameStatePaused( ContentManager content )
		{
			_contentManager = content;
		}

		public void OnUpdate( GameTime gameTime )
		{
			if ( Platform.Instance.Input.IsKeyPressed( Keys.Escape, false ) )
			{
				StateManager.Instance.PopState();
			}
			else if ( Platform.Instance.Input.IsKeyPressed( Keys.Enter, false ) )
			{
				StateManager.Instance.PopState();
				StateManager.Instance.ChangeState( GameState.Menu );
			}
		}

		public void OnRender( GameTime gameTime )
		{
			_spriteBatch.Begin();

			float x = Platform.Instance.Device.Viewport.Width / 2.0f - 100f;

			_spriteBatch.DrawString( _font, "Paused. Press ESCAPE to resume.", new Vector2( x, Platform.Instance.Device.Viewport.Height / 2.0f ), Color.Black );
			_spriteBatch.DrawString( _font, "        Press ENTER to exit to menu.", new Vector2( x, Platform.Instance.Device.Viewport.Height / 2.0f + 30f ), Color.Black );
			_spriteBatch.End();
		}

		public void OnEnter()
		{
			_font = _contentManager.Load<SpriteFont>( "fonts/SpriteFont1" );
			_spriteBatch = new SpriteBatch( Platform.Instance.Device );
		}

		public void OnClose()
		{
			_contentManager.Unload();
		}
	}
}
