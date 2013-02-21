
using VertexArmy.Entities.Physics;
using VertexArmy.Graphics;

namespace VertexArmy.Entities
{
	public abstract class BaseEntity
	{
		public IPhysicsEntity PhysicsEntity { get; protected set; }
		public SceneNode SceneNode { get; protected set; }

	}
}
