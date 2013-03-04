using System;
using FarseerPhysics;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using VertexArmy.Global;
using VertexArmy.Levels;
using VertexArmy.Physics.DebugView;

namespace VertexArmy.States
{
	internal class GameStateLevelLoading : IGameState
	{
		private Level _level;

		private DebugViewXNA _debugView;
		private Matrix _view;
		private Matrix _projection;
		private float _cameraPosition;

		private readonly ContentManager _contentManager;

		public GameStateLevelLoading( ContentManager contentManager )
		{
			_contentManager = contentManager;
		}

		private void LoadContent()
		{
			_level = LevelManager.Instance.GetLevel( "tutorial" );
		}

		public void OnUpdate( GameTime dt )
		{
			Platform.Instance.PhysicsWorld.Step( Math.Min( ( float ) dt.ElapsedGameTime.TotalMilliseconds * 0.001f, ( 1f / 30f ) ) );
		}

		public void OnRender( GameTime dt )
		{
			_projection = Matrix.CreateOrthographicOffCenter(
				_cameraPosition - Platform.Instance.Device.Viewport.Width / 2f * 0.05f,
				_cameraPosition + Platform.Instance.Device.Viewport.Width / 2f * 0.05f,
				Platform.Instance.Device.Viewport.Height * 0.05f,
				0f,
				0f,
				1f
				);

			_debugView.DrawString( 1, 30, "Level: " + _level.Name );
			_debugView.RenderDebugData( ref _projection, ref _view );
		}

		public void OnEnter()
		{
			LoadContent();

			_debugView = new DebugViewXNA( Platform.Instance.PhysicsWorld );
			_debugView.LoadContent( Platform.Instance.Device, Platform.Instance.Content );
			_debugView.RemoveFlags( DebugViewFlags.Joint );

			_debugView.TextColor = Color.Black;

			_view = Matrix.Identity;

			//Body rec = BodyFactory.CreateRectangle( Platform.Instance.PhysicsWorld, 2f, 2f, 0.3f );
			//rec.Position = new Vector2( 0f, 5f );
			//rec.BodyType = BodyType.Static;

			_cameraPosition = 0;
		}

		public void OnClose()
		{
			_contentManager.Unload();
		}
	}
}
