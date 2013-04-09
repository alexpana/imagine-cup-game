using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using VertexArmy.Global;
using VertexArmy.Graphics;

namespace VertexArmy.Content.Materials
{
	class DepthOfFieldMaterial
	{
		public static Material CreateMaterial()
		{
			Material mat = new Material();
			Effect effect = Platform.Instance.Content.Load<Effect>( "effects/" + "snmap" );

			mat.Effect = effect;

			mat.AddParameter( "ColorMap", Platform.Instance.Content.Load<Texture2D>( "images/" + "tile-d" ) );
			mat.AddParameter( "NormalMap", Platform.Instance.Content.Load<Texture2D>( "images/" + "tile-n" ) );
			mat.AddParameter( "SpecularMap", Platform.Instance.Content.Load<Texture2D>( "images/" + "specular2" ) );
			mat.AddParameter( "AOMap", Platform.Instance.Content.Load<Texture2D>( "images/" + "ao" ) );
			mat.AddParameter( "matWorldViewProj", Matrix.Identity );
			mat.AddParameter( "matWorldInverseTranspose", Matrix.Identity );
			mat.AddParameter( "matWorld", Matrix.Identity );
			mat.AddParameter( "eyePosition", Vector3.Zero );
			mat.AddParameter( "lightPosition", Vector3.Zero );
			mat.AddParameter( "uvScale", new Vector2(0.125f, 0.25f) );

			return mat;
		}
	}
}
