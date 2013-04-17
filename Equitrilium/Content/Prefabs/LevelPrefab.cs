using System.Collections.Generic;
using System.Runtime.Serialization;
using VertexArmy.Global.Managers;
#if WINDOWS
using System.IO;
using VertexArmy.Serialization;
#endif

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
#if NETFX_CORE
			throw new System.NotImplementedException( "Level prefab saving not implemented in Windows 8 yet." );
#else
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
