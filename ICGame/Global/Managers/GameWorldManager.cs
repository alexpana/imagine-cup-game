using System.Collections.Generic;
using System.Linq;
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

		public void SpawnEntity( PrefabEntity prefab, string entityName, Vector3 position, float scale = 1f,
			GameEntityParameters parameters = null )
		{
			GameEntity entity = prefab.CreateGameEntity( this, scale, parameters );
			entity.Name = entityName;
			_entities.Add( entityName, entity );
			entity.SetPosition( position );
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
				state.Prefab = ent.Prefab;

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
				if ( _entities.ContainsKey( entState.Name ) )
				{
					_entities[entState.Name].SetPosition( entState.Position );
					_entities[entState.Name].SetRotation( entState.Rotation );
				}
				else
				{

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
	}
}