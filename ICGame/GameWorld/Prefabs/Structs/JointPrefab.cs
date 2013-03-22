using FarseerPhysics.Dynamics;
using FarseerPhysics.Dynamics.Joints;
using Microsoft.Xna.Framework;
using VertexArmy.Global;
using VertexArmy.Utilities;

namespace VertexArmy.GameWorld.Prefabs.Structs
{
	public struct JointPrefab
	{
		public string Name { get; set; }
		public JointType Type { get; set; }

		public string Body1, Body2;
		public Vector2 Anchor, Anchor2, Axis;

		public float MaxMotorTorque;
		public bool MotorEnabled;
		public float Frequency;
		public float DampingRatio;


		public Joint GetPhysicsJoint( Body body1, Body body2, float scale )
		{
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