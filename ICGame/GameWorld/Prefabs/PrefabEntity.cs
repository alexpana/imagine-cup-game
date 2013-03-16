using System.Collections.Generic;
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
		public string Main { get; set; }
		public float PhysicsScale;

		private PhysicsPrefab _physicsPrefab;
		private Dictionary<string, SceneNodesPrefab> _sceneNodesPrefab;

		public PrefabEntity()
		{
			_physicsPrefab = new PhysicsPrefab( );

			_physicsPrefab.Bodies = new Dictionary<string, BodyPrefab>( );
			_physicsPrefab.Joints = new Dictionary<string, JointPrefab>( );
			_physicsPrefab.Paths = new Dictionary<string, PathPrefab>( );

			_sceneNodesPrefab = new Dictionary<string, SceneNodesPrefab>( );
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
				Main = body.Name;
			}
		}

		public void RegisterSceneNode( SceneNodesPrefab scn )
		{
			_sceneNodesPrefab.Add( scn.Name, scn );
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
				entity.PhysicsEntity.AddBody( b.Name, b.GetPhysicsBody( ) );
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
					p.GetPhysicsBodies( )
				);
			}
		}

		private void GameEntityCreateSceneNodes( GameEntity entity )
		{
			/* create main node */
			SceneNode mainNode = new SceneNode( );
			mainNode.AddAttachable(
				new SimpleMeshEntity( Platform.Instance.Content.Load<Model>( _sceneNodesPrefab[Main].Mesh ), _sceneNodesPrefab[Main].GetMaterial( ) )
			);

			/* rest of nodes */
			foreach ( SceneNodesPrefab scnp in _sceneNodesPrefab.Values )
			{
				if ( !Main.Equals( scnp.Name ) )
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
			}

			/* finish main node */
			entity.MainSubcomponent = new TransformableController( mainNode, entity.PhysicsEntity.GetBody( Main ) );
			entity.Subcomponents.Add( entity.MainSubcomponent );
			TransformableControllerUpdater.Instance.RegisterUpdatable( entity.MainSubcomponent );

			SceneManager.Instance.RegisterSceneTree( mainNode );
		}

		public static void ImportJsonScript( string file )
		{

		}

	}
}