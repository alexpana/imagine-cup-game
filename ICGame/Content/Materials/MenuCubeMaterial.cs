using System.Collections.Generic;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using VertexArmy.Global;
using VertexArmy.Graphics;
using VertexArmy.Utilities;

namespace VertexArmy.Content.Materials
{
	class MenuCubeMaterial
	{
		public static Material CreateMaterial( IDictionary<string, object> args )
		{
			Material material = new Material();
			Effect effect = Platform.Instance.Content.Load<Effect>( @"effects\textured" );
			material.Effect = effect;

			var parameters = args ?? new Dictionary<string, object>();

			material.AddParameter( Material.ColorMap, Platform.Instance.Content.Load<Texture2D>( "images/menu/" +
				parameters.GetValue( Material.ColorMap, "mainmenu_cube" ) ) );
			material.AddParameter( "matWorldViewProj", Matrix.Identity );
			material.AddParameter( "matWorldInverseTranspose", Matrix.Identity );
			material.AddParameter( "matWorld", Matrix.Identity );
			material.AddParameter( "eyePosition", Vector3.Zero );
			material.AddParameter( "lightPosition", Vector3.Zero );

			return material;
		}
	}
}
