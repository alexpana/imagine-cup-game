using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using VertexArmy.Global;
using VertexArmy.Graphics;

namespace VertexArmy.Content.Materials
{
	internal class HighlightMaterial
	{
		public static Material CreateMaterial()
		{
			Material mat = new Material();
			Effect effect = Platform.Instance.LoadEffect( "highlight" );
			mat.Effect = effect;
			mat.AddParameter( "matWorldViewProj", Matrix.Identity );
			mat.AddParameter( "fTimeMs", 0.0f );
			mat.AddParameter( "f3Color", Vector3.One );
			return mat;
		}
	}
}
