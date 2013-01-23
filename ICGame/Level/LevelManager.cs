
using System.Collections.Generic;

namespace VertexArmy.Level
{
	public class LevelManager
	{
		public LevelManager Instance { get { return LevelManagerInstanceHolder.Instance; } }

		private readonly Dictionary<string, Level> _loadedLevels;

		public Level GetLevel( string name )
		{
			Level level;

			if ( !_loadedLevels.TryGetValue( name, out level ) )
			{
				level = LoadLevel( name );
				_loadedLevels.Add( name, level );
			}

			return level;
		}

		private Level LoadLevel( string name )
		{
			Level level = new Level { Name = name };
			//TODO: load from Filesystem
			return level;
		}

		private LevelManager()
		{
			_loadedLevels = new Dictionary<string, Level>();
		}

		#region Singleton
		private static class LevelManagerInstanceHolder
		{
			public static readonly LevelManager Instance = new LevelManager();
		}
		#endregion
	}
}
