using System;
using FarseerPhysics.Dynamics;
using Microsoft.Xna.Framework;
using VertexArmy.Common;
using VertexArmy.Utilities;

namespace VertexArmy.Global.Controllers
{
	public class BodyController : IController
	{
		private ITransformable _transformable;
		public ITransformable OutputTransformable
		{
			set
			{
				_transformable = value;
				UpdateTransformableRotation( );
				UpdateTransformablePosition( );
			}
			get { return _transformable; }
		}
		private Body _body;

		private Vector2 _lastBodyPosition;
		private float _lastBodyRotation;

		private const float RotationError = 0.001f;
		private const float PositionError = 0.001f;

		public BodyController( ITransformable transformable, Body body )
		{
			_body = body;
			_transformable = transformable;


			UpdateTransformableRotation( );
			UpdateTransformablePosition( );
		}

		public Body Body
		{
			set
			{
				_body = value;
				UpdateTransformableRotation( );
				UpdateTransformablePosition( );
			}
			get { return _body; }
		}

		public void Update( GameTime dt )
		{
			float rotationDelta = Math.Abs( _body.Rotation - _lastBodyRotation );
			float positionDelta = ( _body.Position - _lastBodyPosition ).Length( );

			if ( rotationDelta > RotationError )
			{
				UpdateTransformableRotation( );
			}

			if ( positionDelta > PositionError )
			{
				UpdateTransformablePosition( );
			}
		}

		private void UpdateTransformableRotation()
		{
			_lastBodyRotation = _body.Rotation;
			if ( OutputTransformable != null && _body != null )
			{
				OutputTransformable.SetRotation( Quaternion.CreateFromAxisAngle( new Vector3( 0f, 0f, 1f ), -_body.Rotation ) );
			}
		}

		private void UpdateTransformablePosition()
		{
			_lastBodyPosition = _body.Position;
			if ( OutputTransformable != null && _body != null )
			{
				OutputTransformable.SetPosition( new Vector3( UnitsConverter.ToDisplayUnits( _body.Position ), 0f ) );
			}
		}
	}
}
