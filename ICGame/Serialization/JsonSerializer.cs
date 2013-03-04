using System;
using System.Collections.Generic;
using System.IO;
using System.Runtime.Serialization.Json;
using VertexArmy.Entities;
using VertexArmy.Entities.Physics;

namespace VertexArmy.Serialization
{
	public class JsonSerializer<T> : ISerializer<T>
	{
		private readonly DataContractJsonSerializer _serializer;

		public JsonSerializer()
		{
			_serializer = new DataContractJsonSerializer( typeof( T ),
				new List<Type>
				{
					typeof(BlockEntity),
					
					typeof(PhysicsEntityRobot),
					typeof(PhysicsEntityBasic),
				} );
		}

		public void WriteObject( T obj, Stream stream )
		{
			_serializer.WriteObject( stream, obj );
		}

		public T ReadObject( Stream stream )
		{
			return ( T ) _serializer.ReadObject( stream );
		}
	}
}
