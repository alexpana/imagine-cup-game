//#define ALLOW_HACKS
using System;
using System.Collections.Generic;
using FarseerPhysics.Dynamics;
using FarseerPhysics.Dynamics.Joints;
using FarseerPhysics.Factories;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework.Media;
using VertexArmy.GameWorld;
using VertexArmy.Global;
using VertexArmy.Global.Controllers;
using VertexArmy.Global.Controllers.Components;
using VertexArmy.Global.Managers;
using VertexArmy.Physics.DebugView;
using VertexArmy.Utilities;

namespace VertexArmy.States
{
	internal class GameStateTutorial : PlayableGameState
	{
		private ContentManager _contentManager;

		private DebugViewXNA _debugView;
		private Matrix _projection;
		private Matrix _view;

		public GameEntity Robot;
		public GameEntity Camera;

		private bool _actionReset;
		private bool _actionToggleDebugView;
		private bool _debugViewState;

		private bool _hint1, _hint2, _hint3, _hint4;

		public GameStateTutorial( ContentManager content )
		{
			_contentManager = content;
		}

		public override void OnUpdate( GameTime gameTime )
		{
			base.OnUpdate( gameTime );

			if ( Robot != null )
			{

				if ( Robot.GetPosition().Y < -2000 )
				{
					GameWorldManager.Instance.LoadLastState();
				}

				if ( Keyboard.GetState( PlayerIndex.One ).IsKeyDown( Keys.Up ) )
				{
					if ( Robot.PhysicsEntity.GetCollisionLayer().Equals( Category.Cat1 ) )
					{
						Robot.PhysicsEntity.SetCollisionLayer( Category.Cat2 );
						Vector3 position = Robot.GetPosition();
						Robot.SetPosition( new Vector3( position.X, position.Y, -300f ) );
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
				_projection = Matrix.CreateOrthographicOffCenter(
					UnitsConverter.ToSimUnits( SceneManager.Instance.GetCurrentCamera().Parent.GetPosition().X - Platform.Instance.Device.Viewport.Width / 2f ),
					UnitsConverter.ToSimUnits( SceneManager.Instance.GetCurrentCamera().Parent.GetPosition().X + Platform.Instance.Device.Viewport.Width / 2f ),
					UnitsConverter.ToSimUnits( -SceneManager.Instance.GetCurrentCamera().Parent.GetPosition().Y + Platform.Instance.Device.Viewport.Height / 2f ),
					UnitsConverter.ToSimUnits( -SceneManager.Instance.GetCurrentCamera().Parent.GetPosition().Y - Platform.Instance.Device.Viewport.Height / 2f ),
					0f,
					1f
					);

				_debugView.DrawString( 1, 1, "(R)eset, (F)reeze, (D)ebug to toggle debugview, Arrows to move." );
				_debugView.DrawString( 1, 20, "(S) to spawn/unspawn, O,P to rotate." );
				_debugView.DrawString( 1, 39, "Mouse:" + Mouse.GetState().X + ", " + Mouse.GetState().Y );
				_debugView.RenderDebugData( ref _projection, ref _view );
			}


		}

		public void LoadStatics()
		{
			int floorCount = 0;
			int wallCount = 0;
			int roofCount = 0;

			// roof part
			for ( int i = 0; i < 80; i++ )
			{
				GameWorldManager.Instance.SpawnEntity( "Floor", "roof" + roofCount++, new Vector3( -300f + 60f * i, 500f, 0f ) );
			}

			//first floor part
			for ( int i = 0; i < 20; i++ )
			{
				GameWorldManager.Instance.SpawnEntity( "Floor", "floor" + floorCount++, new Vector3( -300f + 60f * i, 0f, 0f ), 1f );
			}

			Vector2 rotationPoint = new Vector2( -300f + 60f * 19, -10f );
			for ( int i = 20; i < 25; i++ )
			{
				GameWorldManager.Instance.SpawnEntity( "Floor", "floor" + floorCount++, new Vector3( -300f + 60f * i, -9f, 0f ) );
				TransformUtility.RotateTransformableAroundPoint2D( GameWorldManager.Instance.GetEntity( "floor" + i ), rotationPoint, 0.3f );
			}

			//first floor part background
			GameWorldManager.Instance.SpawnEntity( "Floor", "floor" + floorCount++, new Vector3( -300f + 60f * 10, 0f, -300f ), new Vector3( 20f, 1f, 1f ), Category.Cat2 );

			for ( int i = 0; i < 5; i++ )
			{
				GameWorldManager.Instance.SpawnEntity( "Floor", "floor" + floorCount++, new Vector3( 1183f + 60f * i, 87.5f, 0f ) );
			}

			for ( int i = 5; i < 7; i++ )
			{
				GameWorldManager.Instance.SpawnEntity( "Floor", "floor" + floorCount++, new Vector3( 1183f + 60f * i, 10f, 0f ) );
			}

			for ( int i = 7; i < 15; i++ )
			{
				GameWorldManager.Instance.SpawnEntity( "Floor", "floor" + floorCount++, new Vector3( 1183f + 60f * i, 87.5f, 0f ) );
			}

			//left wall (first)
			for ( int i = 0; i < 10; i++ )
			{
				GameWorldManager.Instance.SpawnEntity( "Wall", "wall" + wallCount++, new Vector3( -335f, 0f + 60f * i, 0f ) );
			}

			// door walls
			for ( int i = 0; i < 5; i++ )
			{
				GameWorldManager.Instance.SpawnEntity( "Wall", "wall" + wallCount++, new Vector3( 15f, 240f + 60f * i, 0f ) );
			}
			for ( int i = 0; i < 5; i++ )
			{
				GameWorldManager.Instance.SpawnEntity( "Wall", "wall" + wallCount++, new Vector3( 85f, 240f + 60f * i, 0f ) );
			}

			//wall 1
			for ( int i = 0; i < 2; i++ )
			{
				GameWorldManager.Instance.SpawnEntity( "Wall", "wall" + wallCount++, new Vector3( 2048f, 52f - 60f * i, 0f ) );
			}

			//wall air
			for ( int i = 0; i < 3; i++ )
			{
				GameWorldManager.Instance.SpawnEntity( "Wall", "wall" + wallCount++, new Vector3( 2370f, 344f + 60f * i, 0f ) );
			}

			//floor air
			for ( int i = 0; i < 3; i++ )
			{
				GameWorldManager.Instance.SpawnEntity( "Floor", "floor" + floorCount++, new Vector3( 2215f + 60f * i, 320f, 0f ) );
			}

			//floor
			for ( int i = 0; i < 6; i++ )
			{
				GameWorldManager.Instance.SpawnEntity( "Floor", "floor" + floorCount++, new Vector3( 2070f + 60f * i, -43f, 0f ) );
			}

			//floor
			for ( int i = 8; i < 16; i++ )
			{
				GameWorldManager.Instance.SpawnEntity( "Floor", "floor" + floorCount++, new Vector3( 2070f + 60f * i, -43f, 0f ) );
			}

			//wall 2
			for ( int i = 0; i < 3; i++ )
			{
				GameWorldManager.Instance.SpawnEntity( "Wall", "wall" + wallCount++, new Vector3( 2395f, -78 - 60f * i, 0f ) );
			}

			//floor
			for ( int i = 6; i < 12; i++ )
			{
				GameWorldManager.Instance.SpawnEntity( "Floor", "floor" + floorCount++, new Vector3( 2070f + 60f * i, -180f, 0f ) );
			}

			//floor
			for ( int i = 14; i < 22; i++ )
			{
				GameWorldManager.Instance.SpawnEntity( "Floor", "floor" + floorCount++, new Vector3( 2070f + 60f * i, -180f, 0f ) );
			}

			//floor
			for ( int i = 12; i < 14; i++ )
			{
				GameWorldManager.Instance.SpawnEntity( "Floor", "floor" + floorCount++, new Vector3( 2070f + 60f * i, -250f, 0f ) );
			}

			//floor
			for ( int i = 20; i < 30; i++ )
			{
				GameWorldManager.Instance.SpawnEntity( "Floor", "floor" + floorCount++, new Vector3( 2080f + 60f * i, -43f, 0f ) );
			}

			//wall 2
			for ( int i = 0; i < 3; i++ )
			{
				GameWorldManager.Instance.SpawnEntity( "Wall", "wall" + wallCount++, new Vector3( 2080f + 60f * 20, -78 - 60f * i, 0f ) );
			}

			GameWorldManager.Instance.SpawnEntity( "Floor", "floor" + floorCount, new Vector3( 2050f + 60f * 20, -170, 0f ) );
			GameWorldManager.Instance.GetEntity( "floor" + floorCount ).SetRotation( -0.3f );
			floorCount++;

			GameWorldManager.Instance.SpawnEntity( "FloorBridge", "bridge", new Vector3( 2040f + 60f * 18, -43f, 0f ) );
			JointFactory.CreateFixedRevoluteJoint(
				Platform.Instance.PhysicsWorld,
				GameWorldManager.Instance.GetEntity( "bridge" ).PhysicsEntity.GetBody( "FloorBody" ),
				UnitsConverter.ToSimUnits( new Vector2( -120f, 0f ) ),
				UnitsConverter.ToSimUnits( new Vector2( 2040f + 60f * 16, -43 ) )
				);


			FixedDistanceJoint dj = JointFactory.CreateFixedDistanceJoint(
				Platform.Instance.PhysicsWorld,
				GameWorldManager.Instance.GetEntity( "bridge" ).PhysicsEntity.GetBody( "FloorBody" ),
				UnitsConverter.ToSimUnits( new Vector2( 120f, -5f ) ),
				UnitsConverter.ToSimUnits( new Vector2( 2040f + 60f * 20, -180f ) )
				);
			dj.Frequency = 2f;
			dj.DampingRatio = 0.4f;



			//upgrade cube platform
			GameWorldManager.Instance.SpawnEntity( "Floor", "floor" + floorCount++, new Vector3( 500f + 60f, 10f, 0f ) );
			GameWorldManager.Instance.SpawnEntity( "Floor", "floor" + floorCount++, new Vector3( 500f + 60f * 2, 10f, 0f ) );
			GameWorldManager.Instance.SpawnEntity( "Floor", "floor" + floorCount++, new Vector3( 500f + 60f * 3, 10f, 0f ) );

		}

		public void LoadSemiStatics()
		{
			GameWorldManager.Instance.SpawnEntity( "Button", "button1", new Vector3( -330f, 46f, 0f ), 5f );
			GameWorldManager.Instance.GetEntity( "button1" ).RegisterComponent(
				"active",
				new ButtonComponent( "ButtonJoint1" )
				);

			GameWorldManager.Instance.GetEntity( "button1" ).SetRotation( ( float ) Math.PI / 2f );

			GameWorldManager.Instance.SpawnEntity( "LiftedDoor", "door", new Vector3( 50, 330f, 0f ), 1f );
			GameWorldManager.Instance.GetEntity( "door" ).RegisterComponent(
				"doorHandle",
				new LiftedDoorComponent( GameWorldManager.Instance.GetEntity( "button1" ).GetComponent( "active" ), "DoorJoint1" )
			);
		}

		public void LoadDynamics()
		{
			GameWorldManager.Instance.SpawnEntity( "Camera", "camera1", new Vector3( 0, -200, 800 ) );
			GameWorldManager.Instance.SpawnEntity( "Robot", "robotPlayer", new Vector3( -150f, 100f, 0f ), new Vector3( 1.5f, 1.5f, 2.5f ) );
			GameWorldManager.Instance.SpawnEntity( "WallBackground", "wallBackground1",
				new Vector3( 0f, 0f, -800f ), Quaternion.CreateFromAxisAngle( Vector3.UnitX, -0.3f ), 150 );

			Robot = GameWorldManager.Instance.GetEntity( "robotPlayer" );

			Robot.RegisterComponent(
				"control",
				new CarControlComponent( new List<string> { "GearJoint1", "GearJoint2", "GearJoint3" }, new List<float>() { 7f, 7f, 7f } )
				);

			GameWorldManager.Instance.SpawnEntity( "Crate", "crate1", new Vector3( -250, 100f, 0f ), 3f );
			GameWorldManager.Instance.SpawnEntity( "Crate", "crate2", new Vector3( 1700, 130f, 0f ), 4f );
			GameWorldManager.Instance.SpawnEntity( "Crate", "crate3", new Vector3( 2200, 360f, 0f ), 5f );
			GameWorldManager.Instance.SpawnEntity( "Crate", "crate4", new Vector3( 2450, -90f, 0f ), 4f );

			GameWorldManager.Instance.SpawnEntity( "Crate", "crate5", new Vector3( -80, 100f, -300f ), 4f, Category.Cat2 );
			GameWorldManager.Instance.SpawnEntity( "Crate", "crate6", new Vector3( 0, 100f, -300f ), 4f, Category.Cat2 );
			GameWorldManager.Instance.SpawnEntity( "Crate", "crate7", new Vector3( -50, 150f, -300f ), 3f, Category.Cat2 );

			CameraController camControl = new CameraController( Robot, SceneManager.Instance.GetCurrentCamera() );
			ControllerRepository.Instance.RegisterController( "camcontrol", camControl );
			FrameUpdateManager.Instance.Register( camControl );

			Camera = GameWorldManager.Instance.GetEntity( "camera1" );
			Camera.SetRotation( 5f );
			FrameUpdateManager.Instance.Register( new GravityController() );
		}

		public void LoadTriggers()
		{
			GameWorldManager.Instance.SpawnEntity( "Trigger", "upgradeCube1", new Vector3( 500f + 60f * 2, 70f, 0f ) );
			GameWorldManager.Instance.GetEntity( "upgradeCube1" ).RegisterComponent(
					"trigger",
					new BodyTriggerAreaComponent( new Vector2( 10f, 10f ), Robot.MainBody, UpgradeCube1Callback )
				);

			GameWorldManager.Instance.SpawnEntity( "Trigger", "death1", new Vector3( 1492, 60f, 0f ) );
			GameWorldManager.Instance.GetEntity( "death1" ).RegisterComponent(
					"trigger",
					new BodyTriggerAreaComponent( new Vector2( 10f, 10f ), Robot.MainBody, LoadLastSateCallback )
				);

			GameWorldManager.Instance.SpawnEntity( "Trigger", "death2", new Vector3( 2455, -60f, 0f ) );
			GameWorldManager.Instance.GetEntity( "death2" ).RegisterComponent(
					"trigger",
					new BodyTriggerAreaComponent( new Vector2( 10f, 10f ), Robot.MainBody, LoadLastSateCallback )
				);

			GameWorldManager.Instance.SpawnEntity( "Trigger", "endGame", new Vector3( 3500, 30f, 0f ) );
			GameWorldManager.Instance.GetEntity( "endGame" ).RegisterComponent(
					"trigger",
					new BodyTriggerAreaComponent( new Vector2( 10f, 10f ), Robot.MainBody, EndGameCallback )
				);

			GameWorldManager.Instance.SpawnEntity( "Trigger", "hint1", new Vector3( -150, 60f, 0f ) );
			GameWorldManager.Instance.GetEntity( "hint1" ).RegisterComponent(
					"trigger",
					new BodyTriggerAreaComponent( new Vector2( 10f, 10f ), Robot.MainBody, Hint1 )
				);

			GameWorldManager.Instance.SpawnEntity( "Trigger", "hint2", new Vector3( 320, 60f, 0f ) );
			GameWorldManager.Instance.GetEntity( "hint2" ).RegisterComponent(
					"trigger",
					new BodyTriggerAreaComponent( new Vector2( 10f, 10f ), Robot.MainBody, Hint2 )
				);

			GameWorldManager.Instance.SpawnEntity( "Trigger", "hint3", new Vector3( 1200, 150f, 0f ) );
			GameWorldManager.Instance.GetEntity( "hint3" ).RegisterComponent(
					"trigger",
					new BodyTriggerAreaComponent( new Vector2( 10f, 10f ), Robot.MainBody, Hint3 )
				);

			GameWorldManager.Instance.SpawnEntity( "Trigger", "hint4", new Vector3( 1900, 150f, 0f ) );
			GameWorldManager.Instance.GetEntity( "hint4" ).RegisterComponent(
					"trigger",
					new BodyTriggerAreaComponent( new Vector2( 10f, 10f ), Robot.MainBody, Hint4 )
				);
		}

		public void LoadLevel()
		{

			LoadStatics();
			LoadSemiStatics();
			LoadDynamics();
			LoadTriggers();


		}

		public override void OnEnter()
		{

			LoadLevel();
			GameWorldManager.Instance.SaveState();
			Platform.Instance.PhysicsWorld.Gravity = Vector2.UnitY * Platform.Instance.PhysicsWorld.Gravity.Length();

			FrameUpdateManager.Instance.Register( HintManager.Instance );

			_hint1 = _hint2 = _hint3 = _hint4 = false;
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

			FrameUpdateManager.Instance.Register( SceneManager.Instance );

			_contentManager.Unload();
		}

		public void UpgradeCube1Callback()
		{
			if ( Robot.GetComponent( "force" ) == null )
			{
				Robot.RegisterComponent( "force", new SentientForceComponent( CursorManager.Instance.SceneNode ) );
				GameWorldManager.Instance.SaveState();
				string Text = "Press Mouse1 to pull objects towards the robot. \nPress Mouse2 to push away objects from the robot.";
				HintManager.Instance.SpawnHint( Text, new Vector2( 100, 100 ), 7000, 1 );
			}
		}

		public void LoadLastSateCallback()
		{
			GameWorldManager.Instance.LoadLastState();
		}

		public void EndGameCallback()
		{
			StateManager.Instance.PopState();
			StateManager.Instance.ChangeState( GameState.Menu );
		}

		public void Hint1()
		{
			if ( !_hint1 )
			{

				string Text = "Crates can be pushed around.\nTry pushing that crate towards the wall button.";
				HintManager.Instance.SpawnHint( Text, new Vector2( 100, 100 ), 4000, 1 );
				_hint1 = true;
			}
		}

		public void Hint2()
		{
			if ( !_hint2 )
			{
				string Text = "Step onto the platform ahead for an upgrade.";
				HintManager.Instance.SpawnHint( Text, new Vector2( 100, 150 ), 2000, 1 );
				_hint2 = true;
			}
		}

		public void Hint3()
		{
			if ( !_hint3 )
			{
				string Text = "Sometimes you can get stuck.\nPress R to reverse to the last checkpoint.";
				HintManager.Instance.SpawnHint( Text, new Vector2( 100, 180 ), 5000, 1 );
				_hint3 = true;
			}
		}

		public void Hint4()
		{
			if ( !_hint4 )
			{
				string Text = "Some puzzles can be harder to overcome.\nRemember to press R incase you get stuck.";
				HintManager.Instance.SpawnHint( Text, new Vector2( 100, 70 ), 5000, 1 );
				_hint4 = true;
			}
		}
	}
}
