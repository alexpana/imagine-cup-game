using System.Collections.Generic;
using System.Diagnostics;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Input;
using VertexArmy.Global.Behaviours;

namespace VertexArmy.Global.Controllers.Components
{
	public class CarControlComponent : BaseComponent
	{
		private readonly List<string> _lineJoints;
		private readonly List<float> _lineJointsSpeeds;

		public CarControlComponent( ICollection<string> lineJointNames, ICollection<float> speeds )
		{
			Data = new List<IParameter>();
			Debug.Assert( lineJointNames.Count == speeds.Count );

			_type = ComponentType.CarControlComponent;
			_lineJoints = new List<string>();
			_lineJoints.AddRange( lineJointNames );
			_lineJointsSpeeds = new List<float>();
			_lineJointsSpeeds.AddRange( speeds );

			ParameterFloat direction = new ParameterFloat
			{
				Alive = true,
				Input = true,
				Null = false,
				Output = false,
				Value = 0f
			};

			Data.Add( direction );
		}

		public override void InitEntity()
		{
		}


		public override void Update( GameTime dt )
		{
			List<IParameter> parameters = Data;
			float direction = 0f;
			if ( Keyboard.GetState().IsKeyDown( Keys.Left ) )
			{
				direction = -1f;
			}
			else if ( Keyboard.GetState().IsKeyDown( Keys.Right ) )
			{
				direction = 1f;
			}

			( ( ParameterFloat ) parameters[0] ).Value = direction;
			DirectCompute( ref parameters );
		}

		public override void DirectCompute( ref List<IParameter> data )
		{
			if ( Entity != null && Entity.PhysicsEntity.Enabled )
			{
				var parameterFloat = data[0] as ParameterFloat;
				if ( parameterFloat != null )
				{
					float direction = parameterFloat.Value;
					for ( int i = 0; i < _lineJoints.Count; i++ )
					{
						Entity.PhysicsEntity.SetLineJointMotorSpeed( _lineJoints[i], direction * _lineJointsSpeeds[i] );
					}
				}
			}
		}

		public override void Clean()
		{
			_lineJoints.Clear();
			_lineJointsSpeeds.Clear();
		}
	}
}