using System;
using System.Collections.Generic;
using FarseerPhysics.Common;
using FarseerPhysics.Dynamics;
using FarseerPhysics.Dynamics.Contacts;
using FarseerPhysics.Factories;
using Microsoft.Xna.Framework;
using VertexArmy.Global.Behaviours;
using VertexArmy.Global.Managers;
using VertexArmy.Utilities;

namespace VertexArmy.Global.Controllers.Components
{
	public class SentientForceComponent : BaseComponent
	{
		public const float Distance = 500f;
		public const float Angle = 0.5f;
		public const float Force = 1f;

		private Vector2 _oldPosition;

		public Body Cone;

		public SentientForceComponent()
		{
			_type = ComponentType.SentientForce;

			float distance = UnitsConverter.ToSimUnits( Distance );
			Vertices coneShape = new Vertices();

			coneShape.Add( Vector2.Zero );

			coneShape.Add( new Vector2( ( float ) Math.Cos( -Angle / 2 ), ( float ) Math.Sin( -Angle / 2 ) ) * distance );
			coneShape.Add( new Vector2( ( float ) Math.Cos( -Angle / 4 ), ( float ) Math.Sin( -Angle / 4 ) ) * distance );
			coneShape.Add( new Vector2( 1f, 0f ) * distance );
			coneShape.Add( new Vector2( ( float ) Math.Cos( Angle / 4 ), ( float ) Math.Sin( Angle / 4 ) ) * distance );
			coneShape.Add( new Vector2( ( float ) Math.Cos( Angle / 2 ), ( float ) Math.Sin( Angle / 2 ) ) * distance );

			Cone = new Body( Platform.Instance.PhysicsWorld );
			Cone.BodyType = BodyType.Dynamic;

			Cone.Position = Vector2.Zero;
			Cone.IsSensor = true;
			FixtureFactory.AttachPolygon( coneShape, 0f, Cone );

			_oldPosition = Vector2.Zero;
		}

		public override void InitEntity()
		{
			Entity.PhysicsEntity.IgnoreCollisionWith( Cone );
			PhysicsContactManager.Instance.RegisterBeginCallback( ContactCallbackType.FixtureBBegin, Cone, BeginContact );
		}


		public override void Update( GameTime dt )
		{
			List<IParameter> parameters = Data;
			DirectCompute( ref parameters );
		}

		public override void DirectCompute( ref List<IParameter> data )
		{
			if ( Entity != null )
			{
				_oldPosition = Cone.Position;
				Vector3 newPosition3D = UnitsConverter.ToSimUnits( Entity.GetPosition() );
				Cone.Position = new Vector2( newPosition3D.X, newPosition3D.Y );
				Cone.Rotation = Entity.GetRotationRadians();
			}
		}

		public bool BeginContact( Contact c )
		{
			Vector2 forceDirection = _oldPosition - c.FixtureA.Body.Position;
			float ratio = forceDirection.Length() * 100 / Distance;
			forceDirection.Normalize();
			c.FixtureA.Body.ApplyForce( forceDirection * ratio * Force );

			return false;
		}
	}
}