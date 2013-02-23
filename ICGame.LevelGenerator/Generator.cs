using System;
using System.Collections.Generic;
using System.IO;
using VertexArmy.Levels;
using VertexArmy.Utilities.Serialization;

namespace ICGame.LevelGenerator
{
	public class Generator
	{
		private readonly string _targetPath;

		public Generator( string targetPath )
		{
			_targetPath = targetPath;
		}

		public void GenerateLevels()
		{
			Console.WriteLine( "Generating levels in " + _targetPath );

			List<Level> levels = new List<Level>
			{
				TutorialGenerator.GenerateTutorial()
			};

			foreach ( var level in levels )
			{
				SerializeLevel( level, _targetPath + level.Name.ToLowerInvariant() + ".level" );
			}
		}

		private void SerializeLevel( Level level, string path )
		{
			using ( StreamWriter streamWriter = new StreamWriter( path ) )
			{
				ISerializer<Level> serializer = SerializerFactory.CreateSerializer<Level>();
				serializer.WriteObject( level, streamWriter.BaseStream );
			}
		}
	}
}
