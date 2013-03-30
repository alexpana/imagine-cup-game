using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using VertexArmy.Global;
using VertexArmy.Graphics;

namespace VertexArmy.Content.Materials
{
	class MenuCubeMaterial
	{
		public static Material CreateMaterial()
		{
			Material material = new Material();
			Effect effect = Platform.Instance.Content.Load<Effect>( @"effects\textured" );
			material.Effect = effect;

			material.AddParameter( "ColorMap", Platform.Instance.Content.Load<Texture2D>( "images/menu/" + "mainmenu_cube" ) );
			material.AddParameter( "matWorldViewProj", Matrix.Identity );
			material.AddParameter( "matWorldInverseTranspose", Matrix.Identity );
			material.AddParameter( "matWorld", Matrix.Identity );
			material.AddParameter( "eyePosition", Vector3.Zero );
			material.AddParameter( "lightPosition", Vector3.Zero );

			return material;
		}
	}
}
