using System;
using FarseerPhysics.Dynamics;
using Microsoft.Xna.Framework;
using UnifiedInputSystem;
using UnifiedInputSystem.Keyboard;
using UnifiedInputSystem.Mouse;
using VertexArmy.Content.Materials;
using VertexArmy.Content.Prefabs;
using VertexArmy.Global;
using VertexArmy.Global.Managers;
using VertexArmy.States;
using VertexArmy.Utilities;

#if WINDOWS
using TomShane.Neoforce.Controls;
#endif

namespace VertexArmy
{
	public class MainGame : Game
	{
#if WINDOWS
		private readonly Manager _guiManager;
#endif
#if USE_KINECT
		private Kinect.KinectChooser _kinectChooser;
		private UnifiedInputSystem.Kinect.KinectInputProcessor _kinectInputProcessor;
		private UnifiedInputSystem.Kinect.KinectInputStream _kinectInputStream;
#endif

		public MainGame()
		{
			Platform.Instance.Game = this;

			Platform.Instance.DeviceManager = new GraphicsDeviceManager( this )
			{
				PreferredBackBufferWidth = 1024,
				PreferredBackBufferHeight = 768,
				IsFullScreen = false
			};

			Content.RootDirectory = "Content";

#if WINDOWS
			_guiManager = new Manager( this, Platform.Instance.DeviceManager );
			Platform.Instance.GuiManager = _guiManager;
#endif
#if USE_KINECT
			SetupKinect();
#endif
		}

		protected override void Initialize()
		{
			base.Initialize();
#if WINDOWS
			_guiManager.Initialize();
#endif
			Platform.Instance.PhysicsWorld = new World( new Vector2( 0f, 9.82f ) );

			Platform.Instance.Settings = new Settings();
			Platform.Instance.Settings.Load();
			Platform.Instance.SoundManager = new SoundManager( Platform.Instance.Settings );
			UnitsConverter.SetDisplayUnitToSimUnitRatio( 64 );

			InitializeInput();

#if USE_KINECT
			_kinectChooser.Initialize();
			_kinectChooser.DiscoverSensor();
#endif

			PhysicsContactManager.Instance.Initialize();
#if MODEL_VIEW
			StateManager.Instance.ChangeState( GameState.ModelView );
#elif EDITOR
			StateManager.Instance.ChangeState( GameState.Editor );
#elif TUTORIAL
			StateManager.Instance.ChangeState( GameState.TutorialLevel );
#else
			StateManager.Instance.ChangeState( GameState.Menu );
#endif
		}

		private void InitializeInput()
		{
			InputAggregator inputAggregator = new InputAggregator();

			inputAggregator.Add( new MouseInputProcessor( new MouseInputStream() ) );
			inputAggregator.Add( new KeyboardProcessor( new KeyboardInputStream() ) );

			Platform.Instance.Input = inputAggregator;
		}

		protected override void LoadContent()
		{
#if USE_KINECT
			_kinectChooser.LoadContent();
#endif

			PrefabRepository.Instance.RegisterPrefab( "Robot", RobotPrefab.CreatePrefab() );
			PrefabRepository.Instance.RegisterPrefab( "DamagedRobot1", DamagedRobot1Prefab.CreatePrefab() );
			PrefabRepository.Instance.RegisterPrefab( "Crate", CratePrefab.CreatePrefab() );
			PrefabRepository.Instance.RegisterPrefab( "Mesh", SimpleMeshPrefab.CreatePrefab() );
			PrefabRepository.Instance.RegisterPrefab( "Camera", CameraPrefab.CreatePrefab() );
			PrefabRepository.Instance.RegisterPrefab( MenuCubePrefab.PrefabName, MenuCubePrefab.CreatePrefab() );
			PrefabRepository.Instance.RegisterPrefab( "Button", ButtonPrefab.CreatePrefab() );
			PrefabRepository.Instance.RegisterPrefab( "Floor", FloorPrefab.CreatePrefab() );
			PrefabRepository.Instance.RegisterPrefab( "Wall", WallPrefab.CreatePrefab() );
			PrefabRepository.Instance.RegisterPrefab( "LiftedDoor", LiftedDoorPrefab.CreatePrefab() );
			PrefabRepository.Instance.RegisterPrefab( "Trigger", TriggerPrefab.CreatePrefab() );
			PrefabRepository.Instance.RegisterPrefab( "Saf", SafPrefab.CreatePrefab() );
			PrefabRepository.Instance.RegisterPrefab( "FloorBridge", FloorBridgePrefab.CreatePrefab() );
			PrefabRepository.Instance.RegisterPrefab( "UpgradePlatform", UpgradePlatformPrefab.CreatePrefab() );
			PrefabRepository.Instance.RegisterPrefab( "Pipe", PipePrefab.CreatePrefab() );
			PrefabRepository.Instance.RegisterPrefab( "SafCollectible", SafCollectiblePrefab.CreatePrefab() );

			PrefabRepository.Instance.RegisterPrefab( "WallBackground", SimpleMeshPrefab.CreatePrefab( "models/quad", "WallMaterial", "wall-node" ) );
			PrefabRepository.Instance.RegisterPrefab( "WallMenu", SimpleMeshPrefab.CreatePrefab( "models/quad", "WallMenuMaterial", "wall-node-2" ) );
			PrefabRepository.Instance.RegisterPrefab( "WallMenu2", SimpleMeshPrefab.CreatePrefab( "models/quad", "WallMenuMaterial2", "wall-node-3" ) );

			MaterialRepository.Instance.RegisterMaterial( "MenuCubeMaterial", args => MenuCubeMaterial.CreateMaterial( args ) );
			MaterialRepository.Instance.RegisterMaterial( "SafMaterial", args => SafMaterial.CreateMaterial() );
			MaterialRepository.Instance.RegisterMaterial( "HighlightMaterial", args => HighlightMaterial.CreateMaterial() );

			// Unlit materials for style2.0
			MaterialRepository.Instance.RegisterMaterial( "ButtonMaterial", args => TexturedMaterial.CreateMaterial( "color_button" ) );
			MaterialRepository.Instance.RegisterMaterial( "RobotMaterial", args => TexturedMaterial.CreateMaterial( "color_robo" ) );
			MaterialRepository.Instance.RegisterMaterial( "UPlatformMaterial", args => TexturedMaterial.CreateMaterial( "color_uplatform" ) );
			MaterialRepository.Instance.RegisterMaterial( "CrateMaterial", args => TexturedMaterial.CreateMaterial( "color_crate" ) );
			MaterialRepository.Instance.RegisterMaterial( "CelShadingMaterial", args => TexturedMaterial.CreateMaterial( "color_crate" ) );
			MaterialRepository.Instance.RegisterMaterial( "InnerDoorMaterial", args => TexturedMaterial.CreateMaterial( "color_door_inner" ) );
			MaterialRepository.Instance.RegisterMaterial( "OuterDoorMaterial", args => TexturedMaterial.CreateMaterial( "color_door_outer" ) );
			MaterialRepository.Instance.RegisterMaterial( "WallMaterial", args => TexturedMaterial.CreateMaterial( "empty_gray" ) );
			MaterialRepository.Instance.RegisterMaterial( "TileMaterial", args => TexturedMaterial.CreateMaterial( "empty_dark_gray" ) );
			MaterialRepository.Instance.RegisterMaterial( "FloorBridgeMaterial", args => TexturedMaterial.CreateMaterial( "color_bridge" ) );
			MaterialRepository.Instance.RegisterMaterial( "PipeMaterial", args => TexturedMaterial.CreateMaterial( "color_pipe" ) );
			MaterialRepository.Instance.RegisterMaterial( "PowerupSphereMaterial", args => TexturedMaterial.CreateMaterial( "empty_dark_gray" ) );
		}

		protected override void UnloadContent()
		{
#if USE_KINECT
			_kinectChooser.UnloadContent();
#endif
		}

		protected override void Update( GameTime gameTime )
		{
			base.Update( gameTime );
#if WINDOWS
			_guiManager.Update( gameTime );
#endif

			Platform.Instance.Input.Update( new Time( gameTime ) );
			CursorManager.Instance.Update();

			if ( StateManager.Instance.CurrentGameState != null )
			{
				StateManager.Instance.CurrentGameState.OnUpdate( gameTime );
			}

			StateManager.Instance.OnFrameEndCommitStates();
		}

		protected override void Draw( GameTime gameTime )
		{

#if WINDOWS
			_guiManager.BeginDraw( gameTime );
#endif
			base.Draw( gameTime );

			if ( StateManager.Instance.CurrentGameState != null )
			{
				StateManager.Instance.CurrentGameState.OnRender( gameTime );
			}

#if WINDOWS
			_guiManager.EndDraw();
#endif
#if USE_KINECT
			_kinectChooser.Draw( gameTime );
#endif

			CursorManager.Instance.Render();
		}

#if USE_KINECT
		private void SetupKinect()
		{
			_kinectChooser = new Kinect.KinectChooser( this,
				Microsoft.Kinect.ColorImageFormat.RgbResolution640x480Fps30,
				Microsoft.Kinect.DepthImageFormat.Resolution640x480Fps30 );
			_kinectChooser.SensorChanged += ( sender, args ) =>
			{
				if ( _kinectChooser.Sensor == null )
				{
					if ( _kinectInputProcessor != null )
					{
						Platform.Instance.Input.Remove( _kinectInputProcessor );

						_kinectInputStream.Dispose();

						_kinectInputProcessor = null;
						_kinectInputStream = null;
					}
				}
				else
				{
					if ( _kinectInputStream != null )
					{
						throw new ArgumentException( "Cannot use two kinect streams at a time!" );
					}

					_kinectInputStream = new UnifiedInputSystem.Kinect.KinectInputStream( _kinectChooser.Sensor,
						Platform.Instance.DeviceManager.PreferredBackBufferWidth,
						Platform.Instance.DeviceManager.PreferredBackBufferHeight );
					_kinectInputProcessor = new UnifiedInputSystem.Kinect.KinectInputProcessor( _kinectInputStream );
					Platform.Instance.Input.AddToFront( _kinectInputProcessor );
				}
			};
		}
#endif
	}
}
