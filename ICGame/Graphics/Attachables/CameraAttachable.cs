using Microsoft.Xna.Framework;

namespace VertexArmy.Graphics
{
	public class CameraAttachable : Attachable
	{
		private readonly Vector3 _lookingDirection;
		private readonly Vector3 _upVector;
		private readonly float _near, _far, _aspectRatio, _fov;

		public CameraAttachable ()
		{
			_lookingDirection = new Vector3( 0, 0, 1 );
			_upVector = new Vector3( 0, 1, 0 );
			_near = 1;
			_far = 10000;
			_fov = MathHelper.PiOver4;
			_aspectRatio = Global.Platform.Instance.Device.Viewport.AspectRatio;
		}

		public CameraAttachable(Vector3 lookingdir, Vector3 upvector, float near, float far, float fov, float aspectratio)
		{
			_lookingDirection = lookingdir;
			_upVector = upvector;
			_near = near;
			_far = far;
			_fov = fov;
			_aspectRatio = aspectratio;
		}

		public Matrix GetViewMatrix()
		{
			Vector3 position = Vector3.Transform(new Vector3(0,0,0), Parent.GetAbsoluteTransformation());
			return Matrix.CreateLookAt( position, position + _lookingDirection, _upVector );
		}

		public  Matrix GetPerspectiveMatrix()
		{
			return Matrix.CreatePerspectiveFieldOfView(_fov, _aspectRatio, _near, _far);
		}
	}
}