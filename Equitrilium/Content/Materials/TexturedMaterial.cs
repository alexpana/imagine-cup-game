using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using VertexArmy.Global;
using VertexArmy.Graphics;

namespace VertexArmy.Content.Materials
{
	internal class TexturedMaterial
	{
		public static Material CreateMaterial( string colorMap )
		{
			Material robotMat = new Material();
			Effect robofx = Platform.Instance.LoadEffect( "robo" );

			robotMat.Effect = robofx;

			robotMat.AddParameter( "ColorMap", Platform.Instance.Content.Load<Texture2D>( "images/" + colorMap ) );
			robotMat.AddParameter( "NormalMap", Platform.Instance.Content.Load<Texture2D>( "images/empty_normals" ) );
			robotMat.AddParameter( "SpecularMap", Platform.Instance.Content.Load<Texture2D>( "images/empty_specular" ) );
			robotMat.AddParameter( "AOMap", Platform.Instance.Content.Load<Texture2D>( "images/empty_white" ) );

			robotMat.AddParameter( "matWorldViewProj", Matrix.Identity );
			robotMat.AddParameter( "matWorldInverseTranspose", Matrix.Identity );
			robotMat.AddParameter( "matWorld", Matrix.Identity );
			robotMat.AddParameter( "eyePosition", Vector3.Zero );
			robotMat.AddParameter( "lightPosition", Vector3.Zero );

			return robotMat;
		}
	}
}
