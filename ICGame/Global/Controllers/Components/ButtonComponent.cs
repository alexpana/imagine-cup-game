using System.Collections.Generic;
using System.Diagnostics;
using FarseerPhysics.Dynamics.Joints;
using Microsoft.Xna.Framework;

namespace VertexArmy.Global.Controllers.Components
{
	public class ButtonComponent : BaseComponent
	{
		private readonly string _jointName;
		private bool _inversed;
		private bool _permanent;
		private bool _activatedOnce;

		public bool ButtonState { get; internal set; }

		public ButtonComponent( string jointName, bool inversed = false, bool permanent = false )
		{
			Data = new List<object>();
			_jointName = jointName;
			_type = ComponentType.ButtonComponent;
			_inversed = inversed;
			_permanent = permanent;
			_activatedOnce = false;
		}

		public override void InitEntity()
		{
			Debug.Assert( Entity.PhysicsEntity.GetJoint( _jointName ) != null );
			ButtonState = false;
		}

		public override void Update( GameTime dt )
		{
			if ( Entity != null )
			{
				if ( _permanent && _activatedOnce )
				{
					return;
				}
				PrismaticJoint j = Entity.PhysicsEntity.GetJoint( _jointName ) as PrismaticJoint;
				ButtonState = _inversed;
				if ( j.JointTranslation < ( j.UpperLimit - j.LowerLimit ) / 2f )
				{
					_activatedOnce = true;
					ButtonState = !_inversed;
				}
			}
		}

		public override void Clean()
		{

		}
	}
}