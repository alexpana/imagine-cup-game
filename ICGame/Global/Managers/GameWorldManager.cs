using System.Collections.Generic;
using VertexArmy.GameWorld;

namespace VertexArmy.Global.Managers
{
	public class GameWorldManager
	{
		private Dictionary<string, GameEntity> _entities;

		/*
		public void SpawnEntity(PrefabEntity prefab, Vector3 position)
		{
		
		}
		 */

		public void RemoveEntity(string name)
		{
			if ( _entities.ContainsKey( name ) )
			{
				_entities[name].Remove();
				_entities.Remove( name );
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

		private static class GameWorldManagerInstanceHolder
		{
			// ReSharper disable MemberHidesStaticFromOuterClass
			public static readonly GameWorldManager Instance = new GameWorldManager( );
			// ReSharper restore MemberHidesStaticFromOuterClass
		}
	}
}