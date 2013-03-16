using System;
using FarseerPhysics.Dynamics;
using Microsoft.Xna.Framework;
using VertexArmy.Common;
using VertexArmy.Utilities;

namespace VertexArmy.Graphics
{
	public class TransformableController : IUpdateableObject
	{
		private ITransformable _transformable;
		public ITransformable Transformable
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

		public TransformableController( ITransformable transformable, Body body )
		{
			_body = body;
			Transformable = transformable;


			UpdateTransformableRotation( );
			UpdateTransformablePosition( );
		}

		public Body Body
		{
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
			if ( Transformable != null && _body != null )
			{
				Transformable.SetRotation( Quaternion.CreateFromAxisAngle( new Vector3( 0f, 0f, 1f ), _body.Rotation ) );
			}
		}

		private void UpdateTransformablePosition()
		{
			_lastBodyPosition = _body.Position;
			if ( Transformable != null && _body != null )
			{
				Transformable.SetPosition( new Vector3( UnitsConverter.ToDisplayUnits( _body.Position ), 0f ) );
			}
		}

		private void RemoveSceneNode()
		{

		}
	}
}
