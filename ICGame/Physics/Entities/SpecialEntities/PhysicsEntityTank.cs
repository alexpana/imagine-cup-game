using System.Collections.Generic;
using FarseerPhysics.Collision.Shapes;
using FarseerPhysics.Common;
using FarseerPhysics.Dynamics;
using FarseerPhysics.Dynamics.Joints;
using FarseerPhysics.Factories;
using Microsoft.Xna.Framework;

namespace VertexArmy.Physics.SpecialEntities
{
	public class PhysicsEntityTank : IPhysicsEntity
	{
		private World _physicsWorld;

		private bool _frozen = false;
		private Body _tankBody;
		private Path _path;
		private List<Body> _bodies;

		private Body _gear1;
		private Body _gear2;
		private Body _gear3;

		private LineJoint _joint1;
		private LineJoint _joint2;
		private LineJoint _joint3;

		public void SetPosition( Vector2 position )
		{			
			Vector2 relative = position - _tankBody.Position;

			_tankBody.ResetDynamics( );
			_tankBody.SetTransform( position, _tankBody.Rotation );

			_gear1.ResetDynamics( );
			_gear2.ResetDynamics( );
			_gear3.ResetDynamics( );
			_gear1.SetTransform( _gear1.Position + relative, _gear1.Rotation );
			_gear2.SetTransform( _gear2.Position + relative, _gear2.Rotation );
			_gear3.SetTransform( _gear3.Position + relative, _gear3.Rotation );

			foreach ( Body b in _bodies )
			{
				b.ResetDynamics();
				b.SetTransform( b.Position + relative, b.Rotation );
			}

		}

		public Vector2 GetPosition()
		{
			return _tankBody.Position;
		}

		public void SetFreeze( bool value )
		{
			_tankBody.Enabled = !value;

			_gear1.Enabled = !value;
			_gear2.Enabled = !value;
			_gear3.Enabled = !value;

			foreach ( Body b in _bodies )
			{
				b.Enabled = !value;
			}

			_frozen = value;
		}

		public bool IsFrozen()
		{
			return _frozen;
		}


		public PhysicsEntityTank( World physicsWorld )
		{
			_physicsWorld = physicsWorld;
			loadPhysics();
		}

		public PhysicsEntityTank( World physicsWorld, Vector2 position )
		{
			_physicsWorld = physicsWorld;
			loadPhysics( );
			SetPosition( position );
		}

		public void Move(float value)
		{
			_joint1.MotorSpeed = value;
			_joint2.MotorSpeed = value;
			_joint3.MotorSpeed = value;
		}

		private void loadPhysics()
		{
			_gear1 = BodyFactory.CreateCircle( _physicsWorld, 0.6f, 0.5f );
			_gear1.Position = new Vector2( -2.66f, 1.47f );
			_gear1.BodyType = BodyType.Dynamic;
			_gear1.Friction = 1f;
			_gear1.Restitution = 0f;

			_gear2 = BodyFactory.CreateCircle( _physicsWorld, 0.6f, 0.5f );
			_gear2.Position = new Vector2( 2.66f, 1.47f );
			_gear2.BodyType = BodyType.Dynamic;
			_gear2.Friction = 1f;
			_gear2.Restitution = 0f;

			_gear3 = BodyFactory.CreateCircle( _physicsWorld, 0.6f, 0.5f );
			_gear3.Position = new Vector2( 0f, -3.06f );
			_gear3.BodyType = BodyType.Dynamic;
			_gear3.Friction = 1f;
			_gear3.Restitution = 0f;

			Vertices chassis = new Vertices( 3 );
			chassis.Add( new Vector2( -2f, 1.14f ) );
			chassis.Add( new Vector2( 0f, -2.3f ) );
			chassis.Add( new Vector2( 2f, 1.14f ) );

			PolygonShape tankChassis = new PolygonShape( chassis, 2f );

			_tankBody = new Body( _physicsWorld );
			_tankBody.BodyType = BodyType.Dynamic;
			_tankBody.CreateFixture( tankChassis );
			_tankBody.Restitution = 0f;

			_tankBody.AngularDamping = 100f;

			_joint1 = new LineJoint( _tankBody, _gear1, _gear1.Position, new Vector2( 0.66f, -0.33f ) );
			_joint2 = new LineJoint( _tankBody, _gear2, _gear2.Position, new Vector2( -0.66f, -0.33f ) );
			_joint3 = new LineJoint( _tankBody, _gear3, _gear3.Position, new Vector2( 0f, 0.76f ) );

			_joint3.MaxMotorTorque = 60.0f;
			_joint3.MotorEnabled = true;
			_joint3.Frequency = 20f;
			_joint3.DampingRatio = 0.85f;

			_joint1.MaxMotorTorque = 60.0f;
			_joint1.MotorEnabled = true;
			_joint1.Frequency = 20f;
			_joint1.DampingRatio = 0.85f;

			_joint2.MaxMotorTorque = 60.0f;
			_joint2.MotorEnabled = true;
			_joint2.Frequency = 20f;
			_joint2.DampingRatio = 0.85f;

			_physicsWorld.AddJoint( _joint1 );
			_physicsWorld.AddJoint( _joint2 );
			_physicsWorld.AddJoint( _joint3 );

			_path = new Path( );

			_path.Add( new Vector2( -3.9f, 2.8f ) );
			_path.Add( new Vector2( 0f, -4.6f ) );
			_path.Add( new Vector2( 3.9f, 2.8f ) );

			_path.Closed = true;

			List<Shape> shapes = new List<Shape>( 2 );

			shapes.Add( new PolygonShape( PolygonTools.CreateRectangle( 0.15f, 0.27f, new Vector2( 0.075f, 0.135f ), 0f ), 2f ) );
			shapes.Add( new CircleShape( 0.27f, 2f ) );

			_bodies = PathManager.EvenlyDistributeShapesAlongPath( _physicsWorld, _path, shapes, BodyType.Dynamic, 30, 1 );

			foreach ( Body b in _bodies )
			{
				b.Friction = 1f;
				b.Restitution = 0f;
			}

			PathManager.AttachBodiesWithRevoluteJoint( _physicsWorld, _bodies, new Vector2( 0, 0.365f ), new Vector2( 0, -0.365f ), true, false ); 
		}


	}
}