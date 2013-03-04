using System;
using System.Collections.Generic;
using Microsoft.Xna.Framework.Graphics;

namespace VertexArmy.Graphics
{
	class Material
	{
		
		public Effect Effect;
		private readonly dynamic _parameter = new List<object>( );
		private readonly Dictionary<String, int> _bindings = new Dictionary<string, int>( );

		public void Apply()
		{
			foreach (var additionalInformation in _bindings)
			{
				Effect.Parameters[additionalInformation.Key].SetValue(_parameter[additionalInformation.Value]);
			}
		}

		public void AddParameter(string name, object data)
		{
			_parameter.Add( data );
		}

		public void SetParameter( string name, object data )
		{
			_parameter[_bindings[name]] = data;
		}
	}
}
