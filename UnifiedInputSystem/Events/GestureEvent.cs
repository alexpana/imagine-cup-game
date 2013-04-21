using Microsoft.Xna.Framework;

namespace UnifiedInputSystem.Events
{
	/// <summary>
	/// An event that represents a gesture 
	/// </summary>
	public class GestureEvent : IInputEvent
	{
		public GestureEvent( GestureType type, GestureSource source, Vector2 position )
		{
			Gesture = type;
			Location = position;
			Source = source;
		}

		/// <summary>
		/// The gesture of the event
		/// </summary>
		public GestureType Gesture { get; private set; }

		/// <summary>
		/// The location where the gesture has happened
		/// </summary>
		public Vector2 Location { get; private set; }

		/// <summary>
		/// The source of the gesture
		/// </summary>
		public GestureSource Source { get; private set; }
	}
}
