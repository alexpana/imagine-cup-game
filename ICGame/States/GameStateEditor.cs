using System;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.GamerServices;
using Microsoft.Xna.Framework.Input;
using VertexArmy.Content.Prefabs;
using VertexArmy.GameWorld;
using VertexArmy.Global;
using VertexArmy.Global.Controllers;
using VertexArmy.Global.Managers;
using VertexArmy.Physics.DebugView;
using VertexArmy.Utilities;

namespace VertexArmy.States
{
	internal class GameStateEditor : PlayableGameState
	{
		private ContentManager _contentManager;

		private DebugViewXNA _debugView;
		private Matrix _projection;
		private Matrix _view;

		public GameEntity Robot;
		public GameEntity Camera;

		private LevelPrefab _level;

		private bool _actionToggleDebugView;
		private bool _debugViewState;

		private bool _levelLoaded;
		private bool _saveAction;
		private string _levelName;

		public GameStateEditor( ContentManager content )
		{
			_contentManager = content;
			_saveAction = false;
		}

		public override void OnUpdate( GameTime gameTime )
		{
			if ( !_levelLoaded )
			{
				return;
			}

			base.OnUpdate( gameTime );
			_pausePhysics = true;



			if ( Keyboard.GetState().IsKeyDown( Keys.LeftControl ) && Keyboard.GetState().IsKeyDown( Keys.S ) && !_saveAction )
			{
				_saveAction = true;
				GameWorldManager.Instance.SaveState();
				_level = PrefabRepository.Instance.GetLevelPrefab( @"Content\Levels\" + _levelName + ".eql" );
				_level.SetState( GameWorldManager.Instance.GetState() );
				_level.SerializeLevel();
				HintManager.Instance.SpawnHint( "Saved " + _levelName, new Vector2( 200, 200 ), 5000, 13 );
			}
			else if ( Keyboard.GetState().IsKeyUp( Keys.S ) || Keyboard.GetState().IsKeyUp( Keys.LeftControl ) )
			{
				_saveAction = false;
			}

			if ( Keyboard.GetState( PlayerIndex.One ).IsKeyDown( Keys.D ) )
			{
				if ( !_actionToggleDebugView )
				{
					_debugViewState = !_debugViewState;
					_actionToggleDebugView = true;
				}
			}

			if ( Keyboard.GetState( PlayerIndex.One ).IsKeyUp( Keys.D ) )
			{
				_actionToggleDebugView = false;
			}
		}

		public override void OnRender( GameTime gameTime )
		{
			if ( !_levelLoaded )
			{
				return;
			}

			base.OnRender( gameTime );

			if ( _debugViewState )
			{
				float scale = ( SceneManager.Instance.GetCurrentCamera().Parent.GetPosition().Z / 800.0f );
				_projection = Matrix.CreateOrthographicOffCenter(
					UnitsConverter.ToSimUnits( SceneManager.Instance.GetCurrentCamera().Parent.GetPosition().X - Platform.Instance.Device.Viewport.Width / 2f * scale ),
					UnitsConverter.ToSimUnits( SceneManager.Instance.GetCurrentCamera().Parent.GetPosition().X + Platform.Instance.Device.Viewport.Width / 2f * scale ),
					UnitsConverter.ToSimUnits( -SceneManager.Instance.GetCurrentCamera().Parent.GetPosition().Y + Platform.Instance.Device.Viewport.Height / 2f * scale ),
					UnitsConverter.ToSimUnits( -SceneManager.Instance.GetCurrentCamera().Parent.GetPosition().Y - Platform.Instance.Device.Viewport.Height / 2f * scale ),
					0f,
					1f
					);

				_debugView.RenderDebugData( ref _projection, ref _view );
			}
		}

		public void LoadLevel()
		{
			GameWorldManager.Instance.SpawnEntity( "Camera", "cameraEditor", new Vector3( 0, 0, 800 ) );
			FreeCameraController camControl = new FreeCameraController( SceneManager.Instance.GetCurrentCamera() );
			ControllerRepository.Instance.RegisterController( "camcontrol", camControl );
			FrameUpdateManager.Instance.Register( camControl );
			FrameUpdateManager.Instance.Register( new EditorToolsController() );
		}

		public override void OnEnter()
		{
			SceneManager.Instance.UsePostDraw = true;
			Guide.BeginShowKeyboardInput( PlayerIndex.One, "Select level", "Specify the name of the level", "level1", LevelNameInputCallback, null );
		}

		private void LevelNameInputCallback( IAsyncResult ar )
		{
			_levelName = Guide.EndShowKeyboardInput( ar );

			// nothing to do yet
			if ( !ar.IsCompleted )
			{
				return;
			}

			_level = PrefabRepository.Instance.GetLevelPrefab( @"Content\Levels\" + _levelName + ".eql" );
			GameWorldManager.Instance.SetState( _level._savedState );
			GameWorldManager.Instance.LoadLastState();

			LoadLevel();
			Platform.Instance.PhysicsWorld.Gravity = Vector2.UnitY * Platform.Instance.PhysicsWorld.Gravity.Length();

			_debugViewState = false;
			_actionToggleDebugView = false;

			_debugView = new DebugViewXNA( Platform.Instance.PhysicsWorld );

			_debugView.LoadContent( Platform.Instance.Device, Platform.Instance.Content );
			//_debugView.RemoveFlags( DebugViewFlags.Joint );
			//_debugView.AppendFlags( DebugViewFlags.PerformanceGraph );

			_debugView.TextColor = Color.Black;

			_view = Matrix.Identity;

			//Song song = _contentManager.Load<Song>( "music/Beluga_-_Lost_In_Outer_Space" );
			//Platform.Instance.SoundManager.PlayMusic( song );

			FrameUpdateManager.Instance.Register( HintManager.Instance );
			FrameUpdateManager.Instance.Register( SceneManager.Instance );

			_levelLoaded = true;
		}

		public override void OnClose()
		{
			GameWorldManager.Instance.Clear();
			ControllerRepository.Instance.Clear();
			PhysicsContactManager.Instance.Clear();
			FrameUpdateManager.Instance.Clear();
			Platform.Instance.PhysicsWorld.Clear();
			SceneManager.Instance.Clear();
			SceneManager.Instance.UsePostDraw = false;
			Platform.Instance.SoundManager.StopMusic();

			_contentManager.Unload();

			HintManager.Instance.Clear( );
		}

		public void LoadLastSateCallback()
		{
			GameWorldManager.Instance.LoadLastState();
		}
	}
}
