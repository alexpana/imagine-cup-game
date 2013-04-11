using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using FarseerPhysics.Dynamics;
using Microsoft.Xna.Framework;
using VertexArmy.GameWorld;
using VertexArmy.GameWorld.Prefabs;
using VertexArmy.Graphics.Attachables;

namespace VertexArmy.Global.Managers
{
	public class GameWorldManager
	{
		private readonly Dictionary<string, GameEntity> _entities;
		private List<EntityState> _savedState;
		private Dictionary<MeshAttachable, GameEntity> _meshes;
		private Dictionary<GameEntity, List<MeshAttachable>> _entitiesMapToMeshes;

		private bool _frozen;

		public GameEntity GetEntity( string name )
		{
			if ( _entities.ContainsKey( name ) )
			{
				return _entities[name];
			}
			return null;
		}

		public void SpawnEntity( string prefabName, string entityName, Vector3 position, float scale = 1f, Category layer = Category.Cat1,
			GameEntityParameters parameters = null )
		{
			SpawnEntity( PrefabRepository.Instance.GetPrefab( prefabName ), entityName, position, Vector3.One * scale, layer, parameters );
		}

		public void SpawnEntity( string prefabName, string entityName, Vector3 position, Quaternion externalRotation, float scale = 1f, Category layer = Category.Cat1, GameEntityParameters parameters = null )
		{
			SpawnEntity( PrefabRepository.Instance.GetPrefab( prefabName ), entityName, position, externalRotation, Vector3.One * scale, layer, parameters );
		}

		public void SpawnEntity( string prefabName, string entityName, Vector3 position, Vector3 scale, Category layer = Category.Cat1,
	GameEntityParameters parameters = null )
		{
			SpawnEntity( PrefabRepository.Instance.GetPrefab( prefabName ), entityName, position, Vector3.One * scale, layer, parameters );
		}

		public void SpawnEntity( string prefabName, string entityName, Vector3 position, Quaternion externalRotation, Vector3 scale, Category layer = Category.Cat1, GameEntityParameters parameters = null )
		{
			SpawnEntity( PrefabRepository.Instance.GetPrefab( prefabName ), entityName, position, externalRotation, Vector3.One * scale, layer, parameters );
		}

		public void SpawnEntity( PrefabEntity prefab, string entityName, Vector3 position, float scale = 1f, Category layer = Category.Cat1,
	GameEntityParameters parameters = null )
		{
			SpawnEntity( prefab, entityName, position, Vector3.One * scale, layer, parameters );
		}

		public void SpawnEntity( PrefabEntity prefab, string entityName, Vector3 position, Quaternion externalRotation, float scale = 1f, Category layer = Category.Cat1, GameEntityParameters parameters = null )
		{
			SpawnEntity( prefab, entityName, position, externalRotation, Vector3.One * scale, layer, parameters );
		}

		public void SpawnEntity( PrefabEntity prefab, string entityName, Vector3 position, Vector3 scale, Category layer = Category.Cat1,
			GameEntityParameters parameters = null )
		{
			GameEntity entity = prefab.CreateGameEntity( this, scale, parameters );
			entity.Name = entityName;
			_entities.Add( entityName, entity );
			entity.SetPosition( position );
			entity.PhysicsEntity.SetCollisionLayer( layer );
		}

		public void SpawnEntity( PrefabEntity prefab, string entityName, Vector3 position, Quaternion externalRotation, Vector3 scale, Category layer = Category.Cat1, GameEntityParameters parameters = null )
		{
			GameEntity entity = prefab.CreateGameEntity( this, scale, parameters );
			entity.Name = entityName;
			_entities.Add( entityName, entity );
			entity.PhysicsEntity.SetCollisionLayer( layer );
			entity.SetPosition( position );
			entity.SetExternalRotation( externalRotation );
		}

		public void RemoveEntity( string entityName )
		{
			if ( _entities.ContainsKey( entityName ) )
			{
				GameEntity ent = _entities[entityName];

				if ( _entitiesMapToMeshes.ContainsKey( ent ) )
				{
					foreach ( MeshAttachable m in _entitiesMapToMeshes[ent] )
					{
						_meshes.Remove( m );
					}

					_entitiesMapToMeshes.Remove( ent );
				}

				_entities[entityName].Remove();
				_entities.Remove( entityName );
			}
		}

		public static GameWorldManager Instance
		{
			get { return GameWorldManagerInstanceHolder.Instance; }
		}

		public void RegisterMesh( MeshAttachable mesh, GameEntity entity )
		{
			_meshes.Add( mesh, entity );

			if ( !_entitiesMapToMeshes.ContainsKey( entity ) )
			{
				_entitiesMapToMeshes.Add( entity, new List<MeshAttachable>() );
			}

			_entitiesMapToMeshes[entity].Add( mesh );
		}

		public ICollection<MeshAttachable> Meshes
		{
			get { return _meshes.Keys; }
		}

		public GameEntity GetEntityByMesh( MeshAttachable mesh )
		{
			if ( _meshes.ContainsKey( mesh ) )
			{
				return _meshes[mesh];
			}

			return null;
		}

		public GameWorldManager()
		{
			_entities = new Dictionary<string, GameEntity>();
			_entitiesMapToMeshes = new Dictionary<GameEntity, List<MeshAttachable>>();
			_meshes = new Dictionary<MeshAttachable, GameEntity>();
			_frozen = false;
		}

		public void Clear()
		{
			while ( _entities.Count > 0 )
			{
				RemoveEntity( _entities.Keys.First() );
			}
		}

		public void Freeze()
		{
			if ( !_frozen )
			{
				foreach ( GameEntity ent in _entities.Values )
				{
					ent.SetPhysicsEnabled( false );
				}
			}
		}

		public void SaveState()
		{
			if ( _savedState == null )
			{
				_savedState = new List<EntityState>();
			}

			_savedState.Clear();

			foreach ( GameEntity ent in _entities.Values )
			{
				EntityState state = new EntityState();
				state.Name = ent.Name;
				state.Position = ent.GetPosition();
				state.Rotation = ent.GetRotation();
				state.ExternalRotation = ent.GetExternalRotation();
				state.Prefab = ent.Prefab.Name;
				state.PhysicsEnabled = ent.PhysicsEntity.Enabled;

				state.CollisionCategory = ent.PhysicsEntity.GetCollisionLayer();

				state.BodyPositions = new Dictionary<string, Vector2>();
				state.BodyRotations = new Dictionary<string, float>();
				state.BodyLinearVelocities = new Dictionary<string, Vector2>();
				state.BodyAngularVelocities = new Dictionary<string, float>();

				state.PathBodyPositions = new Dictionary<string, List<Vector2>>();
				state.PathBodyRotations = new Dictionary<string, List<float>>();
				state.PathBodyLinearVelocities = new Dictionary<string, List<Vector2>>();
				state.PathBodyAngularVelocities = new Dictionary<string, List<float>>();

				state.Scale = ent.Scale;

				foreach ( string BodyName in ent.PhysicsEntity.BodyNames )
				{
					Body b = ent.PhysicsEntity.GetBody( BodyName );

					state.BodyPositions.Add( BodyName, b.Position );
					state.BodyRotations.Add( BodyName, b.Rotation );
					state.BodyLinearVelocities.Add( BodyName, b.LinearVelocity );
					state.BodyAngularVelocities.Add( BodyName, b.AngularVelocity );
				}

				foreach ( string PathName in ent.PhysicsEntity.PathNames )
				{
					List<Vector2> positions = new List<Vector2>();
					List<float> rotations = new List<float>();
					List<Vector2> linearVelocities = new List<Vector2>();
					List<float> angularVelocities = new List<float>();

					for ( int i = 0; i < ent.PhysicsEntity.GetBodyCountFromPath( PathName ); i++ )
					{
						Body b = ent.PhysicsEntity.GetBodyFromPath( PathName, i );
						positions.Add( b.Position );
						rotations.Add( b.Rotation );
						linearVelocities.Add( b.LinearVelocity );
						angularVelocities.Add( b.AngularVelocity );
					}

					state.PathBodyPositions.Add( PathName, positions );
					state.PathBodyRotations.Add( PathName, rotations );
					state.PathBodyLinearVelocities.Add( PathName, linearVelocities );
					state.PathBodyAngularVelocities.Add( PathName, angularVelocities );
				}

				_savedState.Add( state );
			}
		}

		public void LoadLastState()
		{
			if ( _savedState == null )
			{
				return;
			}

			foreach ( EntityState entState in _savedState )
			{
				if ( !_entities.ContainsKey( entState.Name ) )
				{
					this.SpawnEntity( entState.Prefab, entState.Name, entState.Position, entState.Scale );
				}

				GameEntity entity = _entities[entState.Name];
				entity.SetPhysicsEnabled( entState.PhysicsEnabled );

				entity.SetPosition( entState.Position );
				entity.SetRotation( entState.Rotation );
				entity.PhysicsEntity.SetCollisionLayer( entState.CollisionCategory );

				foreach ( string BodyName in entState.BodyPositions.Keys )
				{
					Body b = entity.PhysicsEntity.GetBody( BodyName );
					b.ResetDynamics();
					b.Position = entState.BodyPositions[BodyName];
					b.Rotation = entState.BodyRotations[BodyName];
					b.LinearVelocity = entState.BodyLinearVelocities[BodyName];
					b.AngularVelocity = entState.BodyAngularVelocities[BodyName];
				}

				foreach ( string PathName in entState.PathBodyPositions.Keys )
				{
					var positions = entState.PathBodyPositions[PathName];
					var rotations = entState.PathBodyRotations[PathName];
					var linearVelocities = entState.PathBodyLinearVelocities[PathName];
					var angularVelocities = entState.PathBodyAngularVelocities[PathName];

					for ( int i = 0; i < positions.Count; i++ )
					{
						Body b = entity.PhysicsEntity.GetBodyFromPath( PathName, i );
						b.ResetDynamics();
						b.Position = positions[i];
						b.Rotation = rotations[i];
						b.LinearVelocity = linearVelocities[i];
						b.AngularVelocity = angularVelocities[i];
					}
				}

			}
		}

		public void SetState( List<EntityState> state )
		{
			_savedState = state;
		}

		public List<EntityState> GetState()
		{
			return _savedState;
		}

		private static class GameWorldManagerInstanceHolder
		{
			// ReSharper disable MemberHidesStaticFromOuterClass
			public static readonly GameWorldManager Instance = new GameWorldManager();
			// ReSharper restore MemberHidesStaticFromOuterClass
		}
	}

	[DataContract]
	public class EntityState
	{
		[DataMember]
		public string Name;
		[DataMember]
		public Vector3 Position;
		[DataMember]
		public Quaternion Rotation;
		[DataMember]
		public Quaternion ExternalRotation;
		[DataMember]
		public string Prefab;
		[DataMember]
		public bool PhysicsEnabled;
		[DataMember]
		public Vector3 Scale;
		[DataMember]
		public Category CollisionCategory;

		[DataMember]
		public Dictionary<string, Vector2> BodyPositions;
		[DataMember]
		public Dictionary<string, float> BodyRotations;
		[DataMember]
		public Dictionary<string, Vector2> BodyLinearVelocities;
		[DataMember]
		public Dictionary<string, float> BodyAngularVelocities;

		[DataMember]
		public Dictionary<string, List<Vector2>> PathBodyPositions;
		[DataMember]
		public Dictionary<string, List<float>> PathBodyRotations;
		[DataMember]
		public Dictionary<string, List<Vector2>> PathBodyLinearVelocities;
		[DataMember]
		public Dictionary<string, List<float>> PathBodyAngularVelocities;

	}

}