using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using VertexArmy.Global;
using VertexArmy.Graphics;

namespace VertexArmy.Content.Materials
{
	class WallMenuMaterial
	{
		public static Material CreateMaterial()
		{
			Material mat = new Material();
			Effect robofx = Platform.Instance.Content.Load<Effect>( "effects/" + "wall" );

			mat.Effect = robofx;

			mat.AddParameter( "ColorMap", null );
			mat.AddParameter( "NormalMap", null );
			mat.AddParameter( "SpecularMap", null );
			mat.AddParameter( "AOMap", null );
			mat.AddParameter( "matWorldViewProj", Matrix.Identity );
			mat.AddParameter( "matWorldInverseTranspose", Matrix.Identity );
			mat.AddParameter( "matWorld", Matrix.Identity );
			mat.AddParameter( "eyePosition", Vector3.Zero );
			mat.AddParameter( "lightPosition", Vector3.Zero );
			mat.AddParameter( "uvScale", Vector2.One / 10 );

			return mat;
		}
	}
}
