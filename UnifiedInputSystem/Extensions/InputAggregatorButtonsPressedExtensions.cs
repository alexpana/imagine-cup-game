
using System.Collections.Generic;
using System.Linq;
using UnifiedInputSystem.Events;

namespace UnifiedInputSystem.Extensions
{
	public static class InputAggregatorButtonsPressedExtensions
	{
		/// <summary>
		/// Gets the first event of the specified type, or null if none exists
		/// </summary>
		/// <returns>An <see cref="IInputEvent"/> or null if none exists</returns>
		public static bool HasEvent( this InputAggregator inputAggregator,
			Buttons button, bool firstTime = false )
		{
			return inputAggregator.GetEvents<ButtonsPressedEvent>().
				FirstOrDefault( ev => ev.PressedButtons.Contains( new KeyValuePair<Buttons, bool>( button, firstTime ) ) ) != null;
		}
	}
}
