using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using VertexArmy.Global;
using VertexArmy.Graphics;


namespace VertexArmy.Content.Materials
{
	class SafMaterial
	{
		public static Material CreateMaterial()
		{
			Material material = new Material();
			Effect effect = Platform.Instance.LoadEffect( "saf" );
			material.Effect = effect;

			material.State = new BlendState()
			{
				AlphaSourceBlend = Blend.SourceAlpha,
				AlphaDestinationBlend = Blend.InverseSourceAlpha,
				ColorSourceBlend = Blend.SourceAlpha,
				ColorDestinationBlend = Blend.InverseSourceAlpha,
				AlphaBlendFunction = BlendFunction.Add,
			};

			material.Layer = 2;

			material.AddParameter( Material.ColorMap, Platform.Instance.Content.Load<Texture2D>( "images/waves-noalpha" ) );
			material.AddParameter( "AlphaMap", Platform.Instance.Content.Load<Texture2D>( "images/waves" ) );


			material.AddParameter( "matWorldViewProj", Matrix.Identity );
			material.AddParameter( "fTimeMs", 0f );
			material.AddParameter( "fAlpha", 0.10f );
			material.AddParameter( "fVel", new Vector2( 0.00f, 0.00025f ) );

			return material;
		}
	}
}
