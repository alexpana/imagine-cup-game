using System;
using System.Collections.Generic;
using System.IO;
using System.Runtime.Serialization.Json;

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
