using Microsoft.Xna.Framework;

namespace VertexArmy.Graphics.Attachables
{
	public class CameraAttachable : Attachable
	{
		private readonly Vector3 _upVector;
		private readonly float _near, _far, _aspectRatio, _fov;
		private BoundingFrustum _frustum;


		private bool _perspectiveDirty;
		private bool _viewDirty;
		private bool _frustDirty;

		private Vector3 _lookingDirection;

		private Matrix _view, _perspective;

		public CameraAttachable()
			: this( new Vector3( 0, 0, 1 ), new Vector3( 0, 1, 0 ), 1, 10000, MathHelper.PiOver4, Global.Platform.Instance.Device.Viewport.AspectRatio )
		{
		}

		public CameraAttachable( Vector3 lookingdir, Vector3 upvector, float near, float far, float fov, float aspectratio )
		{
			LookingDirection = lookingdir;
			_upVector = upvector;
			_near = near;
			_far = far;
			_fov = fov;
			_aspectRatio = aspectratio;
			_viewDirty = true;
			_perspectiveDirty = true;
			_frustDirty = true;
		}

		public BoundingFrustum GetFrustum()
		{
			if ( _frustDirty || _frustum == null)
			{
				_frustum = new BoundingFrustum(GetViewMatrix() * GetPerspectiveMatrix());
				_frustDirty = false;
			}
			return _frustum;
		}

		public Vector3 LookingDirection
		{
			get { return _lookingDirection; }
			set
			{
				_lookingDirection = value;
				_viewDirty = true;
			}
		}

		public Matrix GetViewMatrix()
		{
			if(Parent.PositionChanged || _viewDirty)
			{
				Vector3 position = Parent.GetAbsolutePosition();
				_view = Matrix.CreateLookAt( position, position + LookingDirection, _upVector );
				_viewDirty = false;
				_frustDirty = true;
			}
			return _view;
		}

		public Matrix GetPerspectiveMatrix()
		{
			if ( _perspectiveDirty )
			{
				_perspective = Matrix.CreatePerspectiveFieldOfView(_fov, _aspectRatio, _near, _far);
				_perspectiveDirty = false;
				_frustDirty = true;
			}
			return _perspective;
		}
	}
}