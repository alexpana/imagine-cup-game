using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using VertexArmy.Global;
using VertexArmy.Graphics;

namespace VertexArmy.Content.Materials
{
	class CrateMaterial
	{
		public static Material CreateMaterial()
		{
			Material robotMat = new Material( );
			Effect robofx = Platform.Instance.Content.Load<Effect>( "effects/" + "snmap" );

			robotMat.Effect = robofx;

			robotMat.AddParameter( "ColorMap", Platform.Instance.Content.Load<Texture2D>( "images/" + "crate_DIFF" ) );
			robotMat.AddParameter( "NormalMap", Platform.Instance.Content.Load<Texture2D>( "images/" + "crate_NRM" ) );
			robotMat.AddParameter( "SpecularMap", Platform.Instance.Content.Load<Texture2D>( "images/" + "no_spec" ) );
			robotMat.AddParameter( "AOMap", Platform.Instance.Content.Load<Texture2D>( "images/" + "white" ) );
			robotMat.AddParameter( "matWorldViewProj", Matrix.Identity );
			robotMat.AddParameter( "matWorldInverseTranspose", Matrix.Identity );
			robotMat.AddParameter( "matWorld", Matrix.Identity );
			robotMat.AddParameter( "eyePosition", Vector3.Zero );
			robotMat.AddParameter( "lightPosition", Vector3.Zero );
			robotMat.AddParameter( "uvscale", new Vector2( 1, 1 ) );

			return robotMat;
		}
	}
}
