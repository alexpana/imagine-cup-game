using System.Collections.Generic;
using Microsoft.Xna.Framework;
using VertexArmy.Global.Behaviours;
using VertexArmy.Graphics;

namespace VertexArmy.Global.Controllers
{
	public class CameraController : IController, IUpdatable
	{
		private Vector3 _delta = Vector3.Zero;
		public CameraController( ITransformable transformable, CameraAttachable camera )
		{
			IParameter transParam = new ParameterTransformable
			{
				Alive = true,
				Input = true,
				Null = false,
				Output = false,
				Value = transformable
			};

			IParameter cameraParam = new ParameterCamera
			{
				Alive = true,
				Input = false,
				Null = false,
				Output = true,
				Value = camera
			};
			Data = new List<IParameter> { transParam, cameraParam };
		}

		public void Update( GameTime dt )
		{
			ParameterTransformable trans = Data[0] as ParameterTransformable;
			ParameterCamera camera = Data[1] as ParameterCamera;

			bool ok = ( trans != null && camera != null );

			if ( !ok ) return;


			List<IParameter> parameters = Data;
			DirectCompute( ref parameters );
		}

		public void DirectCompute( ref List<IParameter> data )
		{
			ParameterTransformable trans = Data[0] as ParameterTransformable;
			ParameterCamera camera = Data[1] as ParameterCamera;

			bool apply = ( trans != null && camera != null );

			if ( !apply ) return;

			apply = !trans.Null && !camera.Null;
			apply = apply && ( trans.Input && camera.Output );
			apply = apply && ( trans.Alive && camera.Alive );


			if ( !apply ) return;



			_delta = trans.Value.GetPosition() - camera.Value.Parent.GetPosition();


			Vector3 lookingPosition = camera.Value.Parent.GetPosition() + _delta + new Vector3( -40, 40, 600 );
			Vector3 lookingDirection = Vector3.Normalize( -lookingPosition + trans.Value.GetPosition() + new Vector3( 40, -40, 0 ) );

			camera.Value.Parent.SetPosition( lookingPosition );
			camera.Value.LookingDirection = lookingDirection;
		}

		public List<IParameter> Data { get; set; }
	}
}
