using System;
using FarseerPhysics.Dynamics;
using Microsoft.Xna.Framework;
using VertexArmy.Content.Materials;
using VertexArmy.Content.Prefabs;
using VertexArmy.Global;
using VertexArmy.Global.Managers;
using VertexArmy.Global.Updaters;
using VertexArmy.Graphics;
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
			//Gearset.GS.Initialize( this );
			base.Initialize( );

			Platform.Instance.Input = new PCInputSystem( );
#if TEST_LEVEL_LOADING
			// This is for testing the level loading part. Do not modify this!
			StateManager.Instance.ChangeState( GameState.LevelLoading );
#else
			StateManager.Instance.ChangeState( GameState.PhysicsPresentationRobot );
#endif
		}

		protected override void LoadContent()
		{
			PrefabRepository.Instance.RegisterPrefab( "robot", RobotPrefab.CreatePrefab( ) );
			MaterialRepository.Instance.RegisterMaterial( "RobotMaterial", RobotMaterial.CreateMaterial( ) );
		}

		protected override void UnloadContent()
		{

		}

		protected override void Update( GameTime gameTime )
		{
			base.Update( gameTime );
			Platform.Instance.Input.Update( gameTime );

			Platform.Instance.PhysicsWorld.Step( Math.Min( ( float ) gameTime.ElapsedGameTime.TotalMilliseconds * 0.001f, ( 1f / 30f ) ) );

			if ( StateManager.Instance.CurrentGameState != null )
			{
				StateManager.Instance.CurrentGameState.OnUpdate( gameTime );
			}

			CursorManager.Instance.Update( );

			TransformableControllerUpdater.Instance.Update( gameTime );
			StateManager.Instance.OnFrameEndCommitStates( );
		}

		protected override void Draw( GameTime gameTime )
		{
			base.Draw( gameTime );
			if ( StateManager.Instance.CurrentGameState != null )
			{
				StateManager.Instance.CurrentGameState.OnRender( gameTime );
			}
			SceneManager.Instance.Render( gameTime.ElapsedGameTime.Milliseconds );

			CursorManager.Instance.Render( );
		}
	}
}
