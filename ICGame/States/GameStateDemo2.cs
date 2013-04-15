//#define ALLOW_HACKS
using System.Collections.Generic;
using FarseerPhysics.Dynamics;
using FarseerPhysics.Dynamics.Joints;
using FarseerPhysics.Factories;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework.Media;
using VertexArmy.Content.Prefabs;
using VertexArmy.GameWorld;
using VertexArmy.Global;
using VertexArmy.Global.Controllers;
using VertexArmy.Global.Controllers.Components;
using VertexArmy.Global.Managers;
using VertexArmy.Physics.DebugView;
using VertexArmy.Utilities;

namespace VertexArmy.States
{
	internal class GameStateDemo2 : PlayableGameState
	{
		private ContentManager _contentManager;

		private DebugViewXNA _debugView;
		private Matrix _projection;
		private Matrix _view;

		public GameEntity Robot;
		public GameEntity Camera;

		private LevelPrefab _level;

		private bool _actionReset;
		private bool _actionToggleDebugView;
		private bool _debugViewState;
		private ButtonComponent _gravityButton;
		private bool _gravityChanged;
		private bool _endOfGameHintShown;

		private Vector2 _sucktionSpot;
		private CameraController _camControl;

		public GameStateDemo2( ContentManager content )
		{
			_contentManager = content;
			_gravityChanged = false;
			_endOfGameHintShown = false;
		}

		public override void OnUpdate( GameTime gameTime )
		{
			base.OnUpdate( gameTime );
#if ALLOW_HACKS
#endif

			if ( _gravityButton.ButtonState && !_gravityChanged )
			{
				Platform.Instance.PhysicsWorld.Gravity = Vector2.Zero;
				GameWorldManager.Instance.SaveState();
				_gravityChanged = true;
				HintManager.Instance.SpawnHint( "Gravity has been disabled, use the attraction \nforce to navigate through the rest of the level.", new Vector2( 100, 100 ), 4000, 1 );
			}

			if ( Robot != null )
			{

				if ( Robot.GetPosition().Y < -2000 )
				{
					GameWorldManager.Instance.LoadLastState();
				}

#if DEBUG
				if ( Keyboard.GetState( PlayerIndex.One ).IsKeyDown( Keys.Up ) )
				{
					if ( Robot.PhysicsEntity.GetCollisionLayer().Equals( Category.Cat1 ) )
					{
						Robot.PhysicsEntity.SetCollisionLayer( Category.Cat2 );
						Vector3 position = Robot.GetPosition();
						Robot.SetPosition( new Vector3( position.X, position.Y, -800f ) );
					}
				}
				if ( Keyboard.GetState( PlayerIndex.One ).IsKeyDown( Keys.Down ) )
				{
					if ( Robot.PhysicsEntity.GetCollisionLayer().Equals( Category.Cat2 ) )
					{
						Robot.PhysicsEntity.SetCollisionLayer( Category.Cat1 );
						Vector3 position = Robot.GetPosition();
						Robot.SetPosition( new Vector3( position.X, position.Y, 0f ) );
					}
				}
#endif

				if ( Keyboard.GetState( PlayerIndex.One ).IsKeyDown( Keys.R ) )
				{
					if ( !_actionReset )
					{
						GameWorldManager.Instance.LoadLastState();
						_actionReset = true;
					}
				}

				if ( Keyboard.GetState( PlayerIndex.One ).IsKeyUp( Keys.R ) )
				{
					_actionReset = false;
				}
			}
#if ALLOW_HACKS
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
#endif

		}

		public override void OnRender( GameTime gameTime )
		{
			base.OnRender( gameTime );

			if ( _debugViewState )
			{

				float scale = ( SceneManager.Instance.GetCurrentCamera().Parent.GetPosition().Z / 1024.0f );
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

		public void LoadStatics()
		{

		}

		public void LoadSemiStatics()
		{
			GameWorldManager.Instance.GetEntity( "button1" ).RegisterComponent(
				"active",
				new ButtonComponent( "ButtonJoint1" )
			);

			GameWorldManager.Instance.GetEntity( "button0" ).RegisterComponent(
				"active",
				new ButtonComponent( "ButtonJoint1", false, true )
			);

			GameWorldManager.Instance.GetEntity( "lifteddoor0" ).RegisterComponent(
				"doorHandle",
				new LiftedDoorComponent( GameWorldManager.Instance.GetEntity( "button1" ).GetComponent( "active" ), "DoorJoint1" )
			);

			_gravityButton = GameWorldManager.Instance.GetEntity( "button0" ).GetComponent( "active" ) as ButtonComponent;

			JointFactory.CreateFixedRevoluteJoint(
				Platform.Instance.PhysicsWorld,
				GameWorldManager.Instance.GetEntity( "floorbridge0" ).PhysicsEntity.GetBody( "FloorBody" ),
				UnitsConverter.ToSimUnits( new Vector2( 120f, 0f ) ),
				UnitsConverter.ToSimUnits( new Vector2( 2689f, 564 ) )
			);


			FixedDistanceJoint dj = JointFactory.CreateFixedDistanceJoint(
				Platform.Instance.PhysicsWorld,
				GameWorldManager.Instance.GetEntity( "floorbridge0" ).PhysicsEntity.GetBody( "FloorBody" ),
				UnitsConverter.ToSimUnits( new Vector2( -120f, -5f ) ),
				UnitsConverter.ToSimUnits( new Vector2( 2466f, 244f ) )
				);
			dj.Frequency = 1.5f;
			dj.DampingRatio = 0.355f;
		}

		public void LoadDynamics()
		{
			GameWorldManager.Instance.SpawnEntity( "Camera", "camera1", new Vector3( 0, -200, 800 ) );

			Robot = GameWorldManager.Instance.GetEntity( "robot0" );
			Robot.RegisterComponent( "force", new SentientForceComponent() );

			Robot.RegisterComponent(
				"control",
				new CarControlComponent( new List<string> { "GearJoint1", "GearJoint2", "GearJoint3" }, new List<float>() { 7f, 7f, 7f } )
				);


			_camControl = new CameraController( Robot, SceneManager.Instance.GetCurrentCamera() );
			ControllerRepository.Instance.RegisterController( "camcontrol", _camControl );
			FrameUpdateManager.Instance.Register( _camControl );

			Camera = GameWorldManager.Instance.GetEntity( "camera1" );
			Camera.SetRotation( 5f );
			FrameUpdateManager.Instance.Register( new GravityController() );
		}

		public void LoadTriggers()
		{
			GameWorldManager.Instance.SpawnEntity( "Trigger", "airpipeforce", new Vector3( -800, 400f, 0f ), 5f );
			GameWorldManager.Instance.GetEntity( "airpipeforce" ).RegisterComponent(
					"trigger",
					new BodyTriggerAreaComponent( new Vector2( 250f, 1200f ), AirPipeTrigger )
				);

			GameWorldManager.Instance.SpawnEntity( "Trigger", "endGame", new Vector3( -797f, 575f, 0f ), 1f );
			GameWorldManager.Instance.GetEntity( "endGame" ).RegisterComponent(
					"trigger",
					new BodyTriggerAreaComponent( new Vector2( 150f, 100f ), Robot.MainBody, EndGame )
				);

			Vector3 sucktionSpot3D = GameWorldManager.Instance.GetEntity( "pipe0" ).GetPosition() + Vector3.UnitY * 700f;
			_sucktionSpot = UnitsConverter.ToSimUnits( new Vector2( sucktionSpot3D.X, sucktionSpot3D.Y ) - ( Vector2.UnitY * 300f ) );
		}

		private void EndGame()
		{
			if ( _endOfGameHintShown ) { return; }

			_endOfGameHintShown = true;
			FrameUpdateManager.Instance.Unregister( _camControl );
			HintManager.Instance.SpawnHint( "Thanks for playing level 2!", new Vector2( 100, 100 ), 4000, 1, EndGameCallback );
		}

		public void EndGameCallback()
		{
			StateManager.Instance.PopState();
			StateManager.Instance.ChangeState( GameState.Menu );
		}

		private void AirPipeTrigger( Body b )
		{
			Vector2 force = _sucktionSpot - b.Position;
			force.Normalize();
			force = force * 0.1f;
			b.ApplyForce( force );
		}

		public void LoadLevel()
		{
			string _levelName = "level2";
			_level = PrefabRepository.Instance.GetLevelPrefab( @"Content\Levels\" + _levelName + ".eql" );
			GameWorldManager.Instance.SetState( _level._savedState );
			GameWorldManager.Instance.LoadLastState();
			LoadStatics();
			LoadSemiStatics();
			LoadDynamics();
			LoadTriggers();

		}

		public override void OnEnter()
		{
			SceneManager.Instance.UseDof = true;
			LoadLevel();
			GameWorldManager.Instance.SaveState();
			Platform.Instance.PhysicsWorld.Gravity = new Vector2( 0f, 9.82f );

			FrameUpdateManager.Instance.Register( HintManager.Instance );

			_actionReset = false;
			_debugViewState = false;
			_actionToggleDebugView = false;


			_debugView = new DebugViewXNA( Platform.Instance.PhysicsWorld );

			_debugView.LoadContent( Platform.Instance.Device, Platform.Instance.Content );
			//_debugView.RemoveFlags( DebugViewFlags.Joint );
			//_debugView.AppendFlags( DebugViewFlags.PerformanceGraph );

			_debugView.TextColor = Color.Black;

			_view = Matrix.Identity;

			Song song = _contentManager.Load<Song>( "music/Beluga_-_Lost_In_Outer_Space" );
			Platform.Instance.SoundManager.PlayMusic( song );
			FrameUpdateManager.Instance.Register( SceneManager.Instance );

			SceneManager.Instance.SortByLayer();
		}

		public override void OnClose()
		{
			GameWorldManager.Instance.Clear();
			ControllerRepository.Instance.Clear();
			PhysicsContactManager.Instance.Clear();
			FrameUpdateManager.Instance.Clear();
			Platform.Instance.PhysicsWorld.Clear();
			SceneManager.Instance.Clear();
			Platform.Instance.SoundManager.StopMusic();

			_contentManager.Unload();

			SceneManager.Instance.UseDof = false;
			HintManager.Instance.Clear();
		}


		public void LoadLastSateCallback()
		{
			GameWorldManager.Instance.LoadLastState();
		}

	}
}
