using System;
using System.Collections.Generic;
using System.Text;
using FarseerPhysics.Common;
using FarseerPhysics.Dynamics;
using FarseerPhysics.Factories;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Input;
using VertexArmy.Global;

namespace VertexArmy.States.Menu
{
	public class GameStateMenu : BaseMenuGameState
	{
		private MenuCube _mainMenuCube;
		private MenuCube _optionsMenuCube;

		private MenuCube _activeCube;

		private Body _ground;

		private readonly Platform _platform;

		public GameStateMenu( ContentManager content )
			: base( content )
		{
			_platform = Platform.Instance;
		}

		private void ActivateMenuCube( MenuCube cube )
		{
			_activeCube = cube;
			_activeCube.Spawn();
		}

		public override void OnUpdate( GameTime gameTime )
		{
			if ( _activeCube != null )
			{
				HandleInput();

				_activeCube.Update( gameTime );
			}

			_platform.PhysicsWorld.Step( Math.Min( ( float ) gameTime.ElapsedGameTime.TotalMilliseconds * 0.001f, ( 1f / 30f ) ) );
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
			_mainMenuCube.SetBackgroundImage( "main" );

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

		public override void OnEnter()
		{
			base.OnEnter();

			CreateMenus();
			CreateCubesGround();

			ActivateMenuCube( _mainMenuCube );

			_platform.SoundManager.PlayMusic( BackgroundMusic );
		}

		public override void OnClose()
		{
			base.OnClose();

			_mainMenuCube.Destroy();
			_optionsMenuCube.Destroy();

			_platform.SoundManager.PauseMusic();
		}
	}
}
