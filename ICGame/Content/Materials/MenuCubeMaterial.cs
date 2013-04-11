using System.Collections.Generic;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using VertexArmy.Global;
using VertexArmy.Graphics;

namespace VertexArmy.Content.Materials
{
	public class MenuCubeMaterial
	{
		public const string MenuTextImage = "TextMap";

		public static Material CreateMaterial( IDictionary<string, object> args )
		{
			args = args ?? new Dictionary<string, object>();
			string menuTextImage = args.ContainsKey( MenuTextImage ) ? args[MenuTextImage].ToString() : "menu_cube_no_text";

			Material mat = new Material();
			Effect robofx = Platform.Instance.Content.Load<Effect>( "effects/" + "snmap_menu_flat" );

			mat.Effect = robofx;

			mat.AddParameter( "ColorMap", Platform.Instance.Content.Load<Texture2D>( "images/menu/" + "menu_cube_flat_DIFF" ) );
			mat.AddParameter( MenuTextImage, Platform.Instance.Content.Load<Texture2D>( "images/menu/" + menuTextImage ) );
			mat.AddParameter( "NormalMap", Platform.Instance.Content.Load<Texture2D>( "images/" + "flat_nrm" ) );
			mat.AddParameter( "SpecularMap", Platform.Instance.Content.Load<Texture2D>( "images/" + "no_spec" ) );
			mat.AddParameter( "AOMap", Platform.Instance.Content.Load<Texture2D>( "images/" + "white" ) );
			mat.AddParameter( "matWorldViewProj", Matrix.Identity );
			mat.AddParameter( "matWorldInverseTranspose", Matrix.Identity );
			mat.AddParameter( "matWorld", Matrix.Identity );
			mat.AddParameter( "eyePosition", Vector3.Zero );
			mat.AddParameter( "lightPosition", Vector3.Zero );
			mat.AddParameter( "uvScale", Vector2.One );

			return mat;
		}
	}
}
