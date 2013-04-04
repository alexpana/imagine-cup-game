using System.Collections.Generic;
using FarseerPhysics.Dynamics;
using FarseerPhysics.Dynamics.Contacts;
using FarseerPhysics.Factories;
using Microsoft.Xna.Framework;
using VertexArmy.Global.Behaviours;
using VertexArmy.Global.Managers;
using VertexArmy.Utilities;

namespace VertexArmy.Global.Controllers.Components
{
	public class BodyTriggerAreaComponent : BaseComponent
	{
		public delegate void MethodToCall();
		public Body Area;
		public MethodToCall Callback;
		public Body _body;

		public BodyTriggerAreaComponent( Vector2 size, Body body, MethodToCall callback )
		{
			Data = new List<IParameter>();

			_type = ComponentType.BodyTriggerArea;
			Area = BodyFactory.CreateRectangle( Platform.Instance.PhysicsWorld, UnitsConverter.ToSimUnits( size.X ), UnitsConverter.ToSimUnits( size.Y ), 0.1f );
			Area.BodyType = BodyType.Kinematic;
			Area.IsSensor = true;

			_body = body;
			Callback = callback;
		}

		public override void InitEntity()
		{
			Entity.PhysicsEntity.IgnoreCollisionWith( Area );
			PhysicsContactManager.Instance.RegisterBeginCallback( ContactCallbackType.FixtureBBegin, Area, BeginContactB );
			PhysicsContactManager.Instance.RegisterBeginCallback( ContactCallbackType.FixtureABegin, Area, BeginContactA );
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
				Vector3 position = UnitsConverter.ToSimUnits( Entity.GetPosition() );
				Area.Position = new Vector2( position.X, position.Y );
			}
		}

		public bool BeginContactB( Contact c )
		{
			if ( c.FixtureA.Body.Equals( _body ) )
			{
				Callback();
			}
			return false;
		}

		public bool BeginContactA( Contact c )
		{
			if ( c.FixtureB.Body.Equals( _body ) )
			{
				Callback();
			}
			return false;
		}

		public override void Clean()
		{
			PhysicsContactManager.Instance.UnregisterCallback( ContactCallbackType.FixtureBBegin, Area );
			PhysicsContactManager.Instance.UnregisterCallback( ContactCallbackType.FixtureABegin, Area );
			Platform.Instance.PhysicsWorld.RemoveBody( Area );
		}
	}
}