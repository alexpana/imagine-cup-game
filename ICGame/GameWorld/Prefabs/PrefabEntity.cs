using System.Collections.Generic;
using FarseerPhysics.Dynamics;
using Microsoft.Xna.Framework.Graphics;
using VertexArmy.Global;
using VertexArmy.Global.Managers;
using VertexArmy.Global.Updaters;
using VertexArmy.Graphics;

namespace VertexArmy.GameWorld.Prefabs
{
	public class PrefabEntity
	{
		public string Name { get; set; }
		public GameEntityFlags Flags { get; set; }
		public string MainBody { get; set; }
		public float PhysicsScale;

		private PhysicsPrefab _physicsPrefab;
		private Dictionary<string, SceneNodesPrefab> _sceneNodesPrefab;
		private Dictionary<string, PathSceneNodesPrefab> _pathSceneNodesPrefab;

		public PrefabEntity()
		{
			_physicsPrefab = new PhysicsPrefab( );

			_physicsPrefab.Bodies = new Dictionary<string, BodyPrefab>( );
			_physicsPrefab.Joints = new Dictionary<string, JointPrefab>( );
			_physicsPrefab.Paths = new Dictionary<string, PathPrefab>( );

			_sceneNodesPrefab = new Dictionary<string, SceneNodesPrefab>( );
			_pathSceneNodesPrefab = new Dictionary<string, PathSceneNodesPrefab>( );
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

		public void RegisterSceneNode( SceneNodesPrefab scn )
		{
			_sceneNodesPrefab.Add( scn.Name, scn );
		}

		public void RegisterPathSceneNode( PathSceneNodesPrefab pscn )
		{
			_pathSceneNodesPrefab.Add( pscn.Name, pscn );
		}

		public GameEntity CreateGameEntity( GameWorldManager world )
		{
			GameEntity obj = new GameEntity( );

			obj.Name = Name;
			obj.Flags = Flags;

			GameEntityCreatePhysics( obj );
			GameEntityCreateSceneNodes( obj );

			return obj;
		}

		private void GameEntityCreatePhysics( GameEntity entity )
		{
			foreach ( BodyPrefab b in _physicsPrefab.Bodies.Values )
			{
				Body body = b.GetPhysicsBody( );
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
					j.GetPhysicsJoint( entity.PhysicsEntity.GetBody( j.Body1 ), entity.PhysicsEntity.GetBody( j.Body2 ) )
				);
			}

			foreach ( PathPrefab p in _physicsPrefab.Paths.Values )
			{
				entity.PhysicsEntity.AddPath(
					p.Name,
					p.GetPathEntity( )
				);
			}
		}

		private void GameEntityCreateSceneNodes( GameEntity entity )
		{
			/* create main node */
			SceneNode mainNode = new SceneNode( );

			/* rest of nodes */
			foreach ( SceneNodesPrefab scnp in _sceneNodesPrefab.Values )
			{
				SceneNode scn = new SceneNode( );
				scn.AddAttachable(
					new SimpleMeshEntity(
						Platform.Instance.Content.Load<Model>( _sceneNodesPrefab[scnp.Name].Mesh ), _sceneNodesPrefab[scnp.Name].GetMaterial( )
					)
				);
				mainNode.AddChild( scn );
				TransformableController controller = new TransformableController( scn, entity.PhysicsEntity.GetBody( scnp.Body ) );
				entity.Subcomponents.Add( controller );

				TransformableControllerUpdater.Instance.RegisterUpdatable( controller );
			}

			foreach ( PathSceneNodesPrefab scnp in _pathSceneNodesPrefab.Values )
			{
				for ( int i = scnp.StartIndex; i <= scnp.EndIndex; i++ )
				{
					SceneNode scn = new SceneNode( );
					scn.AddAttachable(
						new SimpleMeshEntity(
							Platform.Instance.Content.Load<Model>( _pathSceneNodesPrefab[scnp.Name].Mesh ), _pathSceneNodesPrefab[scnp.Name].GetMaterial( )
							)
						);
					mainNode.AddChild( scn );
					TransformableController controller = new TransformableController( scn, entity.PhysicsEntity.GetBodyFromPath( scnp.Path, i ) );
					entity.Subcomponents.Add( controller );

					TransformableControllerUpdater.Instance.RegisterUpdatable( controller );
				}
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