using System;
using System.Collections.Generic;
using System.IO;
using System.Runtime.Serialization.Json;
using System.Xml.Serialization;
using VertexArmy.Levels;

namespace ICGame.LevelGenerator
{
	public class Generator
	{
		private readonly string _targetPath;
		private readonly string _serializationType;

		public Generator( string targetPath, string serializationType )
		{
			_targetPath = targetPath;
			_serializationType = serializationType;
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
				SerializeLevel( level, level.Name.ToLowerInvariant() + ".level" );
			}
		}

		private void SerializeLevel( Level tutorialLevel, string path )
		{
			using ( StreamWriter streamWriter = new StreamWriter( path ) )
			{
				if ( _serializationType == "xml" )
				{
					XmlSerializer serializer = new XmlSerializer( typeof( Level ) );
					serializer.Serialize( streamWriter, tutorialLevel );
				}
				else
				{
					DataContractJsonSerializer serializer = new DataContractJsonSerializer( typeof( Level ) );
					serializer.WriteObject( streamWriter.BaseStream, tutorialLevel );
				}
			}
		}
	}
}
