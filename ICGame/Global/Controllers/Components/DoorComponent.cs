using System;
using System.Collections.Generic;
using FarseerPhysics.Dynamics.Joints;
using Microsoft.Xna.Framework;

namespace VertexArmy.Global.Controllers.Components
{
	public class LiftedDoorComponent : BaseComponent
	{
		private readonly string _jointName;
		public LiftedDoorComponent( BaseComponent activator, string jointName )
		{
			Data = new List<object> { activator };
			_jointName = jointName;
		}

		public override void InitEntity()
		{
		}

		public override void Update( GameTime dt )
		{
			if ( Entity != null )
			{
				BaseComponent comp = Data[0] as BaseComponent;

				if ( comp != null && comp != null )
				{
					if ( comp.Type.Equals( ComponentType.ButtonComponent ) )
					{
						bool state = ( ( ButtonComponent ) comp ).ButtonState;

						float motorSpeed = ( ( PrismaticJoint ) Entity.PhysicsEntity.GetJoint( _jointName ) ).MotorSpeed;
						if ( state )
						{
							( ( PrismaticJoint ) Entity.PhysicsEntity.GetJoint( _jointName ) ).MotorSpeed = Math.Abs( motorSpeed ) * -1;
						}
						else
						{
							( ( PrismaticJoint ) Entity.PhysicsEntity.GetJoint( _jointName ) ).MotorSpeed = Math.Abs( motorSpeed );
						}
					}
				}
			}
		}

		public override void Clean()
		{

		}
	}
}