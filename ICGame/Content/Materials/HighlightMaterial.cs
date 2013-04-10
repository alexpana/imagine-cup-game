using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using VertexArmy.Global;
using VertexArmy.Graphics;

namespace VertexArmy.Content.Materials
{
	class HighlightMaterial
	{
		public static Material CreateMaterial()
		{
			Material mat = new Material();
			Effect effect = Platform.Instance.Content.Load<Effect>( "effects/" + "highlight" );
			mat.Effect = effect;
			mat.AddParameter( "matWorldViewProj", Matrix.Identity );
			mat.AddParameter( "fTimeMs", 0.0f);
			return mat;
		}
	}
}
