using System;
using System.Collections.Generic;
using FarseerPhysics.Dynamics;
using Microsoft.Xna.Framework;
using VertexArmy.GameWorld;
using VertexArmy.Global.Behaviours;
using VertexArmy.Utilities;

namespace VertexArmy.Global.Controllers
{
	public class BodyController : IController
	{
		private Vector2 _lastBodyPosition;
		private float _lastBodyRotation;

		private const float RotationError = 0.00001f;
		private const float PositionError = 0.00001f;

		public BodyController( ITransformable transformable, Body body, GameEntity entity )
		{
			Data = new List<object> { transformable, body, entity };
		}

		public Body Body
		{
			get { return Data[1] as Body; }
		}

		public ITransformable Transformable
		{
			get { return Data[0] as ITransformable; }
		}

		public void Update( GameTime dt )
		{
			ITransformable trans = Data[0] as ITransformable;
			Body body = Data[1] as Body;
			GameEntity entity = Data[2] as GameEntity;

			bool ok = ( trans != null && body != null && entity != null );

			if ( !ok )
			{
				return;
			}

			float rotationDelta = Math.Abs( body.Rotation - _lastBodyRotation );
			float positionDelta = ( body.Position - _lastBodyPosition ).LengthSquared();

			if ( rotationDelta > RotationError || positionDelta > PositionError || entity.HasExternalRotation() )
			{
				trans.SetPosition( new Vector3( UnitsConverter.ToDisplayUnits( body.Position ), trans.GetPosition().Z ) );
				trans.SetRotation( UnitsConverter.To3DRotation( body.Rotation ) * entity.GetExternalRotation() );

				_lastBodyPosition = body.Position;
				_lastBodyRotation = body.Rotation;
			}
		}


		public List<object> Data { get; set; }

		public void Clean()
		{
		}
	}
}
