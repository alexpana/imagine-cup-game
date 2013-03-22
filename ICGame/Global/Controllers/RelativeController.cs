using Microsoft.Xna.Framework;
using VertexArmy.Common;

namespace VertexArmy.Global.Controllers
{
	public class RelativeController : IController
	{
		private ITransformable _transformable;
		private ITransformable _transformable2;
		public Vector3 RelativePosition;
		public ITransformable OutputTransformable
		{
			set
			{
				_transformable = value;
				UpdateTransformableRotation( );
				UpdateTransformablePosition( );
			}
			get { return _transformable; }
		}


		private const float RotationError = 0.001f;
		private const float PositionError = 0.001f;

		private Vector3 _lastInputPosition;
		private Quaternion _lastInputRotation;

		public RelativeController( ITransformable outputTransformable, ITransformable inputTransformable )
		{
			_transformable = outputTransformable;
			_transformable2 = inputTransformable;
			RelativePosition = Vector3.Zero;


			UpdateTransformableRotation( );
			UpdateTransformablePosition( );
		}

		public RelativeController( ITransformable outputTransformable, ITransformable inputTransformable, Vector3 relativePosition )
		{
			_transformable = outputTransformable;
			_transformable2 = inputTransformable;
			RelativePosition = relativePosition;


			UpdateTransformableRotation( );
			UpdateTransformablePosition( );
		}

		public ITransformable InputTransformable
		{
			set
			{
				_transformable2 = value;
				UpdateTransformableRotation( );
				UpdateTransformablePosition( );
			}
			get { return _transformable; }
		}

		public void Update( GameTime dt )
		{
			float rotationDelta = ( _transformable2.GetRotation( ) * _lastInputRotation ).Length( );
			float positionDelta = ( _transformable2.GetPosition( ) - _lastInputPosition ).Length( );

			if ( rotationDelta > RotationError )
			{
				UpdateTransformableRotation( );
			}

			if ( positionDelta > PositionError )
			{
				UpdateTransformablePosition( );
			}

		}

		private void UpdateTransformableRotation()
		{
			_lastInputRotation = _transformable2.GetRotation( );
			if ( _transformable != null && _transformable2 != null )
			{
				_transformable.SetRotation( _transformable2.GetRotation( ) );
			}
		}

		private void UpdateTransformablePosition()
		{
			_lastInputPosition = _transformable2.GetPosition( );
			if ( _transformable != null && _transformable2 != null )
			{
				_transformable.SetPosition( _transformable2.GetPosition( ) + RelativePosition );
			}
		}
	}
}
