using System.Collections.Generic;
using FarseerPhysics.Dynamics;
using Microsoft.Xna.Framework;
using VertexArmy.Global.Behaviors;
using VertexArmy.Global.Managers;
using VertexArmy.Graphics;
using VertexArmy.Utilities;

namespace VertexArmy.GameWorld
{
	public class GameEntity : ITransformable
	{
		public string Name { get; set; }
		public GameEntityFlags Flags { get; set; }

		public PhysicsEntity PhysicsEntity;
		public List<IController> Controllers;
		public SceneNode MainNode;
		public Body MainBody;

		public void SetPosition( Vector3 newPos )
		{
			if ( MainBody != null )
			{
				PhysicsEntity.SetPosition( MainBody, UnitsConverter.ToSimUnits( new Vector2( newPos.X, newPos.Y ) ), newPos.Z );
			}
			else
			{
				MainNode.SetPosition( newPos );
			}
		}

		public void SetRotation( Quaternion newRot )
		{
			if ( MainBody != null )
			{
				PhysicsEntity.SetRotation( MainBody, TransformUtility.GetAngleRollFromQuaternion( newRot ) );
			}
			else
			{
				MainNode.SetRotation( newRot );
			}
		}

		public void SetRotation( float newRot )
		{
			if ( MainBody != null )
			{
				PhysicsEntity.SetRotation( MainBody, newRot );
			}
			else
			{
				MainNode.SetRotation( new Quaternion( Vector3.UnitZ, newRot ) );
			}
		}

		public void SetScale( Vector3 newScale )
		{
			/*
			foreach ( IController c in Controllers )
			{
				c.OutputTransformable.SetScale( newScale );
			}
			 */
		}

		public Vector3 GetPosition()
		{
			if ( MainBody != null )
			{
				return new Vector3( UnitsConverter.ToDisplayUnits( MainBody.Position ), 0f );
			}
			else
			{
				return MainNode.GetPosition( );
			}

		}

		public Quaternion GetRotation()
		{
			if ( MainBody != null )
			{
				return Quaternion.CreateFromAxisAngle( Vector3.UnitZ, MainBody.Rotation );
			}
			else
			{
				return MainNode.GetRotation( );
			}

		}

		public float GetRotationRadians()
		{
			if ( MainBody != null )
			{
				return MainBody.Rotation;
			}
			else
			{
				return MainNode.GetRotationRadians( );
			}
		}

		public Vector3 GetScale()
		{
			//TODO
			return Vector3.Zero;
		}

		public void Remove()
		{
			PhysicsEntity.Remove( );
			foreach ( IController tc in Controllers )
			{
				//ControllerRepository.Instance.UnregisterController( tc );
			}

			SceneManager.Instance.UnregisterSceneTree( MainNode );
		}

	}
}