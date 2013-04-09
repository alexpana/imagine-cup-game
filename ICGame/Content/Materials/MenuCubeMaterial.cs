using System.Collections.Generic;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using VertexArmy.Global;
using VertexArmy.Graphics;

namespace VertexArmy.Content.Materials
{
	class MenuCubeMaterial
	{
		public const string MenuImageName = "MenuImageName";

		public static Material CreateMaterial( IDictionary<string, object> args )
		{
			args = args ?? new Dictionary<string, object>();
			//string menuImage = args[MenuImageName].ToString();

			Material mat = new Material();
			Effect robofx = Platform.Instance.Content.Load<Effect>( "effects/" + "snmap_menu" );

			mat.Effect = robofx;

			mat.AddParameter( "ColorMap", Platform.Instance.Content.Load<Texture2D>( "images/menu/" + "menu_cube_DIFF" ) );
			mat.AddParameter( "TextMap", Platform.Instance.Content.Load<Texture2D>( "images/menu/" + "menu_cube_text" ) );
			mat.AddParameter( "NormalMap", Platform.Instance.Content.Load<Texture2D>( "images/menu/" + "menu_cube_NORM_NRM" ) );
			mat.AddParameter( "SpecularMap", Platform.Instance.Content.Load<Texture2D>( "images/menu/" + "menu_cube_SPEC" ) );
			mat.AddParameter( "AOMap", Platform.Instance.Content.Load<Texture2D>( "images/menu/" + "menu_cube_OCC" ) );
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
