using Microsoft.Xna.Framework;

namespace VertexArmy.Graphics.ParticleSystem
{
	internal class LinearValueGenerator : IValueGenerator
	{
		private readonly Range<Vector3> _vec3Range = null;
		private readonly Range<float> _floatRange = null;

		private LinearValueGenerator( Range<Vector3> range )
		{
			_vec3Range = range;
		}

		private float Interpolate( float min, float max, float index )
		{
			return min + ( max - min ) * index;
		}

		public Vector3 GetVector3Value( float index )
		{
			return new Vector3(
				Interpolate( _vec3Range.Start.X, _vec3Range.End.X, index ),
				Interpolate( _vec3Range.Start.Y, _vec3Range.End.Y, index ),
				Interpolate( _vec3Range.Start.Z, _vec3Range.End.Z, index ) );
		}

		public float GetFloatValue( float index )
		{
			return Interpolate( _floatRange.Start, _floatRange.End, index );
		}
	}
}
