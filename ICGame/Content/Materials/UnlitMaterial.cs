using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using VertexArmy.Global;
using VertexArmy.Graphics;

namespace VertexArmy.Content.Materials
{
	class UnlitMaterial
	{
		public static Material CreateMaterial( string colorMap )
		{
			Material robotMat = new Material( );
			Effect robofx = Platform.Instance.Content.Load<Effect>( "effects/" + "robo" );

			robotMat.Effect = robofx;

			robotMat.AddParameter( "ColorMap", Platform.Instance.Content.Load<Texture2D>( "images/" + colorMap ) );
			robotMat.AddParameter( "NormalMap", Platform.Instance.Content.Load<Texture2D>( "images/" + "flat_nrm" ) );
			robotMat.AddParameter( "SpecularMap", Platform.Instance.Content.Load<Texture2D>( "images/" + "no_spec" ) );
			robotMat.AddParameter( "AOMap", Platform.Instance.Content.Load<Texture2D>( "images/" + "white" ) );
			robotMat.AddParameter( "matWorldViewProj", Matrix.Identity );
			robotMat.AddParameter( "matWorldInverseTranspose", Matrix.Identity );
			robotMat.AddParameter( "matWorld", Matrix.Identity );
			robotMat.AddParameter( "eyePosition", Vector3.Zero );
			robotMat.AddParameter( "lightPosition", Vector3.Zero );

			return robotMat;
		}
	}
}
