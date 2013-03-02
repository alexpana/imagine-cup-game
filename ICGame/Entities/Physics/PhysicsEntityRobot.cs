using System.Collections.Generic;
using System.Runtime.Serialization;
using FarseerPhysics.Collision.Shapes;
using FarseerPhysics.Common;
using FarseerPhysics.Dynamics;
using FarseerPhysics.Dynamics.Joints;
using FarseerPhysics.Factories;
using Microsoft.Xna.Framework;
using VertexArmy.Global;
using VertexArmy.Utilities;

namespace VertexArmy.Entities.Physics
{
	[DataContract]
	public class PhysicsEntityRobot : IPhysicsEntity
	{
		private bool _enabled;

		private Body _robotBody;

		private Path _path;

		private List<Body> _bodies;

		private Body _gear1;
		private Body _gear2;
		private Body _gear3;

		private LineJoint _joint1;
		private LineJoint _joint2;
		private LineJoint _joint3;

		[DataMember]
		private float _scale = 0.7f;

		/* Serialization helpers */
		[DataMember]
		private Vector2 _serializePosition;

		/* Interface requirement */
		public Vector2 Position
		{
			set
			{
				Vector2 relative = value - _robotBody.Position;

				_robotBody.ResetDynamics();
				_robotBody.SetTransform( value, _robotBody.Rotation );

				_gear1.ResetDynamics();
				_gear2.ResetDynamics();
				_gear3.ResetDynamics();
				_gear1.SetTransform( _gear1.Position + relative, _gear1.Rotation );
				_gear2.SetTransform( _gear2.Position + relative, _gear2.Rotation );
				_gear3.SetTransform( _gear3.Position + relative, _gear3.Rotation );

				foreach ( Body b in _bodies )
				{
					b.ResetDynamics();
					b.SetTransform( b.Position + relative, b.Rotation );
				}
			}

			get { return _robotBody.Position; }
		}

		public float Rotation
		{
			set
			{
				float modifier = value - Rotation;
				BodyUtility.RotateBodyAroundPoint( _gear1, _robotBody.Position, modifier );
				BodyUtility.RotateBodyAroundPoint( _gear2, _robotBody.Position, modifier );
				BodyUtility.RotateBodyAroundPoint( _gear3, _robotBody.Position, modifier );

				foreach (Body b in _bodies)
				{
					BodyUtility.RotateBodyAroundPoint( b, _robotBody.Position, modifier );
				}

				BodyUtility.RotateBodyAroundPoint( _robotBody, _robotBody.Position, modifier );
			}
			get { return _robotBody.Rotation; }
		}

		public bool Enabled
		{
			set
			{
				_robotBody.Enabled = value;

				_gear1.Enabled = value;
				_gear2.Enabled = value;
				_gear3.Enabled = value;

				foreach ( Body b in _bodies )
				{
					b.Enabled = value;
				}

				_enabled = value;
			}

			get { return _enabled; }
		}

		/* Custom properties */

		public float Scale 
		{ 
			get { return _scale; }
		}
		public int TreadCount
		{
			get { return _bodies.Count; }
		}

		public float ChassisRotation
		{
			get { return _robotBody.Rotation; }
		}

		public Vector2 ChassisPosition
		{
			get { return _robotBody.Position; }
		}

		/* Constructors */
		public PhysicsEntityRobot( float scale )
		{
			_enabled = false;
			_scale = scale;
			LoadPhysics();
		}

		public PhysicsEntityRobot( float scale, Vector2 position )
		{
			_scale = scale;
			_enabled = true;
			LoadPhysics();
			Position = position;
		}

		/* Rest of methods */
		public void Move( float value )
		{
			_joint1.MotorSpeed = value * _scale;
			_joint2.MotorSpeed = value * _scale;
			_joint3.MotorSpeed = value * _scale;
		}

		private void LoadPhysics()
		{
			float gearRadius = 1.215f;

			_gear1 = BodyFactory.CreateCircle( Platform.Instance.PhysicsWorld, gearRadius * _scale, 1f );
			_gear1.Position = new Vector2( -2.301f * _scale, 1.243f * _scale );
			_gear1.BodyType = BodyType.Dynamic;
			_gear1.Friction = 1f * _scale;
			_gear1.Restitution = 0f;

			_gear2 = BodyFactory.CreateCircle( Platform.Instance.PhysicsWorld, gearRadius * _scale, 1f );
			_gear2.Position = new Vector2( 2.301f * _scale, 1.243f * _scale );
			_gear2.BodyType = BodyType.Dynamic;
			_gear2.Friction = 1f * _scale;
			_gear2.Restitution = 0f;

			_gear3 = BodyFactory.CreateCircle( Platform.Instance.PhysicsWorld, gearRadius * _scale, 1f );
			_gear3.Position = new Vector2( 0f * _scale, -2.615f * _scale );
			_gear3.BodyType = BodyType.Dynamic;
			_gear3.Friction = 1f * _scale;
			_gear3.Restitution = 0f;

			Vertices chassis = new Vertices( 3 );
			chassis.Add( new Vector2( -1.215f * _scale, 0.7f * _scale ) );
			chassis.Add( new Vector2( 0f * _scale, -1.4f * _scale ) );
			chassis.Add( new Vector2( 1.215f * _scale, 0.7f * _scale ) );

			PolygonShape robotChassis = new PolygonShape( chassis, 7f );

			_robotBody = new Body( Platform.Instance.PhysicsWorld );
			_robotBody.BodyType = BodyType.Dynamic;
			_robotBody.CreateFixture( robotChassis );
			_robotBody.Restitution = 0f;

			_robotBody.AngularDamping = 100f;

			_joint1 = new LineJoint( _robotBody, _gear1, _gear1.Position, new Vector2( 0.66f, -0.33f ) * 1f );
			_joint2 = new LineJoint( _robotBody, _gear2, _gear2.Position, new Vector2( -0.66f, -0.33f ) * 1f );
			_joint3 = new LineJoint( _robotBody, _gear3, _gear3.Position, new Vector2( 0f, 0.76f ) * 1f);

			_joint3.MaxMotorTorque = 60.0f * _scale;
			_joint3.MotorEnabled = true;
			_joint3.Frequency = 10f;
			_joint3.DampingRatio = 0.85f;

			_joint1.MaxMotorTorque = 60.0f * _scale;
			_joint1.MotorEnabled = true;
			_joint1.Frequency = 10f;
			_joint1.DampingRatio = 0.85f;

			_joint2.MaxMotorTorque = 60.0f * _scale;
			_joint2.MotorEnabled = true;
			_joint2.Frequency = 10f;
			_joint2.DampingRatio = 0.85f;

			Platform.Instance.PhysicsWorld.AddJoint( _joint1 );
			Platform.Instance.PhysicsWorld.AddJoint( _joint2 );
			Platform.Instance.PhysicsWorld.AddJoint( _joint3 );

			_path = new Path();

			_path.Add( new Vector2( 3.9f * _scale, 2.8f * _scale ) );
			_path.Add( new Vector2( 0f * _scale, -4.6f * _scale ) );
			_path.Add( new Vector2( -3.9f * _scale, 2.8f * _scale ) );

			_path.Closed = true;

			List<Shape> shapes = new List<Shape>( 2 );

			shapes.Add( new PolygonShape( PolygonTools.CreateRectangle( 0.101f * _scale, 0.32f * _scale, new Vector2( 0f, 0f * _scale), 0f ), 2f ) );
			shapes.Add( new PolygonShape( PolygonTools.CreateRectangle( 0.1f * _scale, 0.18f * _scale, new Vector2( 0.12f * _scale, 0f * _scale ), 0f ), 2f ) );

			_bodies = PathManager.EvenlyDistributeShapesAlongPath( Platform.Instance.PhysicsWorld, _path, shapes, BodyType.Dynamic, 29, 1 );

			foreach ( Body b in _bodies )
			{
				b.Friction = 1f * _scale;
				b.Restitution = 0f;
			}

			PathManager.AttachBodiesWithRevoluteJoint( Platform.Instance.PhysicsWorld, _bodies, new Vector2( -0.101f * _scale, 0.375f * _scale ), new Vector2( -0.15f * _scale, -0.375f * _scale ), true, false );
			 
		}

		public Vector2 GetGearPosition( int index )
		{
			switch ( index )
			{
				case 0:
					return _gear1.Position;
				case 1:
					return _gear2.Position;
				case 2:
					return _gear3.Position;
			}

			return Vector2.Zero;
		}

		public Vector2 GetTreadPosition( int index )
		{
			return _bodies[index].Position;
		}

		public float GetGearRotation( int index )
		{
			switch ( index )
			{
				case 0:
					return _gear1.Rotation;
				case 1:
					return _gear2.Rotation;
				case 2:
					return _gear3.Rotation;
			}

			return 0f;
		}

		public float GetTreadRotation( int index )
		{
			return _bodies[index].Rotation;
		}

		public void PreSerialize()
		{
			_serializePosition = _robotBody.Position;
		}
		public void PostDeserialize()
		{
			LoadPhysics();
			Position = _serializePosition;
		}
	}
}