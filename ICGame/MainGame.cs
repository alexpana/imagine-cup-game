using FarseerPhysics.Dynamics;
using Microsoft.Xna.Framework;
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
			Platform.Instance.Game = this;
			Content.RootDirectory = "Content";
			UnitsConverter.SetDisplayUnitToSimUnitRatio( 64 );
		}

		protected override void Initialize()
		{
			base.Initialize( );

			Platform.Instance.Input = new PCInputSystem( );
#if TEST_LEVEL_LOADING
			// This is for testing the level loading part. Do not modify this!
			StateManager.Instance.ChangeState( GameState.LevelLoading );
#elif MODEL_VIEW
			StateManager.Instance.ChangeState( GameState.ModelView );
#else
			StateManager.Instance.ChangeState( GameState.PhysicsPresentationRobot );
#endif
		}

		protected override void LoadContent()
		{
			PrefabRepository.Instance.RegisterPrefab( "robot", RobotPrefab.CreatePrefab( ) );
			PrefabRepository.Instance.RegisterPrefab( "crate", CratePrefab.CreatePrefab( ) );
			PrefabRepository.Instance.RegisterPrefab( "mesh", SimpleMeshPrefab.CreatePrefab( ) );
			PrefabRepository.Instance.RegisterPrefab( "camera", CameraPrefab.CreatePrefab( ) );
			MaterialRepository.Instance.RegisterMaterial( "RobotMaterial", RobotMaterial.CreateMaterial( ) );
		}

		protected override void UnloadContent()
		{

		}

		protected override void Update( GameTime gameTime )
		{
			base.Update( gameTime );

			Platform.Instance.Input.Update( gameTime );
			CursorManager.Instance.Update( );

			if ( StateManager.Instance.CurrentGameState != null )
			{
				StateManager.Instance.CurrentGameState.OnUpdate( gameTime );
			}

			StateManager.Instance.OnFrameEndCommitStates( );
		}

		protected override void Draw( GameTime gameTime )
		{
			base.Draw( gameTime );

			if ( StateManager.Instance.CurrentGameState != null )
			{
				StateManager.Instance.CurrentGameState.OnRender( gameTime );
			}

			CursorManager.Instance.Render( );
		}
	}
}
