using System.Collections.Generic;

namespace UnifiedInputSystem.Extensions
{
	public static class DictionaryExtensions
	{
		public static TValue GetValue<TKey, TValue>( this Dictionary<TKey, TValue> dictionary, TKey key,
			TValue defaultValue = default(TValue) )
		{
			TValue value;
			if ( dictionary.TryGetValue( key, out value ) )
			{
				return value;
			}

			return defaultValue;
		}
	}
}
