
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using VertexArmy.Global;
using VertexArmy.Graphics;

namespace VertexArmy.Content.Materials
{
	class CelShading
	{
		public static Material CreateMaterial()
		{
			Material material = new Material();
			Effect effect = Platform.Instance.Content.Load<Effect>( @"effects\cel" );
			material.Effect = effect;

			material.AddParameter( "matWorldViewProj", Matrix.Identity );
			material.AddParameter( "matWorldViewInverseTranspose", Matrix.Identity );
			material.AddParameter( "ColorMap", Platform.Instance.Content.Load<Texture2D>( @"images\color" ) );
			material.AddParameter( "lightPosition", Vector3.Zero );
			material.AddParameter( "eyePosition", Vector3.Zero );

			return material;
		}
	}
}
