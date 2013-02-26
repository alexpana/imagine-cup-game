using System;
using FarseerPhysics.Dynamics;
using Microsoft.Xna.Framework;

namespace VertexArmy.Utilities
{
	public static class BodyUtility
	{
		 public static void RotateBodyAroundPoint( Body body, Vector2 point, float rotation)
		 {
			 float s = (float)Math.Sin( (double)rotation );
			 float c = (float)Math.Cos( (double)rotation );

			 Vector2 translation = body.Position - point;
			 Vector2 rotatedTranslation = new Vector2( translation.X * c - translation.Y * s, translation.X * s + translation.Y * c );

			 body.Position = rotatedTranslation + point;
			 body.Rotation += rotation;

			 if ( Math.Abs( body.Rotation ) >= 2 * Math.PI )
			 {
				 body.Rotation -= ( float ) Math.Sign( body.Rotation ) * 2f * ( float ) Math.PI;
			 }
			 
		 }
	}
}