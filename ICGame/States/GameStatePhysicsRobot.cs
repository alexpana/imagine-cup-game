using System;
using FarseerPhysics;
using FarseerPhysics.Common;
using FarseerPhysics.DebugViews;
using FarseerPhysics.Dynamics;
using FarseerPhysics.Factories;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Input;
using VertexArmy.Entities.Physics;
using VertexArmy.Global;

namespace VertexArmy.States
{
	internal class GameStatePhysicsRobot : IGameState
	{

		private ContentManager _contentManager;

		private DebugViewXNA _debugView;
		private Matrix _projection;
		private Matrix _view;

		//private Body _floorBody;
		private Body _ground;

		private float _cameraPosition;
		private float _cameraError = 0.7f;
		private bool _cameraMoving;
		private float _cameraStep;

		private PhysicsEntityRobot _robot;
		private bool _actionFreeze;
		private bool _actionReset;

		public GameStatePhysicsRobot( ContentManager content )
		{
			_contentManager = content;
		}

		public void OnUpdate( GameTime dt )
		{
			if ( !_cameraMoving && Math.Abs( _cameraPosition - _robot.Position.X ) > _cameraError )
			{
				_cameraMoving = true;
				_cameraStep = ( -1 ) * ( _cameraPosition - _robot.Position.X ) / 15;
			}

			if ( _cameraMoving )
			{
				_cameraPosition += _cameraStep;

				if ( Math.Abs( _cameraPosition - _robot.Position.X ) <= _cameraError / 2 )
				{
					_cameraMoving = false;
				}
				else
				{
					_cameraStep = ( -1 ) * ( _cameraPosition - _robot.Position.X ) / 15;
				}
			}

			Platform.Instance.PhysicsWorld.Step( Math.Min( ( float ) dt.ElapsedGameTime.TotalMilliseconds * 0.001f, ( 1f / 30f ) ) );

			_robot.Move( 0f );
			if ( Keyboard.GetState( PlayerIndex.One ).IsKeyDown( Keys.Left ) )
			{
				_robot.Move( -40f );
			}
			else if ( Keyboard.GetState( PlayerIndex.One ).IsKeyDown( Keys.Right ) )
			{
				_robot.Move( 40f );
			}

			if ( Keyboard.GetState( PlayerIndex.One ).IsKeyDown( Keys.F ) )
			{
				if ( !_actionFreeze )
				{
					_robot.Enabled = !_robot.Enabled;
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
					_robot.Position = new Vector2( 50f, 5f );
					_actionReset = true;
				}
			}

			if ( Keyboard.GetState( PlayerIndex.One ).IsKeyUp( Keys.R ) )
			{
				_actionReset = false;
			}
		}

		public void RenderScene()
		{
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

			_debugView.DrawString( 1, 1, "(R)eset, (F)reeze, Arrows to move." );
			_debugView.RenderDebugData( ref _projection, ref _view );

		}

		public void LoadPhysicsContent()
		{

			_ground = new Body( Platform.Instance.PhysicsWorld );
			{
				Vertices terrain = new Vertices();
				terrain.Add( new Vector2( -20f, 15f ) );
				terrain.Add( new Vector2( -20f, 20f ) );
				terrain.Add( new Vector2( 20f, 20f ) );
				terrain.Add( new Vector2( 25f, 19.75f ) );
				terrain.Add( new Vector2( 30f, 19f ) );
				terrain.Add( new Vector2( 35f, 16f ) );
				terrain.Add( new Vector2( 40f, 20f ) );
				terrain.Add( new Vector2( 45f, 20f ) );
				terrain.Add( new Vector2( 50f, 21f ) );
				terrain.Add( new Vector2( 55f, 22f ) );
				terrain.Add( new Vector2( 60f, 22f ) );
				terrain.Add( new Vector2( 65f, 21.25f ) );
				terrain.Add( new Vector2( 70f, 20f ) );
				terrain.Add( new Vector2( 75f, 19.7f ) );
				terrain.Add( new Vector2( 80f, 18f ) );
				terrain.Add( new Vector2( 85f, 17f ) );
				terrain.Add( new Vector2( 90f, 20f ) );
				terrain.Add( new Vector2( 95f, 20.5f ) );
				terrain.Add( new Vector2( 100f, 21f ) );
				terrain.Add( new Vector2( 105f, 22f ) );
				terrain.Add( new Vector2( 110f, 22.5f ) );
				terrain.Add( new Vector2( 115f, 21.3f ) );
				terrain.Add( new Vector2( 120f, 20f ) );
				terrain.Add( new Vector2( 160f, 20f ) );
				terrain.Add( new Vector2( 159f, 20f ) );
				terrain.Add( new Vector2( 201f, 20f ) );
				terrain.Add( new Vector2( 200f, 20f ) );
				terrain.Add( new Vector2( 240f, 20f ) );
				terrain.Add( new Vector2( 248f, 15f ) );
				terrain.Add( new Vector2( 250f, 15f ) );
				terrain.Add( new Vector2( 250f, 20f ) );
				terrain.Add( new Vector2( 270f, 20f ) );
				terrain.Add( new Vector2( 270f, 20f ) );
				terrain.Add( new Vector2( 310f, 20f ) );
				terrain.Add( new Vector2( 310f, 15f ) );

				for ( int i = 0; i < terrain.Count - 1; ++i )
				{
					FixtureFactory.AttachEdge( terrain[i], terrain[i + 1], _ground );
				}

				_ground.Friction = 1.2f;
				_ground.Restitution = 0f;
			}
			_robot = new PhysicsEntityRobot( 1f, new Vector2( 50f, 5f ) );

			Body rec = BodyFactory.CreateRectangle( Platform.Instance.PhysicsWorld, 2f, 2f, 0.3f );
			rec.Position = new Vector2( 100f, 10f );
			rec.BodyType = BodyType.Dynamic;

			rec = BodyFactory.CreateRectangle( Platform.Instance.PhysicsWorld, 2f, 10f, 1f );
			rec.Position = new Vector2( 249f, 10f );
			rec.BodyType = BodyType.Dynamic;

			_cameraPosition = _robot.Position.X;

		}

		public void OnEnter()
		{
			_cameraMoving = false;
			_actionFreeze = false;
			_actionReset = false;
			LoadPhysicsContent();
			_debugView = new DebugViewXNA( Platform.Instance.PhysicsWorld );

			_debugView.LoadContent( Platform.Instance.Device, Platform.Instance.Content );
			_debugView.RemoveFlags( DebugViewFlags.Joint );

			_debugView.TextColor = Color.Black;

			_view = Matrix.Identity;
		}

		public void OnClose()
		{
			_contentManager.Unload();
		}
	}
}
