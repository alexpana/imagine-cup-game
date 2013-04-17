using System.Collections.Generic;
using FarseerPhysics.Dynamics;
using Microsoft.Xna.Framework;
using VertexArmy.GameWorld.Prefabs.Structs;
using VertexArmy.Global.Controllers;
using VertexArmy.Global.Managers;
using VertexArmy.Graphics;
using VertexArmy.Graphics.Attachables;

namespace VertexArmy.GameWorld.Prefabs
{
	public class PrefabEntity
	{
		public string Name { get; set; }
		public GameEntityFlags Flags { get; set; }
		public string MainBody { get; set; }

		private readonly PhysicsPrefab _physicsPrefab;
		private readonly Dictionary<string, MeshSceneNodePrefab> _sceneNodesPrefab;
		private readonly Dictionary<string, ArrayMeshSceneNodePrefab> _arrayMeshSceneNodesPrefab;
		private readonly Dictionary<string, ControllerPrefab> _controllersPrefab;

		private readonly Dictionary<string, CameraSceneNodePrefab> _cameraSceneNodesPrefab;

		public PrefabEntity()
		{
			_physicsPrefab = new PhysicsPrefab();

			_physicsPrefab.Bodies = new Dictionary<string, BodyPrefab>();
			_physicsPrefab.Joints = new Dictionary<string, JointPrefab>();
			_physicsPrefab.Paths = new Dictionary<string, PathPrefab>();

			_sceneNodesPrefab = new Dictionary<string, MeshSceneNodePrefab>();
			_arrayMeshSceneNodesPrefab = new Dictionary<string, ArrayMeshSceneNodePrefab>();
			_cameraSceneNodesPrefab = new Dictionary<string, CameraSceneNodePrefab>();
			_controllersPrefab = new Dictionary<string, ControllerPrefab>();
		}

		public void RegisterCamera( string name, CameraSceneNodePrefab prefab )
		{
			_cameraSceneNodesPrefab[name] = prefab;
		}

		public void RegisterBody( BodyPrefab body )
		{
			_physicsPrefab.Bodies.Add( body.Name, body );
		}

		public void RegisterController( ControllerPrefab controller )
		{
			_controllersPrefab[controller.Name] = controller;
		}

		public void RegisterPath( PathPrefab path )
		{
			_physicsPrefab.Paths.Add( path.Name, path );
		}

		public void RegisterJoint( JointPrefab joint )
		{
			_physicsPrefab.Joints.Add( joint.Name, joint );
		}

		public void RegisterBody( BodyPrefab body, bool main )
		{
			_physicsPrefab.Bodies.Add( body.Name, body );

			if ( main )
			{
				MainBody = body.Name;
			}
		}

		public void RegisterMeshSceneNode( MeshSceneNodePrefab scn )
		{
			_sceneNodesPrefab.Add( scn.Name, scn );
		}

		public void RegisterArrayMeshSceneNode( ArrayMeshSceneNodePrefab pscn )
		{
			_arrayMeshSceneNodesPrefab.Add( pscn.Name, pscn );
		}

		public GameEntity CreateGameEntity( GameWorldManager world, Vector3 scale, GameEntityParameters parameters )
		{
			GameEntity obj = new GameEntity();

			obj.Init();

			obj.Flags = Flags;
			Vector2 scale2D = new Vector2( scale.X, scale.Y );
			GameEntityCreatePhysics( obj, scale2D );
			GameEntityCreateSceneNodes( obj, scale, parameters != null ? parameters.SceneNodeParameters : null );
			GameEntityCreateControllers( obj );

			obj.Prefab = this;
			obj.Scale = scale;

			return obj;
		}

		private void GameEntityCreatePhysics( GameEntity entity, Vector2 scale )
		{
			foreach ( BodyPrefab b in _physicsPrefab.Bodies.Values )
			{
				Body body = b.GetPhysicsBody( scale );
				entity.PhysicsEntity.AddBody( b.Name, body );

				if ( MainBody.Equals( b.Name ) )
				{
					entity.MainBody = body;
				}
			}

			foreach ( JointPrefab j in _physicsPrefab.Joints.Values )
			{
				j.FatherEntity = entity;
				entity.PhysicsEntity.AddJoint(
					j.Name,
					j.GetPhysicsJoint( scale )
					);
			}

			foreach ( PathPrefab p in _physicsPrefab.Paths.Values )
			{
				entity.PhysicsEntity.AddPath(
					p.Name,
					p.GetPathEntity( scale )
					);
			}
		}

		private void GameEntityCreateSceneNodes( GameEntity entity, Vector3 scale, IDictionary<string, object> parameters )
		{
			/* create main node */
			SceneNode mainNode = new SceneNode();

			/* rest of nodes */
			foreach ( MeshSceneNodePrefab scnp in _sceneNodesPrefab.Values )
			{
				SceneNode scn = scnp.GetSceneNode( parameters, entity );
				entity.SceneNodes.Add( scnp.Name, scn );
				mainNode.AddChild( scn );
				scn.SetScale( scn.GetScale() * scale );
			}

			foreach ( ArrayMeshSceneNodePrefab ascnp in _arrayMeshSceneNodesPrefab.Values )
			{
				for ( int i = ascnp.StartIndex; i <= ascnp.EndIndex; i++ )
				{
					SceneNode scn = ascnp.GetSceneNode( parameters, entity );
					mainNode.AddChild( scn );
					scn.SetScale( scale );

					if ( ascnp.Path != null && entity.PhysicsEntity.GetBodyFromPath( ascnp.Path, i ) != null )
					{
						BodyController controller = new BodyController( scn, entity.PhysicsEntity.GetBodyFromPath( ascnp.Path, i ), entity );
						entity.BodyControllers.Add( controller );

						FrameUpdateManager.Instance.Register( controller );
					}
				}
			}

			foreach ( var cameraSceneNodePrefab in _cameraSceneNodesPrefab )
			{
				mainNode.AddAttachable(
					new CameraAttachable( cameraSceneNodePrefab.Value.LookingDirection,
						cameraSceneNodePrefab.Value.UpVector,
						cameraSceneNodePrefab.Value.Near,
						cameraSceneNodePrefab.Value.Far,
						cameraSceneNodePrefab.Value.Fov,
						cameraSceneNodePrefab.Value.AspectRatio )
					);
			}

			/* finish main node */
			SceneManager.Instance.RegisterSceneTree( mainNode );
			entity.MainNode = mainNode;
		}

		private void GameEntityCreateControllers( GameEntity entity )
		{
			foreach ( ControllerPrefab cp in _controllersPrefab.Values )
			{
				cp.FatherEntity = entity;
				switch ( cp.Type )
				{
					case ControllerType.BodyController:
						BodyController bc = cp.GetController() as BodyController;
						entity.BodyControllers.Add( bc );
						FrameUpdateManager.Instance.Register( bc );
						break;

					case ControllerType.LineJointController:
						LineJointController ljc = cp.GetController() as LineJointController;
						entity.LineJointControllers.Add( ljc );
						FrameUpdateManager.Instance.Register( ljc );
						break;
				}
			}
		}

		public static void ImportJsonScript( string file )
		{
		}
	}
}
