using Microsoft.Xna.Framework;

namespace VertexArmy.Global.Behaviours
{
	public interface ITransformable
	{
		void SetPosition( Vector3 newPos );

		void SetRotation( Quaternion newRot );

		void SetScale( Vector3 newScale );

		Vector3 GetPosition();

		Quaternion GetRotation();

		Vector3 GetScale();

		float GetRotationRadians();
	}
}
