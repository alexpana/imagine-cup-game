using System;
using System.Collections.Generic;
using Microsoft.Xna.Framework.Graphics;

namespace VertexArmy.Graphics
{
	public class Material
	{
		public const string ColorMap = "ColorMap";

		public Effect Effect;
		private readonly dynamic _parameter = new List<object>();
		private readonly Dictionary<String, int> _bindings = new Dictionary<string, int>();

		public void Apply()
		{
			Effect.CurrentTechnique.Passes[0].Apply();
			foreach ( var additionalInformation in _bindings )
			{
				if ( Effect.Parameters[additionalInformation.Key] != null )
				{
					Effect.Parameters[additionalInformation.Key].SetValue( _parameter[additionalInformation.Value] );
				}
			}
		}

		public void AddParameter( string name, object data )
		{
			_bindings[name] = _parameter.Count;
			_parameter.Add( data );
		}

		public void SetParameter( string name, object data )
		{
			if ( _bindings.ContainsKey( name ) )
				_parameter[_bindings[name]] = data;
		}
	}
}
