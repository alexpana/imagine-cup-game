using System;
using System.Collections.Generic;
using System.Text;
using FarseerPhysics;
using FarseerPhysics.Common;
using FarseerPhysics.Dynamics;
using FarseerPhysics.Factories;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Audio;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework.Media;
using VertexArmy.Global;
using VertexArmy.Global.Managers;
using VertexArmy.Physics.DebugView;
using VertexArmy.Utilities;

namespace VertexArmy.States.Menu
{
	public class GameStateMenu : IGameState
	{
		private MenuCube _mainMenuCube;
		private MenuCube _optionsMenuCube;

		private MenuCube _activeCube;

		private Song _backgroundMusic;
		private SoundEffect _menuItemSelectionSound;
		private SoundEffect _menuEventSound;

		private DebugViewXNA _debugView;
		private Matrix _projection;
		private Matrix _view;

		private Body _ground;

		private readonly ContentManager _content;
		private readonly Platform _platform;

		public GameStateMenu( ContentManager content )
		{
			_content = content;
			_platform = Platform.Instance;
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
				HandleInput();

				_activeCube.Update( gameTime );
			}

			_platform.PhysicsWorld.Step( Math.Min( ( float ) gameTime.ElapsedGameTime.TotalMilliseconds * 0.001f, ( 1f / 30f ) ) );
			FrameUpdateManager.Instance.Update( gameTime );
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
				_platform.SoundManager.PlaySound( _menuEventSound );
			}
			else if ( _platform.Input.IsKeyPressed( Keys.Escape, false ) ||
					 _platform.Input.IsKeyPressed( Keys.Back, false ) )
			{
				if ( _activeCube.PreviousMenu != null )
				{
					_activeCube.Destroy();
					_activeCube = _activeCube.PreviousMenu;
					_platform.SoundManager.PlaySound( _menuEventSound );
				}
			}
		}

		public void OnRender( GameTime gameTime )
		{
			_projection = Matrix.CreateOrthographicOffCenter(
				UnitsConverter.ToSimUnits( -Platform.Instance.Device.Viewport.Width / 2f ),
				UnitsConverter.ToSimUnits( Platform.Instance.Device.Viewport.Width / 2f ),
				UnitsConverter.ToSimUnits( Platform.Instance.Device.Viewport.Height / 2f ),
				UnitsConverter.ToSimUnits( -Platform.Instance.Device.Viewport.Height / 2f ),
				0f,
				1f
			);

			_debugView.RenderDebugData( ref _projection, ref _view );
			SceneManager.Instance.Render( gameTime.ElapsedGameTime.Milliseconds );
		}

		public void OnEnter()
		{
			_backgroundMusic = _content.Load<Song>( "music/proto1_menu" );
			_menuItemSelectionSound = _content.Load<SoundEffect>( "sounds/button-27" );
			_menuEventSound = _content.Load<SoundEffect>( "sounds/button-30" );

			CreateMenus();
			CreateCubesGround();
			CreateDebugView( _content );

			ActivateMenuCube( _mainMenuCube );

			_platform.SoundManager.PlayMusic( _backgroundMusic );
			GameWorldManager.Instance.SpawnEntity( "camera", "menu_camera", new Vector3( 0, 0, 100 ) );
		}

		private void CreateCubesGround()
		{
			_ground = new Body( _platform.PhysicsWorld )
			{
				Friction = 1.2f,
				Restitution = 0f
			};

			Vertices vertices = new Vertices
			{	
				new Vector2( -10f, 0.5f ),
				new Vector2( 10f, 0.5f )
			};

			for ( int i = 0; i < vertices.Count - 1; ++i )
			{
				FixtureFactory.AttachEdge( vertices[i], vertices[i + 1], _ground );
			}
		}

		private void CreateDebugView( ContentManager content )
		{
			_debugView = new DebugViewXNA( _platform.PhysicsWorld );

			_debugView.LoadContent( _platform.Device, content );
			_debugView.RemoveFlags( DebugViewFlags.Joint );

			_debugView.TextColor = Color.Black;
			_view = Matrix.Identity;
		}

		private void CreateMenus()
		{
			_mainMenuCube = new MenuCube( _content )
			{
				Title = "Main menu",
				SelectionSound = _menuItemSelectionSound,
				Items = new List<MenuItem>
				{
					new MenuItem { Title = "Play!", Activated = args => StateManager.Instance.ChangeState(GameState.PhysicsPresentationRobot) },
					new MenuItem { Title = "Options", Activated = args => ActivateMenuCube(_optionsMenuCube) },
					new MenuItem { Title = "Exit", Activated = args => _platform.Game.Exit() }
				}
			};
			_mainMenuCube.SetBackgroundImage( "main" );

			_optionsMenuCube = new MenuCube( _content )
			{
				Title = "Options menu",
				PreviousMenu = _mainMenuCube,
				SelectionSound = _menuItemSelectionSound,
				Items = new List<MenuItem>
				{
					new SwitchMenuItem
					{
						OnTitle = "On", OffTitle = "Off", Prefix = "Sounds",
						IsOn = _platform.Settings.IsMusicEnabled,
						Activated = args =>
						{
							_platform.Settings.IsMusicEnabled = (bool)args;
							_optionsMenuCube.SetBackgroundImage(CreateImagePath("options", new Dictionary<string, string>
							{
								{ "sounds", ((bool)args) ? "on" : "off" }
							}));
						}
					}
				}
			};
			_optionsMenuCube.SetBackgroundImage( "options_sounds-on" );
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

		public void OnClose()
		{
			_mainMenuCube.Destroy();
			_optionsMenuCube.Destroy();

			_platform.SoundManager.StopMusic();

			GameWorldManager.Instance.Clear();
			ControllerRepository.Instance.Clear();
			PhysicsContactManager.Instance.Clear();
			FrameUpdateManager.Instance.Clear();
			_platform.PhysicsWorld.Clear();
			SceneManager.Instance.Clear();

			_content.Unload();
		}
	}
}
