using System.Collections.Generic;
using Microsoft.Xna.Framework;
using VertexArmy.Global.Behaviours;
using VertexArmy.Graphics.Attachables;

namespace VertexArmy.Global.Controllers
{
	public class CameraController : IController
	{
		private Vector3 _delta = Vector3.Zero;

		public CameraController( ITransformable transformable, CameraAttachable camera )
		{
			Data = new List<object> { transformable, camera };
		}

		public void Update( GameTime dt )
		{
			ITransformable trans = Data[0] as ITransformable;
			CameraAttachable camera = Data[1] as CameraAttachable;

			bool ok = ( trans != null && camera != null );

			if ( !ok ) return;


			_delta = trans.GetPosition() - camera.Parent.GetPosition() + new Vector3( 0, 130, 800 );

			_delta /= 40;

			Vector3 lookingPosition = camera.Parent.GetPosition() + _delta;
			Vector3 lookingDirection = Vector3.Normalize( -lookingPosition + trans.GetPosition() );

			camera.Parent.SetPosition( lookingPosition );
			//camera.Value.LookingDirection = lookingDirection;
		}


		public List<object> Data { get; set; }

		public void Clean()
		{
		}
	}
}
