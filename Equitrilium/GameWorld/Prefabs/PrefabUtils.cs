namespace VertexArmy.GameWorld.Prefabs
{
	public class PrefabUtils
	{
		public static string[] GetEntityAndComponentName( string value )
		{
			if ( value.Contains( "." ) )
			{
				char[] splitPattern = new char[] { '.' };
				return value.Split( splitPattern, 2 );
			}

			string[] result = new string[2];
			result[0] = null;
			result[1] = value;

			return result;
		}
	}
}
