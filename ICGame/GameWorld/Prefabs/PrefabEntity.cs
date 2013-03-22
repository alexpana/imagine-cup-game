using System.Collections.Generic;
using FarseerPhysics.Dynamics;
using Microsoft.Xna.Framework.Graphics;
using VertexArmy.Global;
using VertexArmy.Global.Controllers;
using VertexArmy.Global.Managers;
using VertexArmy.Graphics;

namespace VertexArmy.GameWorld.Prefabs
{
	public class PrefabEntity
	{
		public string Name { get; set; }
		public GameEntityFlags Flags { get; set; }
		public string MainBody { get; set; }

		private PhysicsPrefab _physicsPrefab;
		private Dictionary<string, MeshSceneNodePrefab> _sceneNodesPrefab;
		private Dictionary<string, ArrayMeshSceneNodePrefab> _arrayMeshSceneNodesPrefab;

		private Dictionary<string, CameraSceneNodePrefab> _cameraSceneNodesPrefab;

		public PrefabEntity()
		{
			_physicsPrefab = new PhysicsPrefab( );

			_physicsPrefab.Bodies = new Dictionary<string, BodyPrefab>( );
			_physicsPrefab.Joints = new Dictionary<string, JointPrefab>( );
			_physicsPrefab.Paths = new Dictionary<string, PathPrefab>( );

			_sceneNodesPrefab = new Dictionary<string, MeshSceneNodePrefab>( );
			_arrayMeshSceneNodesPrefab = new Dictionary<string, ArrayMeshSceneNodePrefab>( );
			_cameraSceneNodesPrefab = new Dictionary<string, CameraSceneNodePrefab>( );
		}

		public void RegisterCamera( string name, CameraSceneNodePrefab prefab )
		{
			_cameraSceneNodesPrefab[name] = prefab;
		}

		public void RegisterBody( BodyPrefab body )
		{
			_physicsPrefab.Bodies.Add( body.Name, body );
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

		public GameEntity CreateGameEntity( GameWorldManager world, float scale )
		{
			GameEntity obj = new GameEntity( );

			obj.PhysicsEntity = new PhysicsEntity( );
			obj.Controllers = new List<IController>( );

			obj.Name = Name;
			obj.Flags = Flags;

			GameEntityCreatePhysics( obj, scale );
			GameEntityCreateSceneNodes( obj, scale );

			return obj;
		}

		private void GameEntityCreatePhysics( GameEntity entity, float scale )
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
				entity.PhysicsEntity.AddJoint(
					j.Name,
					j.GetPhysicsJoint( entity.PhysicsEntity.GetBody( j.Body1 ), entity.PhysicsEntity.GetBody( j.Body2 ), scale )
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

		private void GameEntityCreateSceneNodes( GameEntity entity, float scale )
		{
			/* create main node */
			SceneNode mainNode = new SceneNode( );

			/* rest of nodes */
			foreach ( MeshSceneNodePrefab scnp in _sceneNodesPrefab.Values )
			{
				SceneNode scn = new SceneNode( );
				scn.AddAttachable(
					new MeshAttachable(
						Platform.Instance.Content.Load<Model>( _sceneNodesPrefab[scnp.Name].Mesh ), _sceneNodesPrefab[scnp.Name].GetMaterial( )
						)
					);
				mainNode.AddChild( scn );
				scn.SetScale( scn.GetScale( ) * scale );

				if ( scnp.Body != null && entity.PhysicsEntity.GetBody( scnp.Body ) != null )
				{

					BodyController controller = new BodyController( scn, entity.PhysicsEntity.GetBody( scnp.Body ) );
					entity.Controllers.Add( controller );
					ControllerManager.Instance.RegisterController( controller );
				}
				else
				{
					if ( scnp.LocalPosition != null )
					{
						scn.SetPosition( scnp.LocalPosition );
					}

					if ( scnp.LocalRotation != null )
					{
						scn.SetRotation( scnp.LocalRotation );
					}
				}
			}


			foreach ( ArrayMeshSceneNodePrefab scnp in _arrayMeshSceneNodesPrefab.Values )
			{
				for ( int i = scnp.StartIndex; i <= scnp.EndIndex; i++ )
				{
					SceneNode scn = new SceneNode( );
					scn.AddAttachable(
						new MeshAttachable(
							Platform.Instance.Content.Load<Model>( _arrayMeshSceneNodesPrefab[scnp.Name].Mesh ), _arrayMeshSceneNodesPrefab[scnp.Name].GetMaterial( )
							)
						);
					mainNode.AddChild( scn );
					scn.SetScale( scn.GetScale( ) * scale );

					if ( scnp.Path != null && entity.PhysicsEntity.GetBodyFromPath( scnp.Path, i ) != null )
					{
						BodyController controller = new BodyController( scn, entity.PhysicsEntity.GetBodyFromPath( scnp.Path, i ) );
						entity.Controllers.Add( controller );

						ControllerManager.Instance.RegisterController( controller );
					}
				}
			}

			foreach ( var cameraSceneNodePrefab in _cameraSceneNodesPrefab )
			{
				SceneNode scn = new SceneNode( );
				scn.AddAttachable(
					new CameraAttachable( cameraSceneNodePrefab.Value.LookingDirection,
						cameraSceneNodePrefab.Value.UpVector,
						cameraSceneNodePrefab.Value.Near,
						cameraSceneNodePrefab.Value.Far,
						cameraSceneNodePrefab.Value.Fov,
						cameraSceneNodePrefab.Value.AspectRatio )
					);
				mainNode.AddChild( scn );
			}

			/* finish main node */
			SceneManager.Instance.RegisterSceneTree( mainNode );
			entity.MainNode = mainNode;
		}

		public static void ImportJsonScript( string file )
		{

		}

	}
}