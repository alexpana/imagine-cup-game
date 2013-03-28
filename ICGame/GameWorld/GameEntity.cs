using System.Collections.Generic;
using FarseerPhysics.Dynamics;
using Microsoft.Xna.Framework;
using VertexArmy.Global.Behaviours;
using VertexArmy.Global.Controllers;
using VertexArmy.Global.Controllers.Components;
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
		public Body MainBody;
		public List<BodyController> BodyControllers;
		public List<LineJointController> LineJointControllers;

		public SceneNode MainNode;
		public Dictionary<string, SceneNode> SceneNodes;

		private Dictionary<string, BaseComponent> _componentsByName;
		private Dictionary<ComponentType, List<BaseComponent>> _componentsByType;

		public void init()
		{
			PhysicsEntity = new PhysicsEntity();
			BodyControllers = new List<BodyController>();
			LineJointControllers = new List<LineJointController>();
			SceneNodes = new Dictionary<string, SceneNode>();
			_componentsByName = new Dictionary<string, BaseComponent>();
			_componentsByType = new Dictionary<ComponentType, List<BaseComponent>>();
		}

		public void SetPhysicsEnabled( bool value )
		{
			if ( value && !PhysicsEntity.Enabled )
			{
				Vector3 actualPosition = GetPosition();
				Quaternion actualRotation = GetRotation();

				SetPosition( new Vector3( Vector2.Zero, GetPosition().Z ) );
				SetRotation( 0f );
				PhysicsEntity.Enabled = true;

				GameTime dt = new GameTime();
				foreach ( var c in BodyControllers )
				{
					c.Update( dt );
				}

				SetPosition( actualPosition );
				SetRotation( actualRotation );
			}
			else if ( !value && PhysicsEntity.Enabled )
			{
				Vector3 actualPosition = GetPosition();
				Quaternion actualRotation = GetRotation();

				SetPosition( new Vector3( Vector2.Zero, GetPosition().Z ) );
				SetRotation( 0f );
				PhysicsEntity.Enabled = false;

				GameTime dt = new GameTime();
				foreach ( var c in BodyControllers )
				{
					c.Update( dt );
				}

				SetPosition( actualPosition );
				SetRotation( actualRotation );
			}
		}

		public void RegisterComponent( string name, BaseComponent component )
		{
			if ( !_componentsByName.ContainsKey( name ) )
			{
				_componentsByName.Add( name, component );

				if ( !_componentsByType.ContainsKey( component.Type ) )
				{
					_componentsByType.Add( component.Type, new List<BaseComponent>() );
				}

				_componentsByType[component.Type].Add( component );
				FrameUpdateManager.Instance.Register( component );
				component.Entity = this;
				component.InitEntity();
			}
		}

		public void UnregisterComponent( string name )
		{
			if ( _componentsByName.ContainsKey( name ) )
			{
				BaseComponent c = _componentsByName[name];
				_componentsByType[c.Type].Remove( c );
				_componentsByName.Remove( name );

				c.Clean();
				FrameUpdateManager.Instance.Unregister( c );
			}
		}

		public void SetPosition( Vector3 newPos )
		{
			if ( MainBody != null && PhysicsEntity.Enabled )
			{
				PhysicsEntity.SetPosition( MainBody, UnitsConverter.ToSimUnits( new Vector2( newPos.X, newPos.Y ) ) );
				MainNode.SetPosition( Vector3.UnitZ * newPos.Z );
			}
			else
			{
				MainNode.SetPosition( newPos );
			}
		}

		public void SetRotation( Quaternion newRot )
		{
			if ( MainBody != null && PhysicsEntity.Enabled )
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
			if ( MainBody != null && PhysicsEntity.Enabled )
			{
				PhysicsEntity.SetRotation( MainBody, newRot );
			}
			else
			{
				MainNode.SetRotation( UnitsConverter.To3DRotation( newRot ) );
			}
		}

		public void SetScale( Vector3 newScale )
		{
			foreach ( BodyController c in BodyControllers )
			{
				c.Transformable.SetScale( newScale );
			}
		}

		public Vector3 GetPosition()
		{
			if ( MainBody != null && PhysicsEntity.Enabled )
			{
				return new Vector3( UnitsConverter.ToDisplayUnits( MainBody.Position ), MainNode.GetPosition().Z );
			}

			return MainNode.GetPosition();
		}

		public Quaternion GetRotation()
		{
			if ( MainBody != null && PhysicsEntity.Enabled )
			{
				return UnitsConverter.To3DRotation( MainBody.Rotation );
			}

			return MainNode.GetRotation();


		}

		public float GetRotationRadians()
		{
			if ( MainBody != null && PhysicsEntity.Enabled )
			{
				return MainBody.Rotation;
			}

			return MainNode.GetRotationRadians();
		}

		public Vector3 GetScale()
		{
			//TODO
			return Vector3.Zero;
		}

		public void Remove()
		{
			PhysicsEntity.Remove();
			foreach ( BodyController tc in BodyControllers )
			{
				FrameUpdateManager.Instance.Unregister( tc );
			}

			SceneManager.Instance.UnregisterSceneTree( MainNode );

			foreach ( BaseComponent comp in _componentsByName.Values )
			{
				FrameUpdateManager.Instance.Unregister( comp );
				comp.Clean();
			}
		}

	}
}