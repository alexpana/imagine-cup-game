
using System.Collections.Generic;
using System.Linq;
using UnifiedInputSystem.Events;
using UnifiedInputSystem.Input;

namespace UnifiedInputSystem.Extensions
{
	public static class InputAggregatorButtonsPressedExtensions
	{
		/// <summary>
		/// Returns true if there is an event that has the specified button pressed, or false if none exists
		/// </summary>
		public static bool HasEvent( this InputAggregator inputAggregator,
			Button button, bool firstTime = false )
		{
			return inputAggregator.GetEvents<ButtonsPressedEvent>().
				FirstOrDefault( ev => ev.PressedButtons.Contains( new KeyValuePair<Button, bool>( button, firstTime ) ) ) != null;
		}
	}
}
