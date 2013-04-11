using System.Collections.Generic;
using System.IO;
using System.Runtime.Serialization;
using VertexArmy.Global.Managers;
using VertexArmy.Serialization;

namespace VertexArmy.Content.Prefabs
{
	[DataContract]
	public class LevelPrefab
	{
		[DataMember]
		public string filepath;
		[DataMember]
		public List<EntityState> _savedState;

		public void SerializeLevel()
		{
			using ( StreamWriter streamWriter = new StreamWriter( filepath ) )
			{
				ISerializer<LevelPrefab> serializer = SerializerFactory.CreateSerializer<LevelPrefab>();
				serializer.WriteObject( this, streamWriter.BaseStream );
			}
#if DEBUG
			using ( StreamWriter streamWriter = new StreamWriter( @"..\..\..\" + filepath ) )
			{
				ISerializer<LevelPrefab> serializer = SerializerFactory.CreateSerializer<LevelPrefab>();
				serializer.WriteObject( this, streamWriter.BaseStream );
			}
#endif
		}

		public void SetState( List<EntityState> state )
		{
			var copy = state.ToArray();
			foreach ( var s in copy )
			{
				if ( s.Prefab.Equals( "Camera" ) || s.Prefab.Equals( "Trigger" ) )
				{
					state.Remove( s );
				}
			}
			_savedState = state;
		}
	}
}