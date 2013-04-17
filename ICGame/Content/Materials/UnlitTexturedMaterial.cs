using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using VertexArmy.Global;
using VertexArmy.Graphics;

namespace VertexArmy.Content.Materials
{
	internal class UnlitTexturedMaterial
	{
		public static Material CreateMaterial( string colorMap )
		{
			Material material = new Material();
			material.Effect = Platform.Instance.LoadEffect( "unlit_textured" );

			material.AddParameter( "ColorMap", Platform.Instance.Content.Load<Texture2D>( "images/" + colorMap ) );
			material.AddParameter( "matWorldViewProj", Matrix.Identity );

			return material;
		}
	}
}
