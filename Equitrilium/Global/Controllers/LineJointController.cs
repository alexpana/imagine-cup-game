using System;
using System.Collections.Generic;
using FarseerPhysics.Dynamics;
using Microsoft.Xna.Framework;
using VertexArmy.Global.Behaviours;
using VertexArmy.Utilities;

namespace VertexArmy.Global.Controllers
{
	public class LineJointController : IController
	{
		private Vector2 _lastBodyPosition;
		private float _lastBodyRotation;

		private const float RotationError = 0.00001f;
		private const float PositionError = 0.00001f;

		public LineJointController( ITransformable transformable, Body body1, Body body2 )
		{
			Data = new List<object> { transformable, body1, body2 };
		}

		public Body Body1
		{
			get { return Data[1] as Body; }
		}

		public Body Body2
		{
			get { return Data[2] as Body; }
		}

		public ITransformable Transformable
		{
			get { return Data[0] as ITransformable; }
		}

		public void Update( GameTime dt )
		{
			ITransformable trans = Data[0] as ITransformable;
			Body body1 = Data[1] as Body;
			Body body2 = Data[2] as Body;

			bool ok = ( trans != null && body1 != null && body2 != null );

			if ( !ok ) return;

			Vector2 line = body2.Position - body1.Position;
			Vector2 newPosition = body1.Position + line / 2;

			line.Normalize();

			float newRotation = ( float ) Math.Acos( line.X ) * Math.Sign( Math.Asin( line.Y ) );

			float rotationDelta = Math.Abs( newRotation - _lastBodyRotation );
			float positionDelta = ( newPosition - _lastBodyPosition ).LengthSquared();

			if ( rotationDelta > RotationError || positionDelta > PositionError )
			{
				trans.SetPosition( new Vector3( UnitsConverter.ToDisplayUnits( newPosition ), 0f ) );
				trans.SetRotation( UnitsConverter.To3DRotation( newRotation ) );

				_lastBodyPosition = newPosition;
				_lastBodyRotation = newRotation;
			}
		}

		public List<object> Data { get; set; }

		public void Clean()
		{
		}
	}
}
