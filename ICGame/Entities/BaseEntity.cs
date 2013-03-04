
using System.Runtime.Serialization;
using Microsoft.Xna.Framework;
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
		[DataMember]
		EntityFlags Flags { get; set; }

		public virtual void OnUpdate( GameTime dt )
		{
			/* override this if needed*/
			BasePhysicsEntity.OnUpdate(dt);
		//	SceneNode.OnUpdate(dt);
		}
	}
}
