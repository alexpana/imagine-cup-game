using System.Collections.Generic;
using System.Diagnostics;
using FarseerPhysics.Dynamics.Joints;
using Microsoft.Xna.Framework;
using VertexArmy.Global.Behaviours;

namespace VertexArmy.Global.Controllers.Components
{
	public class ButtonComponent : BaseComponent
	{
		private readonly string _jointName;

		public bool ButtonState { get; internal set; }

		public ButtonComponent( string jointName )
		{
			Data = new List<IParameter>();
			_jointName = jointName;
			_type = ComponentType.ButtonComponent;
		}

		public override void InitEntity()
		{
			Debug.Assert( Entity.PhysicsEntity.GetJoint( _jointName ) != null );
			ButtonState = false;
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
				PrismaticJoint j = Entity.PhysicsEntity.GetJoint( _jointName ) as PrismaticJoint;
				ButtonState = false;
				if ( j.JointTranslation < ( j.UpperLimit - j.LowerLimit ) / 2f )
				{
					ButtonState = true;
				}
			}
		}

		public override void Clean()
		{

		}
	}
}