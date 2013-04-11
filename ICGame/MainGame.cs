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
				PreferredBackBufferWidth = 800,
				PreferredBackBufferHeight = 600
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
#else
			StateManager.Instance.ChangeState( GameState.Menu );
#endif
		}

		protected override void LoadContent()
		{
			PrefabRepository.Instance.RegisterPrefab( "Robot", RobotPrefab.CreatePrefab() );
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

			PrefabRepository.Instance.RegisterPrefab( "WallBackground", SimpleMeshPrefab.CreatePrefab( "models/quad", "WallMaterial", "wall-node" ) );
			PrefabRepository.Instance.RegisterPrefab( "WallMenu", SimpleMeshPrefab.CreatePrefab( "models/quad", "WallMenuMaterial", "wall-node-2" ) );
			PrefabRepository.Instance.RegisterPrefab( "WallMenu2", SimpleMeshPrefab.CreatePrefab( "models/quad", "WallMenuMaterial2", "wall-node-3" ) );

			MaterialRepository.Instance.RegisterMaterial( "MenuCubeMaterial", args => MenuCubeMaterial.CreateMaterial( args ) );
			MaterialRepository.Instance.RegisterMaterial( "SafMaterial", args => SafMaterial.CreateMaterial() );
			MaterialRepository.Instance.RegisterMaterial( "WallMenuMaterial", args => WallMenuMaterial.CreateMaterial() );
			MaterialRepository.Instance.RegisterMaterial( "WallMenuMaterial2", args => WallMenuMaterial2.CreateMaterial() );
			MaterialRepository.Instance.RegisterMaterial( "TechMaterial", args => TechMaterial.CreateMaterial() );
			MaterialRepository.Instance.RegisterMaterial( "HighlightMaterial", args => HighlightMaterial.CreateMaterial() );

			// Unlit materials for style2.0
			MaterialRepository.Instance.RegisterMaterial( "ButtonMaterial", args => UnlitMaterial.CreateMaterial( "button_COLOR" ) );
			MaterialRepository.Instance.RegisterMaterial( "RobotMaterial", args => UnlitMaterial.CreateMaterial( "robo_COLOR" ) );
			MaterialRepository.Instance.RegisterMaterial( "UPlatformMaterial", args => UnlitMaterial.CreateMaterial( "uplatform_COLOR" ) );
			MaterialRepository.Instance.RegisterMaterial( "CrateMaterial", args => UnlitMaterial.CreateMaterial( "crate_COLOR" ) );
			MaterialRepository.Instance.RegisterMaterial( "CelShadingMaterial", args => UnlitMaterial.CreateMaterial( "crate_COLOR" ) );
			MaterialRepository.Instance.RegisterMaterial( "InnerDoorMaterial", args => UnlitMaterial.CreateMaterial( "door_inner_COLOR" ) );
			MaterialRepository.Instance.RegisterMaterial( "OuterDoorMaterial", args => UnlitMaterial.CreateMaterial( "door_outer_COLOR" ) );
			MaterialRepository.Instance.RegisterMaterial( "WallMaterial", args => UnlitMaterial.CreateMaterial( "flat_gray" ) );
			MaterialRepository.Instance.RegisterMaterial( "TileMaterial", args => UnlitMaterial.CreateMaterial( "flat_dark_gray" ) );
			MaterialRepository.Instance.RegisterMaterial( "FloorBridgeMaterial", args => UnlitMaterial.CreateMaterial( "bridge_COLOR" ) );
			MaterialRepository.Instance.RegisterMaterial( "PipeMaterial", args => UnlitMaterial.CreateMaterial( "pipe_COLOR" ) );
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