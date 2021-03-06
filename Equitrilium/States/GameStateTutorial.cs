﻿using System.Collections.Generic;
using FarseerPhysics.Dynamics;
using FarseerPhysics.Dynamics.Joints;
using FarseerPhysics.Factories;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework.Media;
using UnifiedInputSystem.Extensions;
using UnifiedInputSystem.Input;
using VertexArmy.Content.Prefabs;
using VertexArmy.GameWorld;
using VertexArmy.Global;
using VertexArmy.Global.Controllers;
using VertexArmy.Global.Controllers.Components;
using VertexArmy.Global.Hints;
using VertexArmy.Global.Managers;
using VertexArmy.Global.ShapeListeners;
using VertexArmy.Physics.DebugView;
using VertexArmy.Utilities;

namespace VertexArmy.States
{
	internal class GameStateTutorial : PlayableGameState
	{
		private readonly ContentManager _contentManager;

		private DebugViewXNA _debugView;
		private Matrix _projection;
		private Matrix _view;

		public GameEntity Robot;
		public GameEntity Camera;

		private WorldShapeCollisionController _wsController;

		private LevelPrefab _level;

		private bool _debugViewState;

		private bool _endOfGameHintShown;
		private readonly Dictionary<string, bool> _saveTriggerStates;

		public GameStateTutorial( ContentManager content )
		{
			_contentManager = content;
			_saveTriggerStates = new Dictionary<string, bool>();
		}

		public override void OnUpdate( GameTime gameTime )
		{
			base.OnUpdate( gameTime );
			if ( Robot != null )
			{
				if ( Robot.GetPosition().Y < -2000 )
				{
					ResetGameState();
				}
#if DEBUG
				if ( Platform.Instance.Input.HasEvent( UISButton.Up ) )
				{
					if ( Robot.PhysicsEntity.GetCollisionLayer().Equals( Category.Cat1 ) )
					{
						Robot.PhysicsEntity.SetCollisionLayer( Category.Cat2 );
						Vector3 position = Robot.GetPosition();
						Robot.SetPosition( new Vector3( position.X, position.Y, -800 ) );
					}
				}
				if ( Platform.Instance.Input.HasEvent( UISButton.Down ) )
				{
					if ( Robot.PhysicsEntity.GetCollisionLayer().Equals( Category.Cat2 ) )
					{
						Robot.PhysicsEntity.SetCollisionLayer( Category.Cat1 );
						Vector3 position = Robot.GetPosition();
						Robot.SetPosition( new Vector3( position.X, position.Y, 0f ) );
					}
				}
#endif

				if ( Platform.Instance.Input.HasEvent( UISButton.R, true ) )
				{
					ResetGameState();
				}
			}
#if DEBUG
			if ( Platform.Instance.Input.HasEvent( UISButton.D, true ) )
			{
				_debugViewState = !_debugViewState;
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

				_debugView.DrawString( 1, 1, "(R)eset, (F)reeze, (D)ebug to toggle debugview, Arrows to move." );
				_debugView.DrawString( 1, 20, "(S) to spawn/unspawn, O,P to rotate." );
				_debugView.DrawString( 1, 39, "Mouse:" + Mouse.GetState().X + ", " + Mouse.GetState().Y );
				_debugView.RenderDebugData( ref _projection, ref _view );
			}
		}

		public void LoadStatics()
		{
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
		}

		public void LoadSemiStatics()
		{
			GameWorldManager.Instance.GetEntity( "button1" ).RegisterComponent(
				"active",
				new ButtonComponent( "ButtonJoint1" )
				);

			GameWorldManager.Instance.GetEntity( "door" ).RegisterComponent(
				"doorHandle",
				new LiftedDoorComponent( GameWorldManager.Instance.GetEntity( "button1" ).GetComponent( "active" ), "DoorJoint1" )
				);
		}

		public void LoadDynamics()
		{
			GameWorldManager.Instance.SpawnEntity( "Camera", "camera1", new Vector3( 0, -200, 800 ) );

			Robot = GameWorldManager.Instance.GetEntity( "robotPlayer" );

			Robot.RegisterComponent(
				"control",
				new CarControlComponent(
					new List<string> { "GearJoint1", "GearJoint2", "GearJoint3" }, new List<float> { 7f, 7f, 7f } )
				);


			CameraController camControl = new CameraController( Robot, SceneManager.Instance.GetCurrentCamera() );
			ControllerRepository.Instance.RegisterController( "camcontrol", camControl );

			FrameUpdateManager.Instance.Register( camControl );

			Camera = GameWorldManager.Instance.GetEntity( "camera1" );
			Camera.SetRotation( 5f );
			FrameUpdateManager.Instance.Register( new GravityController() );

			_wsController = new WorldShapeCollisionController();
			ControllerRepository.Instance.RegisterController( "RobotTriggerController", camControl );
			FrameUpdateManager.Instance.Register( _wsController );
			_wsController.SetSubject( Robot );
		}

		public void LoadTriggers()
		{
			GameWorldManager.Instance.SpawnEntity( "Trigger", "upgradeCube1", new Vector3( 500f + 60f * 2, 70f, 0f ) );
			GameWorldManager.Instance.GetEntity( "upgradeCube1" ).RegisterComponent(
				"trigger",
				new BodyTriggerAreaComponent( new Vector2( 10f, 10f ), Robot.MainBody, UpgradeCube1Callback )
				);

			SpawnSaveTrigger( "save1", new Vector3( 1682f, 138f, 0f ) );
			SpawnSaveTrigger( "save2", new Vector3( 3600, 0, 0f ) );

			GameWorldManager.Instance.SpawnEntity( "SafCollectible", "safCollectible1", new Vector3( 500f + 60f * 2, 70f, 0f ) );
			ControllerRepository.Instance.RegisterController( "upgradeCube1Controller", new CollectibleController( GameWorldManager.Instance.GetEntity( "safCollectible1" ).MainNode ) );
			FrameUpdateManager.Instance.Register( ControllerRepository.Instance.GetController( "upgradeCube1Controller" ) );


			GameWorldManager.Instance.SpawnEntity( "Trigger", "endGame", new Vector3( 3500, 30f, 0f ) );
			GameWorldManager.Instance.GetEntity( "endGame" ).RegisterComponent(
				"trigger",
				new BodyTriggerAreaComponent( new Vector2( 10f, 10f ), Robot.MainBody, EndOfGameHint ) );

			RegisterHints();
		}

		private void SpawnSaveTrigger( string triggerName, Vector3 position )
		{
			_saveTriggerStates[triggerName] = false;

			GameWorldManager.Instance.SpawnEntity( "Trigger", triggerName, position );
			GameWorldManager.Instance.GetEntity( triggerName ).RegisterComponent(
				"trigger",
				new BodyTriggerAreaComponent( new Vector2( 10f, 10f ), Robot.MainBody,
					() =>
					{
						if ( _saveTriggerStates[triggerName] )
						{
							return;
						}

						GameWorldManager.Instance.SaveState();
						_saveTriggerStates[triggerName] = true;
					} )
				);
		}

		private void RegisterHints()
		{
			_wsController.Register( new HintShapeListener( new FadeHint( "It seems you are blocked.\nPress the 'R' key to reset your position.", new Vector2( 100, 50 ), 500f, 500f ) ),
				new BoundingSphere( new Vector3( 1492, 30f, 0f ), 90 ) );

			_wsController.Register( new HintShapeListener( new FadeHint( "It seems you are blocked.\nPress the 'R' key to reset your position.", new Vector2( 100, 50 ), 500f, 500f ) ),
				new BoundingBox( new Vector3( 2200, -280, -100 ), new Vector3( 3000, -5, 100 ) ) );

			_wsController.Register(
				new HintShapeListener( new FadeHint( "Crates can be pushed around.\nTry pushing that crate towards the wall button.", new Vector2( 100, 50 ), 2000f, 500f, 500f ), true ),
				new BoundingSphere( new Vector3( -150, 60, 0f ), 200 ) );

			_wsController.Register(
				new HintShapeListener( new FadeHint( "Step onto the platform ahead for an upgrade.", new Vector2( 100, 100 ), 500f, 500f ), true ),
				new BoundingSphere( new Vector3( 320, 60f, 0f ), 150 ) );

			_wsController.Register(
				new HintShapeListener( new FadeHint( "Sometimes you can get stuck.\nPress R to reverse to the last checkpoint.", new Vector2( 100, 100 ), 2000f, 500f, 500f ), true ),
				new BoundingSphere( new Vector3( 1200, 150f, 0f ), 200 ) );

			_wsController.Register(
				new HintShapeListener( new FadeHint( "Some puzzles can be harder to overcome.\nRemember to press R in case you get stuck.", new Vector2( 100, 100 ), 2000f, 500f, 500f ), true ),
				new BoundingSphere( new Vector3( 1900, 150f, 0f ), 200 ) );

			_wsController.Register(
				new HintShapeListener( new FadeHint( "Press Mouse1 to pull objects towards the robot. \nPress Mouse2 to push away objects from the robot.", new Vector2( 100, 100 ), 500f, 500f ), true ),
				new BoundingSphere( new Vector3( 620f, 70f, 0f ), 100 ) );
		}

		public void LoadLevel()
		{
			string _levelName = "level1";
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
			Platform.Instance.PhysicsWorld.Gravity = Vector2.UnitY * Platform.Instance.PhysicsWorld.Gravity.Length();

			FrameUpdateManager.Instance.Register( HintManager.Instance );

			_debugViewState = false;

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

		private void ResetGameState()
		{
			_endOfGameHintShown = false;
			HintManager.Instance.Clear();
			_wsController.Clean();
			GameWorldManager.Instance.LoadLastState();
		}

		public void UpgradeCube1Callback()
		{
			if ( Robot.GetComponent( "force" ) == null )
			{
				GameWorldManager.Instance.RemoveEntity( "safCollectible1" );
				FrameUpdateManager.Instance.Unregister( ControllerRepository.Instance.GetController( "upgradeCube1Controller" ) );
				ControllerRepository.Instance.UnregisterController( "upgradeCube1Controller" );

				Robot.RegisterComponent( "force", new SentientForceComponent() );
				GameWorldManager.Instance.SaveState();
			}
		}

		private void EndOfGameHint()
		{
			if ( _endOfGameHintShown )
			{
				return;
			}

			_endOfGameHintShown = true;
			HintManager.Instance.SpawnHint( "This concludes the tutorial.\nGood luck in your future endeavours.", new Vector2( 100, 100 ), 4000, 1, EndGameCallback );
		}

		public void EndGameCallback()
		{
			StateManager.Instance.PopState();
			StateManager.Instance.ChangeState( GameState.Menu );
		}
	}
}
