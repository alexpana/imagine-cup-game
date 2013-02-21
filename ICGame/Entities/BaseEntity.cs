
using System.Runtime.Serialization;
using VertexArmy.Entities.Physics;
using VertexArmy.Graphics;

namespace VertexArmy.Entities
{
	[DataContract]
	public abstract class BaseEntity
	{
		[DataMember]
		public IPhysicsEntity BasePhysicsEntity { get; set; }
		[DataMember]
		public SceneNode SceneNode { get; set; }
	}
}
