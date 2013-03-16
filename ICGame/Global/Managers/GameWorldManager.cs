﻿using System.Collections.Generic;
using Microsoft.Xna.Framework;
using VertexArmy.GameWorld;
using VertexArmy.GameWorld.Prefabs;

namespace VertexArmy.Global.Managers
{
	public class GameWorldManager
	{
		private Dictionary<string, GameEntity> _entities;

		public GameEntity GetEntity( string name )
		{
			return _entities[name];
		}

		public void SpawnEntity( string prefabName, Vector3 position, string entityName )
		{
			SpawnEntity( PrefabRepository.Instance.GetPrefab( prefabName ), position, entityName );
		}

		public void SpawnEntity( PrefabEntity prefab, Vector3 position, string entityName )
		{
			GameEntity entity = prefab.CreateGameEntity( this );
			_entities.Add( entityName, entity );
			entity.SetPosition( position );
		}

		public void RemoveEntity( string name )
		{
			if ( _entities.ContainsKey( name ) )
			{
				_entities[name].Remove( );
				_entities.Remove( name );
			}
		}

		public static GameWorldManager Instance
		{
			get { return GameWorldManagerInstanceHolder.Instance; }
		}


		public GameWorldManager()
		{
			_entities = new Dictionary<string, GameEntity>( );
		}

		private static class GameWorldManagerInstanceHolder
		{
			// ReSharper disable MemberHidesStaticFromOuterClass
			public static readonly GameWorldManager Instance = new GameWorldManager( );
			// ReSharper restore MemberHidesStaticFromOuterClass
		}
	}
}