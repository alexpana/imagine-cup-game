using System;
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

			GenerateTutorial();
		}

		private void GenerateTutorial()
		{
			Console.WriteLine( "Generating tutorial ..." );
			string tutorialPath = _targetPath + "tutorial.level";

			Level tutorialLevel = new Level
			{
				Name = "Tutorial"
			};

			SerializeLevel( tutorialLevel, tutorialPath );
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
