
namespace UnifiedInputSystem.Utils
{
	/// <summary>
	/// A container that stores bits
	/// </summary>
	public struct BitSet
	{
		private uint _value;

		public bool this[int bit]
		{
			get { return ( _value & bit ) == bit; }
			set
			{
				if ( value )
					_value |= ( uint ) bit;
				else
					_value &= ~( uint ) bit;
			}
		}
	}
}
