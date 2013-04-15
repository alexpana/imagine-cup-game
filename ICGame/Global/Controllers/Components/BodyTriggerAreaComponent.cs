using System.Collections.Generic;
using FarseerPhysics.Dynamics;
using FarseerPhysics.Dynamics.Contacts;
using FarseerPhysics.Factories;
using Microsoft.Xna.Framework;
using VertexArmy.Global.Managers;
using VertexArmy.Utilities;

namespace VertexArmy.Global.Controllers.Components
{
	public class BodyTriggerAreaComponent : BaseComponent
	{
		public delegate void MethodToCall();
		public delegate void MethodToCall2( Body b );
		public Body Area;
		public MethodToCall Callback;
		public MethodToCall2 Callback2;
		public Body _body;

		public BodyTriggerAreaComponent( Vector2 size, Body body, MethodToCall callback )
		{
			Data = new List<object>();

			_type = ComponentType.BodyTriggerArea;
			Area = BodyFactory.CreateRectangle( Platform.Instance.PhysicsWorld, UnitsConverter.ToSimUnits( size.X ), UnitsConverter.ToSimUnits( size.Y ), 0.1f );
			Area.BodyType = BodyType.Kinematic;
			Area.IsSensor = true;

			_body = body;
			Callback = callback;
		}

		public BodyTriggerAreaComponent( Vector2 size, MethodToCall2 callback )
		{
			Data = new List<object>();

			_type = ComponentType.BodyTriggerArea;
			Area = BodyFactory.CreateRectangle( Platform.Instance.PhysicsWorld, UnitsConverter.ToSimUnits( size.X ), UnitsConverter.ToSimUnits( size.Y ), 0.1f );
			Area.BodyType = BodyType.Kinematic;
			Area.IsSensor = true;

			Callback2 = callback;
		}

		public override void InitEntity()
		{
			Entity.PhysicsEntity.IgnoreCollisionWith( Area );
			PhysicsContactManager.Instance.RegisterBeginCallback( ContactCallbackType.FixtureBBegin, Area, BeginContactB );
			PhysicsContactManager.Instance.RegisterBeginCallback( ContactCallbackType.FixtureABegin, Area, BeginContactA );
		}


		public override void Update( GameTime dt )
		{
			if ( Entity != null )
			{
				Vector3 position = UnitsConverter.ToSimUnits( Entity.GetPosition() );
				Area.Position = new Vector2( position.X, position.Y );
			}
		}

		public bool BeginContactB( Contact c )
		{
			if ( _body != null )
			{
				if ( c.FixtureA.Body.Equals( _body ) )
				{
					Callback();
				}
			}
			else
			{
				Callback2( c.FixtureA.Body );
			}

			return false;
		}

		public bool BeginContactA( Contact c )
		{
			if ( _body != null )
			{
				if ( c.FixtureB.Body.Equals( _body ) )
				{
					Callback();
				}
			}
			else
			{
				Callback2( c.FixtureB.Body );
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