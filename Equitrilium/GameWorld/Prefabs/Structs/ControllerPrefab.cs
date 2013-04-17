using FarseerPhysics.Dynamics;
using FarseerPhysics.Dynamics.Joints;
using VertexArmy.Global.Behaviours;
using VertexArmy.Global.Controllers;
using VertexArmy.Global.Managers;

namespace VertexArmy.GameWorld.Prefabs.Structs
{
	public class ControllerPrefab
	{
		public ControllerType Type { get; set; }

		public GameEntity FatherEntity;
		public string Body;
		public string Name;
		public string Joint;

		public string Transformable;

		public IController GetController()
		{
			ITransformable transf;
			string[] transformableNames = PrefabUtils.GetEntityAndComponentName( Transformable );
			if ( transformableNames[0] == null )
			{
				transf = FatherEntity.SceneNodes[Transformable];
			}
			else
			{
				transf = GameWorldManager.Instance.GetEntity( transformableNames[0] ).SceneNodes[transformableNames[1]];
			}

			switch ( Type )
			{
				case ControllerType.BodyController:

					Body body;
					GameEntity ent;

					string[] bodyNames = PrefabUtils.GetEntityAndComponentName( Body );
					if ( bodyNames[0] == null )
					{
						body = FatherEntity.PhysicsEntity.GetBody( Body );
						ent = FatherEntity;
					}
					else
					{
						ent = GameWorldManager.Instance.GetEntity( bodyNames[0] );
						body = ent.PhysicsEntity.GetBody( bodyNames[1] );
					}

					BodyController bc = new BodyController( transf, body, ent );
					return bc;

				case ControllerType.LineJointController:

					Joint joint;

					string[] jointNames = PrefabUtils.GetEntityAndComponentName( Joint );
					if ( jointNames[0] == null )
					{
						joint = FatherEntity.PhysicsEntity.GetJoint( Joint );
					}
					else
					{
						joint = GameWorldManager.Instance.GetEntity( jointNames[0] ).PhysicsEntity.GetJoint( jointNames[1] );
					}

					LineJointController ljc = new LineJointController( transf, joint.BodyA, joint.BodyB );
					return ljc;
			}
			return new BodyController( null, null, null );
		}
	}
}
