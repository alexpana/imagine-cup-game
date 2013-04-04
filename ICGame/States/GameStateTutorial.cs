using System;
using System.Collections.Generic;
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

		private bool _actionFreeze;
		private bool _actionReset;
		private bool _actionSpawn;
		private bool _actionToggleDebugView;
		private bool _debugViewState;

		public GameStateTutorial( ContentManager content )
		{
			_contentManager = content;
		}

		public override void OnUpdate( GameTime gameTime )
		{
			base.OnUpdate( gameTime );

			if ( Robot != null )
			{

				if ( Keyboard.GetState( PlayerIndex.One ).IsKeyDown( Keys.F ) )
				{
					if ( !_actionFreeze )
					{
						Robot.SetPhysicsEnabled( !Robot.PhysicsEntity.Enabled );
						if ( GameWorldManager.Instance.GetEntity( "crate1" ).PhysicsEntity.Enabled )
						{

							GameWorldManager.Instance.GetEntity( "crate1" ).SetPosition( new Vector3( -400f, -800f, 0f ) );
							GameWorldManager.Instance.GetEntity( "crate1" ).SetPhysicsEnabled( false );
						}
						else
						{
							GameWorldManager.Instance.GetEntity( "crate1" ).SetPhysicsEnabled( true );
						}
						_actionFreeze = true;
					}
				}

				if ( Keyboard.GetState( PlayerIndex.One ).IsKeyUp( Keys.F ) )
				{
					_actionFreeze = false;
				}

				if ( Keyboard.GetState( PlayerIndex.One ).IsKeyDown( Keys.R ) )
				{
					if ( !_actionReset )
					{
						Robot.SetPosition( Vector3.Zero );
						_actionReset = true;
					}
				}

				if ( Keyboard.GetState( PlayerIndex.One ).IsKeyUp( Keys.R ) )
				{
					_actionReset = false;
				}

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

			if ( Keyboard.GetState( PlayerIndex.One ).IsKeyDown( Keys.S ) )
			{
				if ( !_actionSpawn )
				{
					if ( Robot == null )
					{
						GameWorldManager.Instance.SpawnEntity( "Robot", "robotPlayer", new Vector3( 0f, 0f, 0f ), 2f );
						Robot = GameWorldManager.Instance.GetEntity( "robotPlayer" );
						Robot.RegisterComponent( "force", new SentientForceComponent( CursorManager.Instance.SceneNode ) );
					}
					else
					{
						GameWorldManager.Instance.RemoveEntity( "robotPlayer" );

						Robot = null;
					}
					_actionSpawn = true;
				}


			}

			if ( Keyboard.GetState( PlayerIndex.One ).IsKeyUp( Keys.S ) )
			{
				_actionSpawn = false;
			}

			if ( Keyboard.GetState( PlayerIndex.One ).IsKeyDown( Keys.O ) )
			{
				GameWorldManager.Instance.SaveState();
				//Robot.SetRotation( Robot.GetRotationRadians() - 0.4f * ( float ) gameTime.ElapsedGameTime.TotalSeconds );
			}
			else if ( Keyboard.GetState( PlayerIndex.One ).IsKeyDown( Keys.P ) )
			{
				GameWorldManager.Instance.LoadLastState();
				//Robot.SetRotation( Robot.GetRotationRadians() + 0.4f * ( float ) gameTime.ElapsedGameTime.TotalSeconds );
			}

		}

		public void RenderScene()
		{
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

			//first floor part
			for ( int i = 0; i < 25; i++ )
			{
				GameWorldManager.Instance.SpawnEntity( "Floor", "floor" + floorCount++, new Vector3( -300f + 60f * i, 0f, 0f ) );
			}

			Vector2 rotationPoint = new Vector2( -300f + 60f * 19, -10f );
			for ( int i = 20; i < 25; i++ )
			{
				TransformUtility.RotateTransformableAroundPoint2D( GameWorldManager.Instance.GetEntity( "floor" + i ), rotationPoint, 0.3f );
			}

			//left wall (first)
			for ( int i = 0; i < 5; i++ )
			{
				GameWorldManager.Instance.SpawnEntity( "Wall", "wall" + wallCount++, new Vector3( -355f, 0f + 60f * i, 0f ) );
			}

			//upgrade cube platform
			GameWorldManager.Instance.SpawnEntity( "Floor", "floor" + floorCount++, new Vector3( 500f + 60f, 10f, 0f ) );
			GameWorldManager.Instance.SpawnEntity( "Floor", "floor" + floorCount++, new Vector3( 500f + 60f * 2, 10f, 0f ) );
			GameWorldManager.Instance.SpawnEntity( "Floor", "floor" + floorCount++, new Vector3( 500f + 60f * 3, 10f, 0f ) );



		}

		public void LoadSemiStatics()
		{
			GameWorldManager.Instance.SpawnEntity( "Button", "button1", new Vector3( -350f, 46f, 0f ), 5f );
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
			GameWorldManager.Instance.SpawnEntity( "Robot", "robotPlayer", new Vector3( 500f, 100f, 0f ), 1.5f );

			Robot = GameWorldManager.Instance.GetEntity( "robotPlayer" );

			Robot.RegisterComponent( "force", new SentientForceComponent( CursorManager.Instance.SceneNode ) );
			Robot.RegisterComponent(
				"control",
				new CarControlComponent( new List<string> { "GearJoint1", "GearJoint2", "GearJoint3" }, new List<float>() { 7f, 7f, 7f } )
				);

			CameraController camControl = new CameraController( Robot, SceneManager.Instance.GetCurrentCamera() );
			ControllerRepository.Instance.RegisterController( "camcontrol", camControl );
			FrameUpdateManager.Instance.Register( camControl );

			Camera = GameWorldManager.Instance.GetEntity( "camera1" );

			GameWorldManager.Instance.SpawnEntity( "Crate", "crate1", new Vector3( -250, 100f, 0f ), 3f );
			//GameWorldManager.Instance.SpawnEntity( "Crate", "crate2", new Vector3( -250, 200f, 0f ), 3f );
		}

		public void LoadLevel()
		{

			LoadStatics();
			LoadSemiStatics();
			LoadDynamics();
		}

		public override void OnEnter()
		{

			LoadLevel();

			_actionFreeze = false;
			_actionSpawn = false;
			_actionReset = false;
			_debugViewState = false;
			_actionToggleDebugView = false;


			_debugView = new DebugViewXNA( Platform.Instance.PhysicsWorld );

			_debugView.LoadContent( Platform.Instance.Device, Platform.Instance.Content );
			//_debugView.RemoveFlags( DebugViewFlags.Joint );

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

			_contentManager.Unload();
		}
	}
}
