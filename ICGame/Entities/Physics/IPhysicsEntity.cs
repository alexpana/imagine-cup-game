using Microsoft.Xna.Framework;
using VertexArmy.Serialization;

namespace VertexArmy.Entities.Physics
{
	public interface IPhysicsEntity : ISerializableObject
	{
		void SetPosition( Vector2 position );
		Vector2 GetPosition();
		float GetRotation();
		void SetFreeze( bool value );
		bool IsFrozen();
	}
}