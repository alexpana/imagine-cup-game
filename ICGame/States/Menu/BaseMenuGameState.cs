
using System;
using FarseerPhysics;
using FarseerPhysics.Common;
using FarseerPhysics.Dynamics;
using FarseerPhysics.Factories;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Audio;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Media;
using VertexArmy.Global;
using VertexArmy.Global.Managers;
using VertexArmy.Physics.DebugView;
using VertexArmy.Utilities;

namespace VertexArmy.States.Menu
{
	public abstract class BaseMenuGameState : IGameState
	{
		protected Song BackgroundMusic;
		protected SoundEffect MenuItemSelectionSound;
		protected SoundEffect MenuEventSound;

		private DebugViewXNA _debugView;
		private Matrix _projection;
		private Matrix _view;

		private Body MenuGround;

		protected ContentManager ContentManager;

		protected BaseMenuGameState( ContentManager contentManager )
		{
			ContentManager = contentManager;
		}

		private void CreateDebugView( ContentManager content )
		{
			_debugView = new DebugViewXNA( Platform.Instance.PhysicsWorld );

			_debugView.LoadContent( Platform.Instance.Device, content );
			_debugView.RemoveFlags( DebugViewFlags.Joint );

			_debugView.TextColor = Color.Black;
			_view = Matrix.Identity;
			_projection = Matrix.CreateOrthographicOffCenter(
				UnitsConverter.ToSimUnits( -Platform.Instance.Device.Viewport.Width / 2f ),
				UnitsConverter.ToSimUnits( Platform.Instance.Device.Viewport.Width / 2f ),
				UnitsConverter.ToSimUnits( Platform.Instance.Device.Viewport.Height / 2f ),
				UnitsConverter.ToSimUnits( -Platform.Instance.Device.Viewport.Height / 2f ),
				0f,
				1f
			);
		}

		public virtual void OnRender( GameTime gameTime )
		{
			_debugView.RenderDebugData( ref _projection, ref _view );
			SceneManager.Instance.Render( gameTime.ElapsedGameTime.Milliseconds );
		}

		public virtual void OnEnter()
		{
			BackgroundMusic = ContentManager.Load<Song>( "music/proto1_menu" );
			MenuItemSelectionSound = ContentManager.Load<SoundEffect>( "sounds/button-27" );
			MenuEventSound = ContentManager.Load<SoundEffect>( "sounds/button-30" );

			CreateDebugView( ContentManager );
			CreateCubesGround();
		}

		public virtual void OnClose()
		{
			GameWorldManager.Instance.Clear();
			ControllerRepository.Instance.Clear();
			PhysicsContactManager.Instance.Clear();
			FrameUpdateManager.Instance.Clear();
			Platform.Instance.PhysicsWorld.Clear();
			SceneManager.Instance.Clear();

			ContentManager.Unload();
		}

		public virtual void OnUpdate( GameTime gameTime )
		{
			Platform.Instance.PhysicsWorld.Step( Math.Min( ( float ) gameTime.ElapsedGameTime.TotalMilliseconds * 0.001f, ( 1f / 30f ) ) );
			FrameUpdateManager.Instance.Update( gameTime );
		}

		protected void CreateCubesGround()
		{
			MenuGround = new Body( Platform.Instance.PhysicsWorld )
			{
				Friction = 1.2f,
				Restitution = 0f
			};

			Vertices vertices = new Vertices
			{	
				new Vector2( -10f, 0.5f ),
				new Vector2( 10f, 0.5f )
			};

			for ( int i = 0; i < vertices.Count - 1; ++i )
			{
				FixtureFactory.AttachEdge( vertices[i], vertices[i + 1], MenuGround );
			}
		}
	}
}
