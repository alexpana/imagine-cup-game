using System.Collections.Generic;
using System.IO;
using Microsoft.Xna.Framework;
using VertexArmy.Content.Prefabs;
using VertexArmy.GameWorld.Prefabs;
using VertexArmy.Serialization;

namespace VertexArmy.Global.Managers
{
	public class PrefabRepository
	{
		private readonly Dictionary<string, PrefabEntity> _prefabs;

		public void RegisterPrefab( string name, PrefabEntity prefab )
		{
			_prefabs.Add( name, prefab );
			prefab.Name = name;
		}

		public ICollection<string> PrefabNames
		{
			get { return _prefabs.Keys; }
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

		public LevelPrefab GetLevelPrefab( string filepath )
		{
			try
			{
				LevelPrefab level;
				using ( Stream stream = TitleContainer.OpenStream( filepath ) )
				{
					ISerializer<LevelPrefab> serializer = SerializerFactory.CreateSerializer<LevelPrefab>();
					level = serializer.ReadObject( stream );
				}

				return level;
			}
			catch ( FileNotFoundException )
			{
				return new LevelPrefab { filepath = filepath };
			}
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
