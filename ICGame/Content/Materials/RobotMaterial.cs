using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using VertexArmy.Global;
using VertexArmy.Graphics;

namespace VertexArmy.Content.Materials
{
	class RobotMaterial
	{
		public static Material CreateMaterial()
		{
			Material robotMat = new Material( );
			Effect robofx = Platform.Instance.Content.Load<Effect>( "effects/" + "robo" );

			robotMat.Effect = robofx;

			robotMat.AddParameter( "ColorMap", Platform.Instance.Content.Load<Texture2D>( "images/" + "color" ) );
			robotMat.AddParameter( "NormalMap", Platform.Instance.Content.Load<Texture2D>( "images/" + "normal" ) );
			robotMat.AddParameter( "SpecularMap", Platform.Instance.Content.Load<Texture2D>( "images/" + "specular" ) );
			robotMat.AddParameter( "AOMap", Platform.Instance.Content.Load<Texture2D>( "images/" + "ao" ) );
			robotMat.AddParameter( "matWorldViewProj", Matrix.Identity );
			robotMat.AddParameter( "matWorldInverseTranspose", Matrix.Identity );
			robotMat.AddParameter( "matWorld", Matrix.Identity );
			robotMat.AddParameter( "eyePosition", Vector3.Zero );
			robotMat.AddParameter( "lightPosition", Vector3.Zero );

			return robotMat;
		}
	}
}
