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
			if ( Keyboard.GetState( PlayerIndex.One ).IsKeyDown( Keys.Enter ) )
			{
				StateManager.Instance.PopState();
			}
		}

		public void OnRender( GameTime gameTime )
		{
			_spriteBatch.Begin();

			_spriteBatch.DrawString( _font, "Paused. Press ENTER to continue.", new Vector2( Platform.Instance.Device.Viewport.Width / 2.0f, Platform.Instance.Device.Viewport.Height / 2.0f ), Color.Black );
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
