using System.Collections.Generic;
using Microsoft.Xna.Framework;
using VertexArmy.Global.Updaters;
using VertexArmy.Graphics;
using VertexArmy.Utilities;

namespace VertexArmy.GameWorld
{
	public class GameEntity :ITransformable
	{
		public string Name { get; set; }
		public GameEntityFlags Flags { get; set; }

		public PhysicsEntity PhysicsEntity;
		public List<TransformableController> Subcomponents;
		public TransformableController MainSubcomponent;

		public GameEntity()
		{
			PhysicsEntity = new PhysicsEntity();
			Subcomponents = new List<TransformableController>();
		}

		public void SetPosition( Vector3 newPos )
		{
			PhysicsEntity.SetPosition( MainSubcomponent.Body, new Vector2(newPos.X, newPos.Y) );			
		}

		public void SetRotation( Quaternion newRot )
		{
			PhysicsEntity.SetRotation( MainSubcomponent.Body, TransformUtility.GetAngleRollFromQuaternion( newRot ));			
		}

		public void SetScale( Vector3 newScale )
		{
			//TODO
		}

		public Vector3 GetPosition()
		{
			return new Vector3( MainSubcomponent.Body.Position, 0f );
		}

		public Quaternion GetRotation()
		{
			return Quaternion.CreateFromAxisAngle( Vector3.UnitZ, MainSubcomponent.Body.Rotation );
		}

		public Vector3 GetScale()
		{
			//TODO
			return Vector3.Zero;
		}

		public void Remove()
		{
			PhysicsEntity.Remove();
			//TODO SceneManager.Instance.UnregisterNode( MainSubcomponent.Transformable );
		}
	
	}
}