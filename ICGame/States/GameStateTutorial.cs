using System;
using System.Collections.Generic;
using FarseerPhysics.Dynamics;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Input;
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

		private Body _ground;

		public GameEntity Robot;
		public GameEntity Camera;
		private float _cameraPosition;
		private float _cameraError = 0.7f;
		private bool _cameraMoving;

		private float _cameraStep;
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

				if ( !_cameraMoving && Math.Abs( _cameraPosition - Robot.GetPosition().X ) > _cameraError )
				{
					_cameraMoving = true;
					_cameraStep = ( -1 ) * ( _cameraPosition - Robot.GetPosition().X ) / 15;
				}

				if ( _cameraMoving )
				{
					_cameraPosition += _cameraStep;

					if ( Math.Abs( _cameraPosition - Robot.GetPosition().X ) <= _cameraError / 2 )
					{
						_cameraMoving = false;
					}
					else
					{
						_cameraStep = ( -1 ) * ( _cameraPosition - Robot.GetPosition().X ) / 15;
					}
				}

				if ( Keyboard.GetState( PlayerIndex.One ).IsKeyDown( Keys.F ) )
				{
					if ( !_actionFreeze )
					{
						Robot.SetPhysicsEnabled( !Robot.PhysicsEntity.Enabled );
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

				if ( Keyboard.GetState( PlayerIndex.One ).IsKeyDown( Keys.O ) )
				{
					Robot.SetRotation( Robot.GetRotationRadians() - 0.4f * ( float ) gameTime.ElapsedGameTime.TotalSeconds );
				}
				else if ( Keyboard.GetState( PlayerIndex.One ).IsKeyDown( Keys.P ) )
				{
					Robot.SetRotation( Robot.GetRotationRadians() + 0.4f * ( float ) gameTime.ElapsedGameTime.TotalSeconds );
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
						GameWorldManager.Instance.SpawnEntity( "robot", "robotPlayer", new Vector3( 0f, -1000f, 0f ) );
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

		}

		public void RenderScene()
		{
		}

		public override void OnRender( GameTime gameTime )
		{
			base.OnRender( gameTime );

			if ( _debugViewState )
			{
				float cameraPosition = UnitsConverter.ToSimUnits( _cameraPosition );
				_projection = Matrix.CreateOrthographicOffCenter(
					cameraPosition - Platform.Instance.Device.Viewport.Width / 2f * 0.05f,
					cameraPosition + Platform.Instance.Device.Viewport.Width / 2f * 0.05f,
					Platform.Instance.Device.Viewport.Height * 0.05f,
					0f,
					0f,
					1f
					);

				_debugView.DrawString( 1, 1, "(R)eset, (F)reeze, (D)ebug to toggle debugview, Arrows to move." );
				_debugView.DrawString( 1, 20, "(S) to spawn/unspawn, O,P to rotate." );
				_debugView.DrawString( 1, 39, "Mouse:" + Mouse.GetState().X + ", " + Mouse.GetState().Y );
				_debugView.RenderDebugData( ref _projection, ref _view );
			}


		}

		public void LoadPhysicsContent()
		{
		}

		public override void OnEnter()
		{
			//Camera
			GameWorldManager.Instance.SpawnEntity( "camera", "camera1", new Vector3( 0, -1300, 600 ) );
			GameWorldManager.Instance.SpawnEntity( "robot", "robotPlayer", new Vector3( -800f, -1000f, 0f ), 2f );
			GameWorldManager.Instance.SpawnEntity( "button", "button1", new Vector3( -400f, -1000f, 0f ), 10f );
			GameWorldManager.Instance.SpawnEntity( "crate", "crate1", new Vector3( -400f, 1000f, 0f ), 3f );

			Robot = GameWorldManager.Instance.GetEntity( "robotPlayer" );
			Robot.PhysicsEntity.Enabled = true;

			Robot.RegisterComponent( "force", new SentientForceComponent( CursorManager.Instance.SceneNode ) );
			Robot.RegisterComponent(
				"control",
				new CarControlComponent( new List<string> { "GearJoint1", "GearJoint2", "GearJoint3" }, new List<float>() { 20f, 20f, 20f } )
				);

			CameraController camControl = new CameraController( GameWorldManager.Instance.GetEntity( "button1" ), SceneManager.Instance.GetCurrentCamera() );
			ControllerRepository.Instance.RegisterController( "camcontrol", camControl );
			FrameUpdateManager.Instance.Register( camControl );

			Camera = GameWorldManager.Instance.GetEntity( "camera1" );

			_cameraMoving = false;
			_actionFreeze = false;
			_actionSpawn = false;
			_actionReset = false;
			_debugViewState = false;
			_actionToggleDebugView = false;


			LoadPhysicsContent();
			_debugView = new DebugViewXNA( Platform.Instance.PhysicsWorld );

			_debugView.LoadContent( Platform.Instance.Device, Platform.Instance.Content );
			//_debugView.RemoveFlags( DebugViewFlags.Joint );

			_debugView.TextColor = Color.Black;

			_view = Matrix.Identity;
		}

		public override void OnClose()
		{
			GameWorldManager.Instance.Clear();
			ControllerRepository.Instance.Clear();
			PhysicsContactManager.Instance.Clear();
			FrameUpdateManager.Instance.Clear();
			Platform.Instance.PhysicsWorld.Clear();
			SceneManager.Instance.Clear();

			_contentManager.Unload();
		}
	}
}
