
using Microsoft.Xna.Framework;
using VertexArmy.Entities.Physics;
using VertexArmy.Graphics;

namespace VertexArmy.Entities
{
	public class RobotEntity : BaseEntity
	{
		public RobotEntity()
		{
			BasePhysicsEntity = new PhysicsEntityRobot( 1f, new Vector2( 50f, 5f ) );
			SceneNode = new RobotSceneNode();
		}

		public override void OnUpdate( GameTime dt )
		{
			base.OnUpdate(dt);

			Vector3 position = new Vector3(BasePhysicsEntity.Position.X, BasePhysicsEntity.Position.Y, 0.0f);
			Quaternion rotation = new Quaternion(new Vector3(0,0,1), BasePhysicsEntity.Rotation);

			SceneNode.SetPosition(position * BasePhysicsEntity.PhysicsToWorldScale);
			SceneNode.SetRotation(rotation);
		}
	}
}
