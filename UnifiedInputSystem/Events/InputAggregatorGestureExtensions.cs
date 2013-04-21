using System.Collections.Generic;
using System.Linq;

namespace UnifiedInputSystem.Events
{
	public static class InputAggregatorGestureExtensions
	{
		/// <summary>
		/// Gets the first event of the specified type, or null if none exists
		/// </summary>
		/// <returns>An <see cref="IInputEvent"/> or null if none exists</returns>
		public static GestureEvent GetGesture( this InputAggregator inputAggregator,
			GestureType gestureType, GestureSource gestureSource = GestureSource.Left )
		{
			return GetGestures( inputAggregator, gestureType, gestureSource ).FirstOrDefault();
		}

		/// <summary>
		/// Gets all events of the specified type
		/// </summary>
		/// <returns>A list of <see cref="IInputEvent"/>s</returns>
		public static IEnumerable<GestureEvent> GetGestures( this InputAggregator inputAggregator,
			GestureType gestureType, GestureSource gestureSource = GestureSource.Left )
		{
			return inputAggregator.GetEvents<GestureEvent>().Where(
				ev => ( ev.Gesture & gestureType ) != 0 && ev.Source == gestureSource );
		}
	}
}
