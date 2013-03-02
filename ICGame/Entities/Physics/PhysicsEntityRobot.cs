using System;
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
		/* constants */
		private const float GearRadius = 1.215f;
		private const float GearDensity = 1f;
		private const float GearMaxTorque = 60f;
		private const float GearDistanceFromChassis = GearRadius;

		private const float JointFrequency = 10f;
		private const float JointDamping = 0.85f;
		private const float JointLengthModifier = 1f;

		private const float ChassisEdgeSize = 2.43f;
		private const float ChassisDensity = 7f;
		private const float ChassisAngularDamping = 100f;

		public const int TreadCount = 29;

		private const float TreadDistanceFromGearModifier = 1.7f;
		private Vector2 _treadSize = new Vector2(0.101f,0.32f);
		private const float TreadDensity = 2f;

		private Vector2 _treadFeetSize = new Vector2(0.1f,0.18f);
		private const float TreadFeetDensity = 2f;

		private Vector2 _treadJointAnchorLeft = new Vector2(-0.101f, 0.375f);
		private Vector2 _treadJointAnchorRight = new Vector2( -0.15f, -0.375f);

		/* members */
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

		private float _robotSpeed;
		private float _robotMaxSpeed;

		private Vector2 _lastRobotPosition;

		[DataMember]
		private float _scale;

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
				resetMaxAttainedSpeed();
				_lastRobotPosition = Position;
			}

			get { return _robotBody.Position; }
		}

		public bool Awake
		{
			set 
			{ 
				_robotBody.Awake = value;
				_gear1.Awake = value;
				_gear2.Awake = value;
				_gear3.Awake = value;

				foreach ( Body b in _bodies )
				{
					b.Awake = value;
				}
			} 

			get { return _robotBody.Awake; }
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

		public float ChassisRotation
		{
			get { return _robotBody.Rotation; }
		}

		public Vector2 ChassisPosition
		{
			get { return _robotBody.Position; }
		}

		public float Speed
		{
			get { return _robotSpeed; }
		}

		public float MaxAttainedSpeed
		{
			get { return _robotMaxSpeed; }
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
			float chassisHeight = ChassisEdgeSize * ( float ) Math.Sqrt( 3 ) / 2f;

			Vertices chassis = new Vertices( 3 );
			chassis.Add( new Vector2( -ChassisEdgeSize / 2f * _scale, chassisHeight / 3f * _scale ) );
			chassis.Add( new Vector2( 0f * _scale, -chassisHeight * 2f / 3f * _scale ) );
			chassis.Add( new Vector2( ChassisEdgeSize / 2f * _scale, chassisHeight / 3f * _scale ) );

			float gearDistanceHeight = GearDistanceFromChassis / (float)Math.Sqrt( 5 );
			float gearDistanceWidth = gearDistanceHeight * 2;

			_gear1 = BodyFactory.CreateCircle( Platform.Instance.PhysicsWorld, GearRadius * _scale, GearDensity );
			_gear1.Position = new Vector2( chassis[0].X - ( gearDistanceWidth * _scale ), chassis[0].Y + ( gearDistanceHeight * _scale ) );
			_gear1.BodyType = BodyType.Dynamic;
			_gear1.Friction = 1f * _scale;
			_gear1.Restitution = 0f;

			_gear2 = BodyFactory.CreateCircle( Platform.Instance.PhysicsWorld, GearRadius * _scale, GearDensity );
			_gear2.Position = new Vector2( chassis[2].X + ( gearDistanceWidth * _scale ), chassis[2].Y + ( gearDistanceHeight * _scale ) );
			_gear2.BodyType = BodyType.Dynamic;
			_gear2.Friction = 1f * _scale;
			_gear2.Restitution = 0f;

			_gear3 = BodyFactory.CreateCircle( Platform.Instance.PhysicsWorld, GearRadius * _scale, GearDensity );
			_gear3.Position = new Vector2( 0f, chassis[1].Y - (GearDistanceFromChassis * _scale) );
			_gear3.BodyType = BodyType.Dynamic;
			_gear3.Friction = 1f * _scale;
			_gear3.Restitution = 0f;

			PolygonShape robotChassis = new PolygonShape( chassis, ChassisDensity );

			_robotBody = new Body( Platform.Instance.PhysicsWorld );
			_robotBody.BodyType = BodyType.Dynamic;
			_robotBody.CreateFixture( robotChassis );
			_robotBody.Restitution = 0f;

			_robotBody.AngularDamping = ChassisAngularDamping;

			_joint1 = new LineJoint( _robotBody, _gear1, _gear1.Position, new Vector2( 0.66f, -0.33f ) * JointLengthModifier );
			_joint2 = new LineJoint( _robotBody, _gear2, _gear2.Position, new Vector2( -0.66f, -0.33f ) * JointLengthModifier );
			_joint3 = new LineJoint( _robotBody, _gear3, _gear3.Position, new Vector2( 0f, 0.76f ) * JointLengthModifier );

			_joint3.MaxMotorTorque = GearMaxTorque * _scale;
			_joint3.MotorEnabled = true;
			_joint3.Frequency = JointFrequency;
			_joint3.DampingRatio = JointDamping;

			_joint1.MaxMotorTorque = GearMaxTorque * _scale;
			_joint1.MotorEnabled = true;
			_joint1.Frequency = JointFrequency;
			_joint1.DampingRatio = JointDamping;

			_joint2.MaxMotorTorque = GearMaxTorque * _scale;
			_joint2.MotorEnabled = true;
			_joint2.Frequency = JointFrequency;
			_joint2.DampingRatio = JointDamping;

			Platform.Instance.PhysicsWorld.AddJoint( _joint1 );
			Platform.Instance.PhysicsWorld.AddJoint( _joint2 );
			Platform.Instance.PhysicsWorld.AddJoint( _joint3 );

			_path = new Path();

			_path.Add( _gear2.Position * TreadDistanceFromGearModifier );
			_path.Add( _gear3.Position * TreadDistanceFromGearModifier );
			_path.Add( _gear1.Position * TreadDistanceFromGearModifier );

			_path.Closed = true;

			List<Shape> shapes = new List<Shape>( 2 );

			shapes.Add( new PolygonShape( PolygonTools.CreateRectangle( _treadSize.X * _scale, _treadSize.Y * _scale, new Vector2( 0f, 0f ), 0f ), TreadDensity ) );
			shapes.Add( new PolygonShape( PolygonTools.CreateRectangle( _treadFeetSize.X * _scale, _treadFeetSize.Y * _scale, new Vector2( 0.12f * _scale, 0f * _scale ), 0f ), TreadFeetDensity ) );

			_bodies = PathManager.EvenlyDistributeShapesAlongPath( Platform.Instance.PhysicsWorld, _path, shapes, BodyType.Dynamic, TreadCount, 1 );

			foreach ( Body b in _bodies )
			{
				b.Friction = 1f * _scale;
				b.Restitution = 0f;
			}

			PathManager.AttachBodiesWithRevoluteJoint( Platform.Instance.PhysicsWorld, _bodies, new Vector2( _treadJointAnchorLeft.X * _scale, _treadJointAnchorLeft.Y * _scale ), new Vector2( _treadJointAnchorRight.X * _scale, _treadJointAnchorRight.Y * _scale ), true, false ); 
			 
		}

		public void resetMaxAttainedSpeed()
		{
			_robotMaxSpeed = 0f;
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

		public void ResetDynamics()
		{
			_robotBody.ResetDynamics();
			_gear1.ResetDynamics();
			_gear2.ResetDynamics( );
			_gear3.ResetDynamics( );

			foreach ( Body b in _bodies )
			{
				b.ResetDynamics();
			}
		}

		public void OnUpdate(GameTime dt)
		{
			float distance = ( Position - _lastRobotPosition ).Length( );
			_lastRobotPosition = Position;
			_robotSpeed = distance / ( float ) dt.ElapsedGameTime.TotalSeconds;

			bool moving = false;
			if ( _joint1.MotorSpeed != 0f)
			{
				moving = true;
			}

			if ( !moving && _robotSpeed < 0.1f )
			{
				ResetDynamics( );
			}

			if ( !moving && _robotSpeed < 0.01f )
			{
				Awake = false;
			}

			if ( _robotSpeed > _robotMaxSpeed )
			{
				_robotMaxSpeed = _robotSpeed;
			}
		}
	}
}
