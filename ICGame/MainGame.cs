using FarseerPhysics.Dynamics;
using Microsoft.Xna.Framework;
using VertexArmy.Global;
using VertexArmy.Input;
using VertexArmy.States;

namespace VertexArmy
{
	public class MainGame : Game
	{
		public MainGame()
		{
			Platform.Instance.DeviceManager = new GraphicsDeviceManager( this );
			Platform.Instance.DeviceManager.PreferredBackBufferWidth = 800;
			Platform.Instance.DeviceManager.PreferredBackBufferHeight = 600;
			Platform.Instance.PhysicsWorld = new World( new Vector2( 0f, 9.82f ) );
			Platform.Instance.Game = this;
			Content.RootDirectory = "Content";
		}

		protected override void Initialize()
		{
			base.Initialize();

			Platform.Instance.Input = new PCInputSystem();
			StateManager.Instance.ChangeState( GameState.PhysicsPresentationTank );
		}

		protected override void LoadContent()
		{
		}

		protected override void UnloadContent()
		{
		}

		protected override void Update( GameTime gameTime )
		{
			base.Update( gameTime );
			Platform.Instance.Input.Update( gameTime );

			if ( StateManager.Instance.CurrentGameState != null )
			{
				StateManager.Instance.CurrentGameState.OnUpdate( gameTime );
			}

			CursorManager.Instance.Update();
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
