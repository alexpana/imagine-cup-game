using System;
using System.Collections.Generic;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Input;
using VertexArmy.Global.Behaviours;
using VertexArmy.Graphics.Attachables;

namespace VertexArmy.Global.Controllers
{
	public class OrbitCameraController : IController
	{
		private Vector2 _rotations;
		private float _distanceFromObject = 1000;


		public OrbitCameraController( ITransformable transformable, CameraAttachable camera )
		{
			Data = new List<object> { transformable, camera };
			_rotations.X = ( float ) Math.PI / 2.0f;
		}

		public void Update( GameTime dt )
		{
			if ( !IsInitialized() )
			{
				return;
			}

			ReadInput();
			UpdateParent();
		}

		private bool IsInitialized()
		{
			ITransformable trans = Data[0] as ITransformable;
			CameraAttachable camera = Data[1] as CameraAttachable;

			return ( trans != null && camera != null );
		}

		private Vector3 GetObjectivePosition()
		{
			return ( Data[0] as ITransformable ).GetPosition();
		}

		private void ReadInput()
		{
			_rotations.X += Platform.Instance.Input.PointerDelta.X * 0.003f;
			_rotations.Y += Platform.Instance.Input.PointerDelta.Y * 0.003f;
			_distanceFromObject = 1000 + Mouse.GetState().ScrollWheelValue;
		}

		private void UpdateParent()
		{
			CameraAttachable camera = Data[1] as CameraAttachable;

			Vector3 objective = GetObjectivePosition();

			Vector3 newPosition = objective + new Vector3(
				                                  ( float ) ( Math.Cos( _rotations.X ) * Math.Cos( _rotations.Y ) * _distanceFromObject ),
				                                  ( float ) ( Math.Sin( _rotations.Y ) * _distanceFromObject ),
				                                  ( float ) ( Math.Sin( _rotations.X ) * Math.Cos( _rotations.Y ) * _distanceFromObject ) );

			//Quaternion newRotation = Quaternion.CreateFromYawPitchRoll( _rotations.Y, _rotations.X, 0 );

			camera.Parent.SetPosition( newPosition );
			camera.LookingDirection = Vector3.Normalize( - newPosition + objective );
		}


		public List<object> Data { get; set; }

		public void Clean()
		{
		}
	}
}
