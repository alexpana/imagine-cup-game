using System;
using System.Collections.Generic;
using FarseerPhysics.Dynamics.Joints;
using Microsoft.Xna.Framework;
using VertexArmy.Global.Behaviours;

namespace VertexArmy.Global.Controllers.Components
{
	public class LiftedDoorComponent : BaseComponent
	{
		private readonly string _jointName;
		public LiftedDoorComponent( BaseComponent activator, string jointName )
		{
			Data = new List<IParameter>();

			IParameter activatorParam = new ParameterComponent()
			{
				Alive = true,
				Input = true,
				Null = false,
				Output = false,
				Value = activator
			};

			Data.Add( activatorParam );
			_jointName = jointName;
		}

		public override void InitEntity()
		{
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
				ParameterComponent comp = Data[0] as ParameterComponent;

				if ( comp != null && comp.Value != null )
				{
					if ( comp.Value.Type.Equals( ComponentType.ButtonComponent ) )
					{
						bool state = ( ( ButtonComponent ) comp.Value ).ButtonState;

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