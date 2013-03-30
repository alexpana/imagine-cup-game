
using System.Collections.Generic;

namespace VertexArmy.Utilities
{
	public static class DictionaryExtensions
	{
		public static TValue GetValue<TKey, TValue>( this IDictionary<TKey, TValue> dictionary, TKey key, TValue defaultValue )
		{
			TValue value;
			if ( !dictionary.TryGetValue( key, out value ) )
			{
				return defaultValue;
			}
			return value;
		}
	}
}
