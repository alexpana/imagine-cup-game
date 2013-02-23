﻿using System.Collections.Generic;
using System.IO;
using Microsoft.Xna.Framework;
using VertexArmy.Utilities.Serialization;

namespace VertexArmy.Levels
{
	public class LevelManager
	{
		public static LevelManager Instance { get { return LevelManagerInstanceHolder.Instance; } }

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
			try
			{
				using ( Stream stream = TitleContainer.OpenStream( @"Content\Levels\" + name + @".level" ) )
				{
					ISerializer<Level> serializer = SerializerFactory.CreateSerializer<Level>();
					return serializer.ReadObject( stream );
				}
			}
			catch ( FileNotFoundException )
			{
				return null;
			}
		}

		private LevelManager()
		{
			_loadedLevels = new Dictionary<string, Level>();
		}

		#region Singleton
		private static class LevelManagerInstanceHolder
		{
			// ReSharper disable MemberHidesStaticFromOuterClass
			public static readonly LevelManager Instance = new LevelManager();
			// ReSharper restore MemberHidesStaticFromOuterClass
		}
		#endregion
	}
}
