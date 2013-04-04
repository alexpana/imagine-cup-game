using System;
using FarseerPhysics.Dynamics;
using Microsoft.Xna.Framework;
using VertexArmy.Global.Behaviours;

namespace VertexArmy.Utilities
{
	public static class TransformUtility
	{
		public static void RotateBodyAroundPoint( Body body, Vector2 point, float rotation )
		{
			float s = ( float ) Math.Sin( rotation );
			float c = ( float ) Math.Cos( rotation );

			Vector2 translation = body.Position - point;
			Vector2 rotatedTranslation = new Vector2( translation.X * c - translation.Y * s, translation.X * s + translation.Y * c );

			body.Position = rotatedTranslation + point;
			body.Rotation += rotation;

			if ( Math.Abs( body.Rotation ) >= 2 * Math.PI )
			{
				body.Rotation -= Math.Sign( body.Rotation ) * 2f * ( float ) Math.PI;
			}

		}

		public static void RotateTransformableAroundPoint2D( ITransformable trans, Vector2 point, float rotation )
		{
			float s = ( float ) Math.Sin( rotation );
			float c = ( float ) Math.Cos( rotation );

			Vector2 currentPosition = new Vector2( trans.GetPosition().X, trans.GetPosition().Y );

			Vector2 translation = currentPosition - point;
			Vector2 rotatedTranslation = new Vector2( translation.X * c - translation.Y * s, translation.X * s + translation.Y * c );

			trans.SetPosition( new Vector3( rotatedTranslation + point, trans.GetPosition().Z ) );

			float newRotation = GetAngleRollFromQuaternion( trans.GetRotation() ) + rotation;
			if ( Math.Abs( newRotation ) >= 2 * Math.PI )
			{
				newRotation -= Math.Sign( newRotation ) * 2f * ( float ) Math.PI;
			}
			trans.SetRotation( UnitsConverter.To3DRotation( -newRotation ) );
		}

		public static float GetAngleRollFromQuaternion( Quaternion q )
		{
			return ( -1 ) * ( float ) Math.Atan2(
				2 * ( q.X * q.Y + q.Z * q.W ),
				1 - 2 * ( Math.Pow( q.Y, 2 ) + Math.Pow( q.Z, 2 ) )
			);
		}
	}
}