using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using VertexArmy.Global;
using VertexArmy.Levels;

namespace VertexArmy.States
{
	class GameStateLevelLoading : IGameState
	{
		private bool _isLoading;
		private int _loadingProgress;

		private SpriteFont _font;

		private Level _level;

		private readonly ContentManager _contentManager;
		private SpriteBatch _spriteBatch;

		public GameStateLevelLoading( ContentManager contentManager )
		{
			_contentManager = contentManager;
		}

		private void LoadContent()
		{
			_font = _contentManager.Load<SpriteFont>( @"fonts\SpriteFont1" );

			_level = LevelManager.Instance.GetLevel( "tutorial" );
		}

		public void OnUpdate( GameTime dt )
		{
			if ( _isLoading )
			{
				_loadingProgress += 1;

				if ( _loadingProgress >= 100 )
				{
					_isLoading = false;
				}
			}
		}

		public void OnRender( GameTime dt )
		{
			_spriteBatch.Begin();
			if ( _isLoading )
			{
				_spriteBatch.DrawString( _font, "Loading: " + _loadingProgress + " %", Vector2.Zero, Color.Black );
			}
			else
			{
				if ( _level != null )
				{
					_spriteBatch.DrawString( _font, "Level: " + _level.Name, Vector2.Zero, Color.Black );
				}
			}
			_spriteBatch.End();
		}

		public void OnEnter()
		{
			_isLoading = true;
			_loadingProgress = 0;

			_spriteBatch = new SpriteBatch( Platform.Instance.Device );

			LoadContent();
		}

		public void OnClose()
		{
			_contentManager.Unload();
		}
	}
}
