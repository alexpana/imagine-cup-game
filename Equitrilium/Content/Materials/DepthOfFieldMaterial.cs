﻿using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using VertexArmy.Global;
using VertexArmy.Graphics;

namespace VertexArmy.Content.Materials
{
	internal class DepthOfFieldMaterial
	{
		public static Material CreateMaterial()
		{
			Material mat = new Material();
			Effect effect = Platform.Instance.LoadEffect( "depthoffield" );

			mat.Effect = effect;

			mat.AddParameter( "ColorMap", null );
			mat.AddParameter( "DepthMap", null );
			mat.AddParameter( "matWorldViewProj", Matrix.Identity );
			mat.AddParameter( "distance", 800.0f );
			mat.AddParameter( "no_dof_range", 300.0f );
			mat.AddParameter( "dof_range", 500.0f );
			mat.AddParameter( "near", 1f );
			mat.AddParameter( "far", 10000f );
			mat.AddParameter( "blurDistance", 0.0023f );

			return mat;
		}
	}
}
