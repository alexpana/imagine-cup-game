﻿using System.Collections.Generic;
using System.Runtime.Serialization;
using FarseerPhysics.Collision.Shapes;
using FarseerPhysics.Common;
using FarseerPhysics.Dynamics;
using FarseerPhysics.Dynamics.Joints;
using FarseerPhysics.Factories;
using Microsoft.Xna.Framework;
using VertexArmy.Global;

namespace VertexArmy.Entities.Physics
{
	[DataContract]
	public class PhysicsEntityTank : IPhysicsEntity
	{
		private bool _enabled;

		private Body _tankBody;

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
				Vector2 relative = value - _tankBody.Position;

				_tankBody.ResetDynamics();
				_tankBody.SetTransform( value, _tankBody.Rotation );

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

			get { return _tankBody.Position; }
		}

		public float Rotation
		{
			get { return _tankBody.Rotation; }
		}

		public bool Enabled
		{
			set
			{
				_tankBody.Enabled = value;

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
			get { return _tankBody.Rotation; }
		}

		public Vector2 ChassisPosition
		{
			get { return _tankBody.Position; }
		}

		/* Constructors */
		public PhysicsEntityTank( float scale )
		{
			_enabled = false;
			_scale = scale;
			LoadPhysics();
		}

		public PhysicsEntityTank( float scale, Vector2 position )
		{
			_scale = scale;
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
			_gear1 = BodyFactory.CreateCircle( Platform.Instance.PhysicsWorld, 0.6f * _scale, 0.5f );
			_gear1.Position = new Vector2( -2.66f * _scale, 1.47f * _scale );
			_gear1.BodyType = BodyType.Dynamic;
			_gear1.Friction = 1f;
			_gear1.Restitution = 0f;

			_gear2 = BodyFactory.CreateCircle( Platform.Instance.PhysicsWorld, 0.6f * _scale, 0.5f );
			_gear2.Position = new Vector2( 2.66f * _scale, 1.47f * _scale );
			_gear2.BodyType = BodyType.Dynamic;
			_gear2.Friction = 1f;
			_gear2.Restitution = 0f;

			_gear3 = BodyFactory.CreateCircle( Platform.Instance.PhysicsWorld, 0.6f * _scale, 0.5f );
			_gear3.Position = new Vector2( 0f * _scale, -3.06f * _scale );
			_gear3.BodyType = BodyType.Dynamic;
			_gear3.Friction = 1f;
			_gear3.Restitution = 0f;

			Vertices chassis = new Vertices( 3 );
			chassis.Add( new Vector2( -2f * _scale, 1.14f * _scale ) );
			chassis.Add( new Vector2( 0f * _scale, -2.3f * _scale ) );
			chassis.Add( new Vector2( 2f * _scale, 1.14f * _scale ) );

			PolygonShape tankChassis = new PolygonShape( chassis, 2f );

			_tankBody = new Body( Platform.Instance.PhysicsWorld );
			_tankBody.BodyType = BodyType.Dynamic;
			_tankBody.CreateFixture( tankChassis );
			_tankBody.Restitution = 0f;

			_tankBody.AngularDamping = 100f;

			_joint1 = new LineJoint( _tankBody, _gear1, _gear1.Position, new Vector2( 0.66f, -0.33f ) );
			_joint2 = new LineJoint( _tankBody, _gear2, _gear2.Position, new Vector2( -0.66f, -0.33f ) );
			_joint3 = new LineJoint( _tankBody, _gear3, _gear3.Position, new Vector2( 0f, 0.76f ) );

			_joint3.MaxMotorTorque = 60.0f * _scale;
			_joint3.MotorEnabled = true;
			_joint3.Frequency = 20f;
			_joint3.DampingRatio = 0.85f;

			_joint1.MaxMotorTorque = 60.0f * _scale;
			_joint1.MotorEnabled = true;
			_joint1.Frequency = 20f;
			_joint1.DampingRatio = 0.85f;

			_joint2.MaxMotorTorque = 60.0f * _scale;
			_joint2.MotorEnabled = true;
			_joint2.Frequency = 20f;
			_joint2.DampingRatio = 0.85f;

			Platform.Instance.PhysicsWorld.AddJoint( _joint1 );
			Platform.Instance.PhysicsWorld.AddJoint( _joint2 );
			Platform.Instance.PhysicsWorld.AddJoint( _joint3 );

			_path = new Path();

			_path.Add( new Vector2( -3.9f * _scale, 2.8f * _scale ) );
			_path.Add( new Vector2( 0f * _scale, -4.6f * _scale ) );
			_path.Add( new Vector2( 3.9f * _scale, 2.8f * _scale ) );

			_path.Closed = true;

			List<Shape> shapes = new List<Shape>( 2 );

			shapes.Add( new PolygonShape( PolygonTools.CreateRectangle( 0.15f * _scale, 0.27f * _scale, new Vector2( 0.075f * _scale, 0.135f * _scale ), 0f ), 2f ) );
			shapes.Add( new CircleShape( 0.27f * _scale, 2f ) );

			_bodies = PathManager.EvenlyDistributeShapesAlongPath( Platform.Instance.PhysicsWorld, _path, shapes, BodyType.Dynamic, 30, 1 );

			foreach ( Body b in _bodies )
			{
				b.Friction = 1f;
				b.Restitution = 0f;
			}

			PathManager.AttachBodiesWithRevoluteJoint( Platform.Instance.PhysicsWorld, _bodies, new Vector2( 0, 0.365f * _scale ), new Vector2( 0, -0.365f * _scale ), true, false );
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
			_serializePosition = _tankBody.Position;
		}
		public void PostDeserialize()
		{
			LoadPhysics();
			Position = _serializePosition;
		}
	}
}