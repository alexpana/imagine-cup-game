using System.Collections.Generic;
using Microsoft.Xna.Framework;
using VertexArmy.Global.Behaviors;

namespace VertexArmy.Global.Controllers
{
	public class RelativeController : IController, IUpdatable
	{
		private Vector3 _lastBodyPosition;
		private Quaternion _lastBodyRotation;

		private const float RotationError = 0.1f;
		private const float PositionError = 0.1f;
		public Vector3 RelativePosition { get; set; }

		public RelativeController( ITransformable outputTransformable, ITransformable inputTransformable )
			: this( outputTransformable, inputTransformable, Vector3.Zero )
		{
		}

		public RelativeController( ITransformable outputTransformable, ITransformable inputTransformable, Vector3 relativePosition )
		{

			RelativePosition = relativePosition;
			IParameter outTransParam = new ParameterTransformable
			{
				Alive = true,
				Input = false,
				Null = false,
				Output = true,
				Value = outputTransformable
			};

			IParameter inTransParam = new ParameterTransformable
			{
				Alive = true,
				Input = true,
				Null = false,
				Output = false,
				Value = inputTransformable
			};

			Data = new List<IParameter> { outTransParam, inTransParam };
		}

		public void Update( GameTime dt )
		{
			ParameterTransformable outTrans = Data[0] as ParameterTransformable;
			ParameterTransformable inTrans = Data[1] as ParameterTransformable;

			bool ok = ( outTrans != null && inTrans != null );

			if ( !ok ) return;


			float rotationDelta = ( inTrans.Value.GetRotation() - _lastBodyRotation ).LengthSquared();
			float positionDelta = ( inTrans.Value.GetPosition() - _lastBodyPosition ).LengthSquared();

			if ( rotationDelta > RotationError || positionDelta > PositionError )
			{
				List<IParameter> parameters = Data;
				DirectCompute( ref parameters );

				_lastBodyPosition = inTrans.Value.GetPosition();
				_lastBodyRotation = inTrans.Value.GetRotation();
			}
		}

		public void DirectCompute( ref List<IParameter> data )
		{
			ParameterTransformable outTrans = data[0] as ParameterTransformable;
			ParameterTransformable inTrans = data[1] as ParameterTransformable;

			bool apply = ( outTrans != null && inTrans != null );

			if ( !apply ) return;

			apply = !outTrans.Null && !inTrans.Null;
			apply = apply && ( outTrans.Output && inTrans.Input );
			apply = apply && ( outTrans.Alive && inTrans.Alive );

			if ( !apply ) return;

			outTrans.Value.SetPosition( inTrans.Value.GetPosition() + RelativePosition );
			outTrans.Value.SetRotation( inTrans.Value.GetRotation() );
		}

		public List<IParameter> Data { get; set; }
	}
}
