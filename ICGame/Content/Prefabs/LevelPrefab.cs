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
		}
	}
}