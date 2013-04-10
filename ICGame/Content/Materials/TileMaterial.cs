using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using VertexArmy.Global;
using VertexArmy.Graphics;

namespace VertexArmy.Content.Materials
{
	class TileMaterial
	{
		public static Material CreateMaterial()
		{
			Material mat = new Material();
			Effect effect = Platform.Instance.Content.Load<Effect>( "effects/" + "wall" );

			mat.Effect = effect;

			mat.AddParameter( "ColorMap", Platform.Instance.Content.Load<Texture2D>( "images/" + "tile-d" ) );
			mat.AddParameter( "NormalMap", Platform.Instance.Content.Load<Texture2D>( "images/" + "tile-n" ) );
			mat.AddParameter( "SpecularMap", Platform.Instance.Content.Load<Texture2D>( "images/" + "floor_SPEC" ) );
			mat.AddParameter( "AOMap", Platform.Instance.Content.Load<Texture2D>( "images/" + "floor_OCC" ) );
			mat.AddParameter( "matWorldViewProj", Matrix.Identity );
			mat.AddParameter( "matWorldInverseTranspose", Matrix.Identity );
			mat.AddParameter( "matWorld", Matrix.Identity );
			mat.AddParameter( "eyePosition", Vector3.Zero );
			mat.AddParameter( "lightPosition", Vector3.Zero );
			mat.AddParameter( "uvScale", new Vector2( 1f, 1f ) );

			return mat;
		}
	}
}
