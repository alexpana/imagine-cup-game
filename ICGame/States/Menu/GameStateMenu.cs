using System;
using System.Collections.Generic;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework.Media;
using VertexArmy.Content.Prefabs;
using VertexArmy.Global;
using VertexArmy.Global.Managers;

namespace VertexArmy.States.Menu
{
	public class GameStateMenu : BaseMenuGameState
	{
		private MenuCube _mainMenuCube;
		private MenuCube _optionsMenuCube;

		private MenuCube _activeCube;

		private readonly Platform _platform;

		private Video _video;
		private VideoPlayer _videoPlayer;
		private SpriteBatch _spriteBatch;
		private Texture2D _lastVideoTexture;
		private Rectangle _videoTargetRectangle;

		Vector3 _lightPos = new Vector3( 0, 40000, 20000 );

		public GameStateMenu( ContentManager content )
			: base( content )
		{
			_platform = Platform.Instance;
		}

		public override void OnRender( GameTime gameTime )
		{
			RenderBackgroundVideo();

			base.OnRender( gameTime );
		}

		private void RenderBackgroundVideo()
		{
			if ( _videoPlayer.State == MediaState.Playing )
			{
				_lastVideoTexture = _videoPlayer.GetTexture();
			}

			if ( _lastVideoTexture != null )
			{
				_spriteBatch.Begin();
				_spriteBatch.Draw( _lastVideoTexture, _videoTargetRectangle, Color.White );
				_spriteBatch.End();
			}
		}

		public override void OnUpdate( GameTime gameTime )
		{
			if ( _activeCube != null )
			{
				HandleInput();

				_activeCube.Update( gameTime );
			}

			_lightPos.Z = ( float ) ( 50000f + 25000.0 * Math.Sin( gameTime.TotalGameTime.TotalMilliseconds / 1000.0 ) );
			SceneManager.Instance.SetLightPosition( _lightPos );

			base.OnUpdate( gameTime );
		}

		private void HandleInput()
		{
			if ( _platform.Input.IsKeyPressed( Keys.Left, false ) )
			{
				_activeCube.SelectPreviousItem();
			}
			else if ( _platform.Input.IsKeyPressed( Keys.Right, false ) )
			{
				_activeCube.SelectNextItem();
			}
			else if ( _platform.Input.IsKeyPressed( Keys.Enter, false ) )
			{
				_activeCube.Items[_activeCube.SelectedItem].Activate();
				_platform.SoundManager.PlaySound( MenuEventSound );
			}
			else if ( _platform.Input.IsKeyPressed( Keys.Escape, false ) ||
					 _platform.Input.IsKeyPressed( Keys.Back, false ) )
			{
				if ( _activeCube.PreviousMenu != null )
				{
					_activeCube.Destroy();
					_activeCube = _activeCube.PreviousMenu;
					_platform.SoundManager.PlaySound( MenuEventSound );
				}
			}
		}

		private void CreateMenus()
		{
			_mainMenuCube = new MenuCube( ContentManager )
			{
				Title = "Main menu",
				SelectionSound = MenuItemSelectionSound,
				Items = new List<MenuItem>
				{
					new MenuItem { Title = "Play!", Activated = args => StateManager.Instance.ChangeState(GameState.SelectLevelMenu) },
					new MenuItem { Title = "Options", Activated = args => ActivateMenuCube(_optionsMenuCube) },
					new MenuItem { Title = "Exit", Activated = args => _platform.Game.Exit() }
				}
			};
			_mainMenuCube.SetTextImage( "main_cube_text" );

			_optionsMenuCube = new MenuCube( ContentManager )
			{
				Title = "Options menu",
				PreviousMenu = _mainMenuCube,
				SelectionSound = MenuItemSelectionSound,
				Items = new List<MenuItem>
				{
					new SwitchMenuItem
					{
						OnTitle = "On", OffTitle = "Off", Prefix = "Sounds",
						IsOn = _platform.Settings.IsMusicEnabled,
						Activated = args =>
						{
							_platform.Settings.IsMusicEnabled = (bool)args;
							UpdateMenuItemBackgroundImage(_optionsMenuCube, (bool)args);
							_platform.Settings.Save();
						}
					}
				}
			};
			UpdateMenuItemBackgroundImage( _optionsMenuCube, _platform.Settings.IsMusicEnabled );
		}

		private void UpdateMenuItemBackgroundImage( MenuCube menuCube, bool isSoundEnabled )
		{
			menuCube.SetTextImage( CreateImagePath( "options_cube", new Dictionary<string, string>
			{
				{ "sounds", isSoundEnabled ? "on" : "off" }
			} ) );
		}

		private void ActivateMenuCube( MenuCube cube )
		{
			_activeCube = cube;
			_activeCube.Spawn( -25f );
		}

		private string CreateImagePath( string prefix, Dictionary<string, string> options )
		{
			StringBuilder stringBuilder = new StringBuilder();

			stringBuilder.Append( prefix );

			foreach ( var option in options )
			{
				stringBuilder.AppendFormat( "_{0}-{1}", option.Key, option.Value );
			}

			return stringBuilder.ToString();
		}

		public override void OnEnter()
		{
			base.OnEnter();
			Platform.Instance.PhysicsWorld.Gravity = Vector2.UnitY * Platform.Instance.PhysicsWorld.Gravity.Length();
			CreateMenus();

			ActivateMenuCube( _mainMenuCube );

			if ( !_platform.SoundManager.IsMusicPlaying )
			{
				_platform.SoundManager.PlayMusic( BackgroundMusic );
			}

			GameWorldManager.Instance.SpawnEntity( CameraPrefab.PrefabName, "menu_camera", new Vector3( 0, 0, 100 ) );

			//GameWorldManager.Instance.SpawnEntity( "WallMenu", "wallMenu1",
			//				new Vector3( 0f, 0f, -1800f ), Quaternion.CreateFromAxisAngle( Vector3.UnitX, -0.3f ), 150 );

			_spriteBatch = new SpriteBatch( _platform.Device );
			_video = ContentManager.Load<Video>( "movies/PurpleStarFlight" );
			_videoPlayer = new VideoPlayer { IsLooped = true };
			_videoTargetRectangle = new Rectangle( 0, 0, _platform.Device.Viewport.Width, _platform.Device.Viewport.Height );

			_videoPlayer.Play( _video );
		}

		public override void OnClose()
		{
			base.OnClose();

			_mainMenuCube.Destroy();
			_optionsMenuCube.Destroy();

			GameWorldManager.Instance.Clear();
		}
	}
}
