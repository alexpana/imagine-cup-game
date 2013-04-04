using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using VertexArmy.Global;
using VertexArmy.Graphics;


namespace VertexArmy.Content.Materials
{
	class SafMaterial
	{
		public static Material CreateMaterial ()
		{
			Material material = new Material();
			Effect effect = Platform.Instance.Content.Load<Effect>( @"effects\saf" );
			material.Effect = effect;

			material.AddParameter(Material.ColorMap, Platform.Instance.Content.Load<Texture2D>("images/waves"));
			
				
			material.AddParameter( "matWorldViewProj", Matrix.Identity );
			material.AddParameter( "fTime", 0f );
			material.AddParameter( "fAlpha", 1f );
			material.AddParameter( "fVel", new Vector2( 0.01f, 0.01f ) );
			
			return material;
		}
	}
}
