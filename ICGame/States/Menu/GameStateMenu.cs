using System;
using System.Collections.Generic;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework.Media;
using VertexArmy.GameWorld.Prefabs;
using VertexArmy.GameWorld.Prefabs.Structs;
using VertexArmy.Global;
using VertexArmy.Global.Managers;

namespace VertexArmy.States.Menu
{
	public class GameStateMenu : IGameState
	{
		private readonly MenuCube _mainMenuCube;
		private readonly MenuCube _optionsMenuCube;

		private MenuCube _activeCube;

		private SpriteBatch _spriteBatch;
		private SpriteFont _font;
		private Song _backgroundMusic;

		private readonly ContentManager _content;

		private bool _shouldRotate = false;
		private Quaternion _rotation = new Quaternion();

		public GameStateMenu( ContentManager content )
		{
			_content = content;

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

				_shouldRotate = true;
			}
			else if ( Platform.Instance.Input.IsKeyPressed( Keys.Right, false ) )
			{
				_activeCube.SelectedItem = ( _activeCube.SelectedItem + 1 ) % _activeCube.Items.Count;
				_shouldRotate = true;
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

			if ( _shouldRotate )
			{
				_rotation = Quaternion.Concatenate( _rotation, Quaternion.CreateFromAxisAngle( Vector3.UnitY, MathHelper.ToRadians( 10 ) ) );
				_shouldRotate = false;
			}

			GameWorldManager.Instance.GetEntity( "mesh1" ).SetRotation( _rotation );

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
			ActivateMenuCube( _mainMenuCube );
			_spriteBatch = new SpriteBatch( Platform.Instance.Device );
			_font = Platform.Instance.Content.Load<SpriteFont>( "fonts/SpriteFont1" );
			_backgroundMusic = _content.Load<Song>( "music/proto1_menu" );

			Platform.Instance.SoundPlayer.PlayMusic( _backgroundMusic );

			PrefabEntity mesh = new PrefabEntity();

			GameWorldManager.Instance.SpawnEntity( "camera", "camera1", new Vector3( 0, 0, 100 ) );

			MeshSceneNodePrefab crateSceneNode = new MeshSceneNodePrefab
			{
				Material = "CelShadingMaterial",
				Mesh = "models/menu_cube",
				Name = "Mesh",
				LocalRotation = new Quaternion( new Vector3( 0f, 0f, 0f ), 0f )
			};

			mesh.RegisterMeshSceneNode( crateSceneNode );
			GameWorldManager.Instance.SpawnEntity( mesh, "mesh1", new Vector3( 0f, 0, 0f ) );
		}

		public void OnClose()
		{
			SceneManager.Instance.Clear();
			GameWorldManager.Instance.Clear();
			Platform.Instance.SoundPlayer.StopMusic();
		}
	}
}
