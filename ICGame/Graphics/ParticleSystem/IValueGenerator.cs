using Microsoft.Xna.Framework;

namespace VertexArmy.Graphics.ParticleSystem
{
	interface IValueGenerator
	{
		Vector3 GetVector3Value( float index );
		float GetFloatValue( float index );
	}
}
