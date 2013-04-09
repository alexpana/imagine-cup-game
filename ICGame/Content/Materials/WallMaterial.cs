using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using VertexArmy.Global;
using VertexArmy.Graphics;

namespace VertexArmy.Content.Materials
{
	class WallMaterial
	{
		public static Material CreateMaterial()
		{
			Material mat = new Material();
			Effect robofx = Platform.Instance.Content.Load<Effect>( "effects/" + "wall" );

			mat.Effect = robofx;

			mat.AddParameter( "ColorMap", Platform.Instance.Content.Load<Texture2D>( "images/" + "background_COLOR" ) );
			mat.AddParameter( "NormalMap", Platform.Instance.Content.Load<Texture2D>( "images/" + "background_NRM" ) );
			mat.AddParameter( "SpecularMap", Platform.Instance.Content.Load<Texture2D>( "images/" + "background_SPEC" ) );
			mat.AddParameter( "AOMap", Platform.Instance.Content.Load<Texture2D>( "images/" + "background_OCC" ) );
			mat.AddParameter( "matWorldViewProj", Matrix.Identity );
			mat.AddParameter( "matWorldInverseTranspose", Matrix.Identity );
			mat.AddParameter( "matWorld", Matrix.Identity );
			mat.AddParameter( "eyePosition", Vector3.Zero );
			mat.AddParameter( "lightPosition", Vector3.Zero );
			mat.AddParameter( "uvScale", Vector2.One / 7 );

			return mat;
		}
	}
}
