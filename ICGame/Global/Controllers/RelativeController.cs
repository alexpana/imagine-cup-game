using System.Collections.Generic;
using Microsoft.Xna.Framework;
using VertexArmy.Global.Behaviours;

namespace VertexArmy.Global.Controllers
{
	public class RelativeController : IController
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
			Data = new List<object> { outputTransformable, inputTransformable };
		}

		public void Update( GameTime dt )
		{
			ITransformable outTrans = Data[0] as ITransformable;
			ITransformable inTrans = Data[1] as ITransformable;

			bool ok = ( outTrans != null && inTrans != null );

			if ( !ok ) return;


			float rotationDelta = ( inTrans.GetRotation() - _lastBodyRotation ).LengthSquared();
			float positionDelta = ( inTrans.GetPosition() - _lastBodyPosition ).LengthSquared();

			if ( rotationDelta > RotationError || positionDelta > PositionError )
			{
				outTrans.SetPosition( inTrans.GetPosition() + RelativePosition );
				outTrans.SetRotation( inTrans.GetRotation() );

				_lastBodyPosition = inTrans.GetPosition();
				_lastBodyRotation = inTrans.GetRotation();
			}
		}

		public List<object> Data { get; set; }

		public void Clean()
		{

		}
	}
}
