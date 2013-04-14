using FarseerPhysics.Dynamics;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.GamerServices;
using VertexArmy.Content.Materials;
using VertexArmy.Content.Prefabs;
using VertexArmy.Global;
using VertexArmy.Global.Managers;
using VertexArmy.Input;
using VertexArmy.States;
using VertexArmy.Utilities;

namespace VertexArmy
{
	public class MainGame : Game
	{
		public MainGame()
		{
			Platform.Instance.DeviceManager = new GraphicsDeviceManager( this )
			{
				PreferredBackBufferWidth = 1024,
				PreferredBackBufferHeight = 768,
				IsFullScreen = false
			};
			Platform.Instance.PhysicsWorld = new World( new Vector2( 0f, 9.82f ) );

			Platform.Instance.Settings = new Settings();
			Platform.Instance.Settings.Load();
			Platform.Instance.SoundManager = new SoundManager( Platform.Instance.Settings );
			Platform.Instance.Game = this;

			Content.RootDirectory = "Content";
			UnitsConverter.SetDisplayUnitToSimUnitRatio( 64 );

			Components.Add( new GamerServicesComponent( this ) );
		}

		protected override void Initialize()
		{
			base.Initialize();

			Platform.Instance.Input = new PCInputSystem();
			PhysicsContactManager.Instance.Initialize();
#if TEST_LEVEL_LOADING
			// This is for testing the level loading part. Do not modify this!
			StateManager.Instance.ChangeState( GameState.LevelLoading );
#elif MODEL_VIEW
			StateManager.Instance.ChangeState( GameState.ModelView );
#elif EDITOR
			StateManager.Instance.ChangeState( GameState.Editor );
#elif TUTORIAL
			StateManager.Instance.ChangeState( GameState.TutorialLevel );
#else
			StateManager.Instance.ChangeState( GameState.Menu );
#endif
		}

		protected override void LoadContent()
		{
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
		}

		protected override void Update( GameTime gameTime )
		{
			base.Update( gameTime );

			Platform.Instance.Input.Update( gameTime );
			CursorManager.Instance.Update();

			if ( StateManager.Instance.CurrentGameState != null )
			{
				StateManager.Instance.CurrentGameState.OnUpdate( gameTime );
			}

			StateManager.Instance.OnFrameEndCommitStates();
		}

		protected override void Draw( GameTime gameTime )
		{
			base.Draw( gameTime );

			if ( StateManager.Instance.CurrentGameState != null )
			{
				StateManager.Instance.CurrentGameState.OnRender( gameTime );
			}

			CursorManager.Instance.Render();

		}
	}
}