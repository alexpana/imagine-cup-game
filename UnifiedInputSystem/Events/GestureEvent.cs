
using Microsoft.Xna.Framework;

namespace UnifiedInputSystem.Events
{
	/// <summary>
	/// An event that represents a gesture 
	/// </summary>
	public class GestureEvent : IInputEvent
	{
		public GestureEvent( GestureType type, Vector2 position )
		{
			Gesture = type;
			Location = position;
		}

		/// <summary>
		/// The gesture of the event
		/// </summary>
		public GestureType Gesture { get; private set; }

		/// <summary>
		/// The location where the gesture has happened
		/// </summary>
		public Vector2 Location { get; private set; }
	}
}
