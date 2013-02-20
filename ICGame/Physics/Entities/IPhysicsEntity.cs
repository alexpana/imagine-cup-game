using Microsoft.Xna.Framework;

namespace VertexArmy.Physics
{
	public interface IPhysicsEntity
	{
		void SetPosition( Vector2 position );
		Vector2 GetPosition();
		void SetFreeze( bool value );
		bool IsFrozen();
	}
}