using Microsoft.Xna.Framework;
using VertexArmy.Common;

namespace VertexArmy.Global.Controllers
{
	public class RelativeController : IController
	{
		private ITransformable _transformable;
		private ITransformable _transformable2;
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
			float rotationDelta = ( _transformable.GetRotation( ) * _lastInputRotation ).Length( );
			float positionDelta = ( _transformable.GetPosition( ) - _lastInputPosition ).Length( );

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
			if ( OutputTransformable != null && InputTransformable != null )
			{
				OutputTransformable.SetRotation( InputTransformable.GetRotation( ) );
			}
		}

		private void UpdateTransformablePosition()
		{
			_lastInputPosition = _transformable2.GetPosition( );
			if ( OutputTransformable != null && InputTransformable != null )
			{
				OutputTransformable.SetPosition( InputTransformable.GetPosition( ) );
			}
		}
	}
}
