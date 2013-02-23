using Microsoft.Xna.Framework;

namespace VertexArmy.Entities.Physics
{
	public interface IPhysicsEntity
	{
		void SetPosition( Vector2 position );
		Vector2 GetPosition();
		float GetRotation();
		void SetFreeze( bool value );
		bool IsFrozen();

		//TODO: Refactor this into an ISerializable interface
		void PostDeserializeInit();
	}
}