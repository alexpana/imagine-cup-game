using System;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Input;
using VertexArmy.Global.Managers;

namespace VertexArmy.States
{
	internal class GameStateModelViewer : PlayableGameState
	{
		private Quaternion _modelRotation;

		private bool _dragging;
		private Vector2 _lastMousePos;

		public GameStateModelViewer( ContentManager contentManager )
		{

		}

		private void Rotate( float delta, Vector3 axis )
		{
			Vector3 localAxis;
			if ( axis.Equals( new Vector3( 0, 1, 0 ) ) )
			{
				localAxis = Vector3.Transform( axis, Matrix.CreateFromQuaternion( _modelRotation ) );
			}
			else
			{
				localAxis = axis;
			}

			_modelRotation = Quaternion.Concatenate( _modelRotation, new Quaternion(
				localAxis * ( float ) Math.Sin( delta / 2.0f ),
				( float ) Math.Cos( delta / 2.0f ) ) );
		}

		public override void OnUpdate( GameTime gameTime )
		{
			base.OnUpdate( gameTime );

			/*
			if ( _frames < _frameFreeze )
			{
				_frames++;
			}
			else if ( _frames == _frameFreeze )
			{
				GameWorldManager.Instance.GetEntity( "mesh1" ).SetPhysicsEnabled( false );
				GameWorldManager.Instance.GetEntity( "mesh1" ).SetPosition( Vector3.Zero );
			}
			*/


			Vector2 mouseDelta = new Vector2( 0, 0 );

			if ( _dragging )
			{
				switch ( Mouse.GetState().LeftButton )
				{
					case ButtonState.Pressed:
						mouseDelta.X = Mouse.GetState().X - _lastMousePos.X;
						mouseDelta.Y = Mouse.GetState().Y - _lastMousePos.Y;
						_lastMousePos.X = Mouse.GetState().X;
						_lastMousePos.Y = Mouse.GetState().Y;
						break;

					case ButtonState.Released:
						_dragging = false;
						break;
				}
			}

			if ( Mouse.GetState().LeftButton == ButtonState.Pressed && !_dragging )
			{
				_dragging = true;
				_lastMousePos.X = Mouse.GetState().X;
				_lastMousePos.Y = Mouse.GetState().Y;
			}

			Rotate( 0.01f * mouseDelta.X, Vector3.UnitY );
			Rotate( 0.01f * mouseDelta.Y, Vector3.UnitX );

			GameWorldManager.Instance.GetEntity( "saf1" ).SetRotation( _modelRotation );
			GameWorldManager.Instance.GetEntity( "camera1" ).SetPosition(
				new Vector3( 0, 0, 70 - Mouse.GetState().ScrollWheelValue / 4.0f ) );
		}

		public override void OnEnter()
		{
			_modelRotation = Quaternion.Identity;

			SceneManager.Instance.Clear();
			CursorManager.Instance.SetActiveCursor( CursorType.Arrow );
			CursorManager.Instance.SetVisible( true );

			GameWorldManager.Instance.SpawnEntity( "Camera", "camera1", new Vector3( 0, 0, 100 ) );
			GameWorldManager.Instance.SpawnEntity( "Saf", "saf1", new Vector3( 0, 0, 0 ) );

			//GameWorldManager.Instance.SpawnEntity( "robot", "mesh1", new Vector3( 0f, 0, 0f ) );
		}

		public override void OnClose()
		{
			SceneManager.Instance.Clear();
		}
	}
}
