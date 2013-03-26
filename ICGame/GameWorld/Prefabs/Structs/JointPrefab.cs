using FarseerPhysics.Dynamics;
using FarseerPhysics.Dynamics.Joints;
using Microsoft.Xna.Framework;
using VertexArmy.Global;
using VertexArmy.Global.Managers;
using VertexArmy.Utilities;

namespace VertexArmy.GameWorld.Prefabs.Structs
{
	public class JointPrefab
	{
		public string Name { get; set; }
		public JointType Type { get; set; }

		public string Body1, Body2;
		public Vector2 Anchor, Anchor2, Axis;

		public GameEntity FatherEntity { get; set; }
		public float MaxMotorTorque;
		public bool MotorEnabled;
		public float Frequency;
		public float DampingRatio;


		public Joint GetPhysicsJoint( float scale )
		{
			Body body1, body2;
			string[] bodyNames = PrefabUtils.GetEntityAndComponentName( Body1 );
			if ( bodyNames[0] == null )
			{
				body1 = FatherEntity.PhysicsEntity.GetBody( Body1 );
			}
			else
			{
				body1 = GameWorldManager.Instance.GetEntity( bodyNames[0] ).PhysicsEntity.GetBody( bodyNames[1] );
			}

			bodyNames = PrefabUtils.GetEntityAndComponentName( Body2 );
			if ( bodyNames[0] == null )
			{
				body2 = FatherEntity.PhysicsEntity.GetBody( Body2 );
			}
			else
			{
				body2 = GameWorldManager.Instance.GetEntity( bodyNames[0] ).PhysicsEntity.GetBody( bodyNames[1] );
			}

			switch ( Type )
			{
				case JointType.Line:
					LineJoint joint = new LineJoint( body1, body2, UnitsConverter.ToSimUnits( Anchor ) * scale, Axis );

					joint.MaxMotorTorque = MaxMotorTorque;
					joint.MotorEnabled = MotorEnabled;
					joint.Frequency = Frequency;
					joint.DampingRatio = DampingRatio;

					Platform.Instance.PhysicsWorld.AddJoint( joint );
					return joint;
				case JointType.Revolute:
					return new RevoluteJoint( body1, body2, UnitsConverter.ToSimUnits( Anchor ) * scale, UnitsConverter.ToSimUnits( Anchor2 ) * scale );
				case JointType.Weld:
					return new WeldJoint( body1, body2, UnitsConverter.ToSimUnits( Anchor ) * scale, UnitsConverter.ToSimUnits( Anchor2 ) * scale );
				default:
					return null;
			}
		}
	}
}