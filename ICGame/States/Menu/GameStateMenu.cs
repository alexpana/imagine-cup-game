using System;
using System.Collections.Generic;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework.Media;
using VertexArmy.Global;
using VertexArmy.Global.Managers;

namespace VertexArmy.States.Menu
{
	public class GameStateMenu : IGameState
	{
		private MenuCube _mainMenuCube;
		private MenuCube _optionsMenuCube;

		private MenuCube _activeCube;

		private SpriteBatch _spriteBatch;
		private SpriteFont _font;
		private Song _backgroundMusic;

		private readonly ContentManager _content;

		public GameStateMenu( ContentManager content )
		{
			_content = content;
		}

		private void ActivateMenuCube( MenuCube cube )
		{
			_activeCube = cube;
			_activeCube.Spawn();
		}

		public void OnUpdate( GameTime gameTime )
		{
			if ( _activeCube != null )
			{
				if ( Platform.Instance.Input.IsKeyPressed( Keys.Left, false ) )
				{
					_activeCube.SelectPreviousItem();
				}
				else if ( Platform.Instance.Input.IsKeyPressed( Keys.Right, false ) )
				{
					_activeCube.SelectNextItem();
				}
				else if ( Platform.Instance.Input.IsKeyPressed( Keys.Enter, false ) )
				{
					_activeCube.Items[_activeCube.SelectedItem].Activate();
				}
				else if ( Platform.Instance.Input.IsKeyPressed( Keys.Escape, false ) ||
						 Platform.Instance.Input.IsKeyPressed( Keys.Back, false ) )
				{
					_activeCube.Destroy();
					_activeCube = _activeCube.PreviousMenu;
				}

				_activeCube.Update( gameTime );
			}

			Platform.Instance.PhysicsWorld.Step( Math.Min( ( float ) gameTime.ElapsedGameTime.TotalMilliseconds * 0.001f, ( 1f / 30f ) ) );
			FrameUpdateManager.Instance.Update( gameTime );
		}

		public void OnRender( GameTime gameTime )
		{
			if ( _activeCube != null )
			{
				_spriteBatch.Begin();

				_spriteBatch.DrawString( _font, _activeCube.Title, new Vector2( 100, 100 ), Color.Black );

				_spriteBatch.DrawString( _font, "< " + _activeCube.Items[_activeCube.SelectedItem].Title + " >",
					new Vector2( 100, 150 ), Color.Black );

				_spriteBatch.End();

				SceneManager.Instance.Render( gameTime.ElapsedGameTime.Milliseconds );
			}
		}

		public void OnEnter()
		{
			CreateMenus();

			ActivateMenuCube( _mainMenuCube );
			_spriteBatch = new SpriteBatch( Platform.Instance.Device );
			_font = Platform.Instance.Content.Load<SpriteFont>( "fonts/SpriteFont1" );
			_backgroundMusic = _content.Load<Song>( "music/proto1_menu" );

			Platform.Instance.SoundPlayer.PlayMusic( _backgroundMusic );
			GameWorldManager.Instance.SpawnEntity( "camera", "camera1", new Vector3( 0, 0, 100 ) );
		}

		private void CreateMenus()
		{
			_mainMenuCube = new MenuCube
			{
				Title = "Main menu",
				Items = new List<MenuItem>
				{
					new MenuItem { Title = "Play!", Activated = args => StateManager.Instance.ChangeState(GameState.PhysicsPresentationRobot) },
					new MenuItem { Title = "Options", Activated = args => ActivateMenuCube(_optionsMenuCube) },
					new MenuItem { Title = "Exit", Activated = args => Platform.Instance.Game.Exit() }
				}
			};

			_optionsMenuCube = new MenuCube
			{
				Title = "Options menu",
				PreviousMenu = _mainMenuCube,
				Items = new List<MenuItem>
				{
					new SwitchMenuItem
					{
						OnTitle = "On", OffTitle = "Off", Prefix = "Music",
						IsOn = Platform.Instance.Settings.GetValue(Settings.IsMusicEnabled, true),
						Activated = args => Platform.Instance.Settings.SetValue(Settings.IsMusicEnabled, (bool)args)
					}
				}
			};
		}

		public void OnClose()
		{
			_mainMenuCube.Destroy();
			_optionsMenuCube.Destroy();

			SceneManager.Instance.Clear();
			GameWorldManager.Instance.Clear();
			Platform.Instance.SoundPlayer.StopMusic();
		}
	}
}
