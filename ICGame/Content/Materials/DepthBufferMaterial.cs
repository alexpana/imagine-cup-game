using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using VertexArmy.Global;
using VertexArmy.Graphics;

namespace VertexArmy.Content.Materials
{
	internal class DepthBufferMaterial
	{
		public static Material CreateMaterial()
		{
			Material mat = new Material();
			Effect effect = Platform.Instance.LoadEffect( "depth" );
			mat.Effect = effect;
			mat.AddParameter( "matWorldViewProj", Matrix.Identity );
			return mat;
		}
	}
}
