using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using VertexArmy.Global;
using VertexArmy.Graphics;

namespace VertexArmy.Content.Materials
{
	internal class BlurMaterial
	{
		public static Material CreateMaterial()
		{
			Material mat = new Material();
			Effect effect = Platform.Instance.LoadEffect( "blur" );
			mat.Effect = effect;
			mat.AddParameter( "matWorldViewProj", Matrix.Identity );
			mat.AddParameter( "blurDistance", 0.001f );
			mat.AddParameter( "ColorMap", null );
			return mat;
		}
	}
}
