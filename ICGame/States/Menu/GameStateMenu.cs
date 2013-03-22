using System.Collections.Generic;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using VertexArmy.Global;

namespace VertexArmy.States.Menu
{
	public class GameStateMenu : IGameState
	{
		private readonly MenuCube _mainMenuCube;
		private readonly MenuCube _optionsMenuCube;

		private MenuCube _activeCube;

		private SpriteBatch _spriteBatch;
		private SpriteFont _font;

		public GameStateMenu( ContentManager content )
		{
			_mainMenuCube = new MenuCube
			{
				Title = "Main menu",
				Items = new List<MenuCubeItem>
				{
					new MenuCubeItem { Title = "Play!" },
					new MenuCubeItem { Title = "Options", Activated = args => ActivateMenuCube(_optionsMenuCube)},
					new MenuCubeItem { Title = "Exit", Activated = args => Platform.Instance.Game.Exit() }
				}
			};

			_optionsMenuCube = new MenuCube
			{
				Title = "Options menu",
				PreviousMenu = _mainMenuCube,
				Items = new List<MenuCubeItem>
				{
					new MenuCubeItem{Title = "Music"}
				}
			};
		}

		private void ActivateMenuCube( MenuCube cube )
		{
			_activeCube = cube;
		}

		public void OnUpdate( GameTime gameTime )
		{
			if ( Platform.Instance.Input.IsKeyPressed( Keys.Left, false ) )
			{
				if ( _activeCube.SelectedItem == 0 )
				{
					_activeCube.SelectedItem = _activeCube.Items.Count - 1;
				}
				else
				{
					_activeCube.SelectedItem--;
				}
			}
			else if ( Platform.Instance.Input.IsKeyPressed( Keys.Right, false ) )
			{
				_activeCube.SelectedItem = ( _activeCube.SelectedItem + 1 ) % _activeCube.Items.Count;
			}
			else if ( Platform.Instance.Input.IsKeyPressed( Keys.Enter, false ) )
			{
				_activeCube.Items[_activeCube.SelectedItem].Activate();
			}
			else if ( Platform.Instance.Input.IsKeyPressed( Keys.Escape, false ) ||
					 Platform.Instance.Input.IsKeyPressed( Keys.Back, false ) )
			{
				if ( _activeCube.PreviousMenu != null )
				{
					ActivateMenuCube( _activeCube.PreviousMenu );
				}
			}
		}

		public void OnRender( GameTime gameTime )
		{
			if ( _activeCube != null )
			{
				_spriteBatch.Begin();

				_spriteBatch.DrawString( _font, _activeCube.Title, new Vector2( 100, 100 ), Color.Black );

				_spriteBatch.DrawString( _font, _activeCube.Items[_activeCube.SelectedItem].Title,
					new Vector2( 100, 150 ), Color.Black );

				_spriteBatch.End();
			}
		}

		public void OnEnter()
		{
			ActivateMenuCube( _mainMenuCube );
			_spriteBatch = new SpriteBatch( Platform.Instance.Device );
			_font = Platform.Instance.Content.Load<SpriteFont>( "fonts/SpriteFont1" );
		}

		public void OnClose()
		{
		}
	}
}
