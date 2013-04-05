using System.Collections.Generic;
using System.Linq;
using FarseerPhysics.Dynamics;
using Microsoft.Xna.Framework;
using VertexArmy.GameWorld;
using VertexArmy.GameWorld.Prefabs;

namespace VertexArmy.Global.Managers
{
	public class GameWorldManager
	{
		private readonly Dictionary<string, GameEntity> _entities;
		private List<EntityState> _savedState;

		public GameEntity GetEntity( string name )
		{
			return _entities[name];
		}

		public void SpawnEntity( string prefabName, string entityName, Vector3 position, float scale = 1f,
			GameEntityParameters parameters = null )
		{
			SpawnEntity( PrefabRepository.Instance.GetPrefab( prefabName ), entityName, position, scale, parameters );
		}

		public void SpawnEntity( string prefab, string entityName, Vector3 position, Quaternion externalRotation, float scale = 1f,
		GameEntityParameters parameters = null )
		{
			SpawnEntity( PrefabRepository.Instance.GetPrefab( prefab ), entityName, position, externalRotation, scale, parameters );
		}

		public void SpawnEntity( PrefabEntity prefab, string entityName, Vector3 position, float scale = 1f,
			GameEntityParameters parameters = null )
		{
			GameEntity entity = prefab.CreateGameEntity( this, scale, parameters );
			entity.Name = entityName;
			_entities.Add( entityName, entity );
			entity.SetPosition( position );
		}


		public void SpawnEntity( PrefabEntity prefab, string entityName, Vector3 position, Quaternion externalRotation, float scale = 1f,
		GameEntityParameters parameters = null )
		{
			GameEntity entity = prefab.CreateGameEntity( this, scale, parameters );
			entity.Name = entityName;
			_entities.Add( entityName, entity );
			entity.SetPosition( position );
			entity.SetExternalRotation( externalRotation );
		}

		public void RemoveEntity( string entityName )
		{
			if ( _entities.ContainsKey( entityName ) )
			{
				_entities[entityName].Remove();
				_entities.Remove( entityName );
			}
		}

		public static GameWorldManager Instance
		{
			get { return GameWorldManagerInstanceHolder.Instance; }
		}


		public GameWorldManager()
		{
			_entities = new Dictionary<string, GameEntity>();
		}

		public void Clear()
		{
			while ( _entities.Count > 0 )
			{
				RemoveEntity( _entities.Keys.First() );
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
				state.Prefab = ent.Prefab;
				state.PhysicsEnabled = ent.PhysicsEntity.Enabled;

				state.BodyPositions = new Dictionary<string, Vector2>();
				state.BodyRotations = new Dictionary<string, float>();
				state.BodyLinearVelocities = new Dictionary<string, Vector2>();
				state.BodyAngularVelocities = new Dictionary<string, float>();
				state.Scale = ent.Scale;

				foreach ( string BodyName in ent.PhysicsEntity.BodyNames )
				{
					Body b = ent.PhysicsEntity.GetBody( BodyName );

					state.BodyPositions.Add( BodyName, b.Position );
					state.BodyRotations.Add( BodyName, b.Rotation );
					state.BodyLinearVelocities.Add( BodyName, b.LinearVelocity );
					state.BodyAngularVelocities.Add( BodyName, b.AngularVelocity );
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

				foreach ( string BodyName in entState.BodyPositions.Keys )
				{
					entity.PhysicsEntity.GetBody( BodyName ).ResetDynamics();
					entity.PhysicsEntity.GetBody( BodyName ).ResetMassData();
					entity.PhysicsEntity.GetBody( BodyName ).Position = entState.BodyPositions[BodyName];
					entity.PhysicsEntity.GetBody( BodyName ).Rotation = entState.BodyRotations[BodyName];
					entity.PhysicsEntity.GetBody( BodyName ).ApplyLinearImpulse( entState.BodyLinearVelocities[BodyName] );
					entity.PhysicsEntity.GetBody( BodyName ).ApplyAngularImpulse( entState.BodyAngularVelocities[BodyName] );
				}

			}
		}

		private static class GameWorldManagerInstanceHolder
		{
			// ReSharper disable MemberHidesStaticFromOuterClass
			public static readonly GameWorldManager Instance = new GameWorldManager();
			// ReSharper restore MemberHidesStaticFromOuterClass
		}
	}

	public class EntityState
	{
		public string Name;
		public Vector3 Position;
		public Quaternion Rotation;
		public Quaternion ExternalRotation;
		public PrefabEntity Prefab;
		public bool PhysicsEnabled;
		public float Scale;

		public Dictionary<string, Vector2> BodyPositions;
		public Dictionary<string, float> BodyRotations;
		public Dictionary<string, Vector2> BodyLinearVelocities;
		public Dictionary<string, float> BodyAngularVelocities;

	}

}