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
		private Dictionary<string, PrefabEntity> _prefabs;
		private Dictionary<string, LevelPrefab> _levelPrefabs;

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

		public void UnloadLevelPrefab( string filepath )
		{
			if ( _levelPrefabs.ContainsKey( filepath ) )
			{
				_levelPrefabs.Remove( filepath );
			}
		}

		public LevelPrefab GetLevelPrefab( string filepath )
		{
			if ( !_levelPrefabs.ContainsKey( filepath ) )
			{
				if ( File.Exists( filepath ) )
				{
					Stream stream = TitleContainer.OpenStream( filepath );

					ISerializer<LevelPrefab> serializer = SerializerFactory.CreateSerializer<LevelPrefab>();
					LevelPrefab level = serializer.ReadObject( stream );
					stream.Close();
					_levelPrefabs.Add( filepath, level );
				}
				else
				{
					_levelPrefabs.Add( filepath, new LevelPrefab() { filepath = filepath } );
				}
			}

			return _levelPrefabs[filepath];
		}


		public void SaveLevelPrefab( string filepath )
		{
			if ( _levelPrefabs.ContainsKey( filepath ) )
			{
				_levelPrefabs[filepath].SerializeLevel();
			}
		}

		public static PrefabRepository Instance
		{
			get { return PrefabRepositoryInstanceHolder.Instance; }
		}

		public PrefabRepository()
		{
			_prefabs = new Dictionary<string, PrefabEntity>();
			_levelPrefabs = new Dictionary<string, LevelPrefab>();
		}

		private static class PrefabRepositoryInstanceHolder
		{
			// ReSharper disable MemberHidesStaticFromOuterClass
			public static readonly PrefabRepository Instance = new PrefabRepository();
			// ReSharper restore MemberHidesStaticFromOuterClass
		}
	}
}