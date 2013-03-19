using System.Collections.Generic;
using FarseerPhysics.Common;
using FarseerPhysics.Dynamics;
using FarseerPhysics.Dynamics.Joints;
using Microsoft.Xna.Framework;
using VertexArmy.Global;
using VertexArmy.Utilities;

namespace VertexArmy.GameWorld
{
	public class PhysicsEntity
	{
		private bool _enabled;

		private Dictionary<string, Body> _bodies;
		private Dictionary<string, Joint> _joints;
		private Dictionary<string, PathEntity> _paths;

		private float _z;

		public float Depth
		{
			get { return _z; }
		}

		public PhysicsEntity()
		{
			_bodies = new Dictionary<string, Body>( );
			_joints = new Dictionary<string, Joint>( );
			_paths = new Dictionary<string, PathEntity>( );

			_enabled = true;
		}

		public ICollection<Body> Bodies
		{

			get { return _bodies.Values; }

		}

		public ICollection<Joint> Joints
		{
			get { return _joints.Values; }
		}

		public ICollection<PathEntity> Paths
		{
			get { return _paths.Values; }
		}

		public bool Enabled
		{
			set
			{
				foreach ( Body b in _bodies.Values )
				{
					b.Enabled = value;
				}

				foreach ( var p in _paths.Values )
				{
					foreach ( var b in p.Bodies )
					{
						b.Enabled = value;
					}
				}

				_enabled = value;
			}

			get { return _enabled; }
		}

		public ICollection<string> BodyNames
		{
			get { return _bodies.Keys; }
		}

		public ICollection<string> JointNames
		{
			get { return _joints.Keys; }
		}

		public ICollection<string> PathNames
		{
			get { return _paths.Keys; }
		}

		public void AddBody( string name, Body body )
		{
			_bodies.Add( name, body );
		}

		public void AddBody( string name, Body body, bool main )
		{
			_bodies.Add( name, body );
		}

		public void AddJoint( string name, Joint joint )
		{
			_joints.Add( name, joint );

		}

		public void AddPath( string name, PathEntity path )
		{
			_paths.Add( name, path );
		}

		public Body GetBody( string name )
		{
			if ( _bodies.ContainsKey( name ) )
			{
				return _bodies[name];
			}

			return null;
		}

		public Joint GetJoint( string name )
		{
			if ( _joints.ContainsKey( name ) )
			{
				return _joints[name];
			}

			return null;
		}

		public Body GetBodyFromPath( string name, int index )
		{
			if ( _paths.ContainsKey( name ) && index < _paths[name].Bodies.Count )
			{
				return _paths[name].Bodies[index];
			}

			return null;
		}

		public int GetBodyCountFromPath( string name )
		{
			if ( _paths.ContainsKey( name ) )
			{
				return _paths[name].Bodies.Count;
			}

			return -1;
		}

		public Joint GetJointFromPath( string name, int index )
		{
			if ( _paths.ContainsKey( name ) && index < _paths[name].Joints.Count )
			{
				return _paths[name].Joints[index];
			}

			return null;
		}

		public int GetJointCountFromPath( string name )
		{
			if ( _paths.ContainsKey( name ) )
			{
				return _paths[name].Joints.Count;
			}

			return -1;
		}

		public void SetPosition( Body center, Vector2 newPosition, float z )
		{
			Vector2 relative = newPosition - center.Position;

			foreach ( Body b in _bodies.Values )
			{
				b.SetTransform( b.Position + relative, b.Rotation );
			}

			foreach ( var p in _paths.Values )
			{
				foreach ( var b in p.Bodies )
				{
					b.ResetDynamics( );
					b.SetTransform( b.Position + relative, b.Rotation );
				}
			}

			_z = z;
		}

		public void SetRotation( Body center, float newRotation )
		{
			float modifier = newRotation - center.Rotation;

			foreach ( Body b in _bodies.Values )
			{
				TransformUtility.RotateBodyAroundPoint( b, center.Position, modifier );
			}

			foreach ( var p in _paths.Values )
			{
				foreach ( var b in p.Bodies )
				{
					TransformUtility.RotateBodyAroundPoint( b, center.Position, modifier );
				}
			}

		}

		public void SetLineJointMotorSpeed( List<string> jointNames, float motorSpeed )
		{
			foreach ( string jointName in jointNames )
			{
				SetLineJointMotorSpeed( jointName, motorSpeed );
			}
		}

		public void SetLineJointMotorSpeed( string jointName, float motorSpeed )
		{
			if ( _joints.ContainsKey( jointName ) && _joints[jointName].JointType.Equals( JointType.Line ) )
			{
				( ( LineJoint ) _joints[jointName] ).MotorSpeed = motorSpeed;
			}
		}

		public void Remove()
		{
			foreach ( Body b in Bodies )
			{
				Platform.Instance.PhysicsWorld.RemoveBody( b );
			}

			foreach ( var p in _paths.Values )
			{
				foreach ( var b in p.Bodies )
				{
					Platform.Instance.PhysicsWorld.RemoveBody( b );
				}
				foreach ( var j in p.Joints )
				{
					Platform.Instance.PhysicsWorld.RemoveJoint( j );
				}
			}

			foreach ( var j in _joints.Values )
			{
				Platform.Instance.PhysicsWorld.RemoveJoint( j );
			}
		}
	}

	public struct PathEntity
	{
		public Path Path;
		public List<Body> Bodies;
		public List<Joint> Joints;
	}
}