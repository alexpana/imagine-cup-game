using System.Collections.Generic;
using FarseerPhysics.Dynamics;
using Microsoft.Xna.Framework;
using VertexArmy.Global.Updaters;
using VertexArmy.Graphics;
using VertexArmy.Utilities;

namespace VertexArmy.GameWorld
{
	public class GameEntity : ITransformable
	{
		public string Name { get; set; }
		public GameEntityFlags Flags { get; set; }

		public PhysicsEntity PhysicsEntity;
		public List<TransformableController> Subcomponents;
		public SceneNode MainNode;
		public Body MainBody;

		public GameEntity()
		{
			PhysicsEntity = new PhysicsEntity( );
			Subcomponents = new List<TransformableController>( );
		}

		public void SetPosition( Vector3 newPos )
		{
			PhysicsEntity.SetPosition( MainBody, UnitsConverter.ToSimUnits( new Vector2( newPos.X, newPos.Y ) ), newPos.Z );
		}

		public void SetRotation( Quaternion newRot )
		{
			PhysicsEntity.SetRotation( MainBody, TransformUtility.GetAngleRollFromQuaternion( newRot ) );
		}

		public void SetRotation( float newRot )
		{
			PhysicsEntity.SetRotation( MainBody, newRot );
		}

		public void SetScale( Vector3 newScale )
		{
			//TODO
		}

		public Vector3 GetPosition()
		{
			return new Vector3( UnitsConverter.ToDisplayUnits( MainBody.Position ), 0f );
		}

		public Quaternion GetRotation()
		{
			return Quaternion.CreateFromAxisAngle( Vector3.UnitZ, MainBody.Rotation );
		}

		public float GetRotationRadians()
		{
			return MainBody.Rotation;
		}

		public Vector3 GetScale()
		{
			//TODO
			return Vector3.Zero;
		}

		public void Remove()
		{
			PhysicsEntity.Remove( );
			foreach ( TransformableController tc in Subcomponents )
			{
				TransformableControllerUpdater.Instance.UnregisterUpdatable( tc );
			}

			SceneManager.Instance.UnregisterSceneTree( MainNode );
		}

	}
}