using System.Collections.Generic;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Input;
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

		public GameStateMenu( ContentManager content )
			: base( content )
		{
			_platform = Platform.Instance;
		}

		public override void OnUpdate( GameTime gameTime )
		{
			if ( _activeCube != null )
			{
				HandleInput();

				_activeCube.Update( gameTime );
			}

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

			CreateMenus();

			ActivateMenuCube( _mainMenuCube );

			_platform.SoundManager.PlayMusic( BackgroundMusic );

			GameWorldManager.Instance.SpawnEntity( CameraPrefab.PrefabName, "menu_camera", new Vector3( 0, 0, 100 ) );
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
