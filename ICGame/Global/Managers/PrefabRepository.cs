using System.Collections.Generic;
using VertexArmy.GameWorld.Prefabs;

namespace VertexArmy.Global.Managers
{
	public class PrefabRepository
	{
		private Dictionary<string, PrefabEntity> _prefabs;

		public void RegisterPrefab( string name, PrefabEntity prefab )
		{
			_prefabs.Add( name, prefab );
		}

		public void UnregisterPrefab( string name )
		{
			if ( _prefabs.ContainsKey( name ) )
			{
				_prefabs.Remove( name );
			}
		}

		public PrefabEntity GetPrefab( string name )
		{
			return _prefabs[name];
		}

		public static PrefabRepository Instance
		{
			get { return PrefabRepositoryInstanceHolder.Instance; }
		}

		public PrefabRepository()
		{
			_prefabs = new Dictionary<string, PrefabEntity>();
		}

		private static class PrefabRepositoryInstanceHolder
		{
			// ReSharper disable MemberHidesStaticFromOuterClass
			public static readonly PrefabRepository Instance = new PrefabRepository();
			// ReSharper restore MemberHidesStaticFromOuterClass
		}
	}
}