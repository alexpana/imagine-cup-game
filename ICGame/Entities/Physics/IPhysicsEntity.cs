using Microsoft.Xna.Framework;
using VertexArmy.Serialization;

namespace VertexArmy.Entities.Physics
{
	public interface IPhysicsEntity : ISerializableObject
	{
		Vector2 Position { get; set; }
		float Rotation { get; set; }
		bool Enabled { get; set; }
		float PhysicsToWorldScale { get; set; }

		void OnUpdate(GameTime dt);
	}
}