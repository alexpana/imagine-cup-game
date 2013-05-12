using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Input;
using UnifiedInputSystem;
using UnifiedInputSystem.Extensions;
using UnifiedInputSystem.Input;
using VertexArmy.Content.Prefabs;
using VertexArmy.Global;
using VertexArmy.Global.Controllers;
using VertexArmy.Global.Managers;
using VertexArmy.Physics.DebugView;
using VertexArmy.Utilities;

namespace VertexArmy.States
{
	internal class GameStateEditor : PlayableGameState
	{
		private readonly ContentManager _contentManager;

		private DebugViewXNA _debugView;
		private Matrix _projection;
		private Matrix _view;

		private LevelPrefab _level;

		private bool _actionToggleDebugView;
		private bool _debugViewState;
		private bool _debugViewGrid;

		private bool _levelLoaded;
		private bool _saveAction;
		private string _levelName;
		private readonly InputAggregator _inputAggregator;

		private float _debugViewGridstep;
		private readonly Color _gridColor;

		public GameStateEditor( ContentManager content )
		{
			_contentManager = content;
			_saveAction = false;
			_inputAggregator = Platform.Instance.Input;
			_debugViewGridstep = UnitsConverter.ToSimUnits( 1f );
			_gridColor = new Color( 128, 128, 128, 80 );
			_debugViewGrid = true;
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

			if ( _inputAggregator.HasEvent( UISButton.G, true ) )
			{
				_debugViewGrid = !_debugViewGrid;
			}

			// debug view segments
			_debugViewGridstep = UnitsConverter.ToSimUnits( 1f );
			if ( _inputAggregator.HasEvent( UISButton.LeftControl ) || _inputAggregator.HasEvent( UISButton.RightControl ) )
			{
				_debugViewGridstep *= 2f;
			}
			else if ( _inputAggregator.HasEvent( UISButton.LeftShift ) || _inputAggregator.HasEvent( UISButton.RightShift ) )
			{
				_debugViewGridstep *= 0.1f;
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
				float scale = ( SceneManager.Instance.GetCurrentCamera().Parent.GetPosition().Z / 1024.0f );

				float left = UnitsConverter.ToSimUnits( SceneManager.Instance.GetCurrentCamera().Parent.GetPosition().X - Platform.Instance.Device.Viewport.Width / 2f * scale );

				float right = UnitsConverter.ToSimUnits( SceneManager.Instance.GetCurrentCamera().Parent.GetPosition().X + Platform.Instance.Device.Viewport.Width / 2f * scale );

				float top = UnitsConverter.ToSimUnits( -SceneManager.Instance.GetCurrentCamera().Parent.GetPosition().Y + Platform.Instance.Device.Viewport.Height / 2f * scale );

				float bottom = UnitsConverter.ToSimUnits( -SceneManager.Instance.GetCurrentCamera().Parent.GetPosition().Y - Platform.Instance.Device.Viewport.Height / 2f * scale );

				_projection = Matrix.CreateOrthographicOffCenter(
					left,
					right,
					top,
					bottom,
					0f,
					1f
					);

				if ( _debugViewGrid )
				{
					_debugView.BeginCustomDraw( ref _projection, ref _view );
					for ( float i = left - left % _debugViewGridstep; i < right; i += _debugViewGridstep )
					{
						_debugView.DrawSegment( new Vector2( i, top ), new Vector2( i, bottom ), _gridColor );
					}
					for ( float i = bottom - bottom % _debugViewGridstep; i < top; i += _debugViewGridstep )
					{
						_debugView.DrawSegment( new Vector2( left, i ), new Vector2( right, i ), _gridColor );
					}
					_debugView.EndCustomDraw();
				}

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
			_levelName = "level1";
			OnLevelSelected();
		}

		private void OnLevelSelected()
		{
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

			HintManager.Instance.Clear();
		}

		public void LoadLastSateCallback()
		{
			GameWorldManager.Instance.LoadLastState();
		}
	}
}
