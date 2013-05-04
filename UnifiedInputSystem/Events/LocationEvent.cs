using Microsoft.Xna.Framework;

namespace UnifiedInputSystem.Events
{
	/// <summary>
	/// An event that contains a 2D location and the delta since the previous event.
	/// </summary>
	public class LocationEvent : IInputEvent
	{
		public LocationEvent( Vector2 location, Vector2 delta )
		{
			Delta = delta;
			Location = location;
		}

		/// <summary>
		/// Current location
		/// </summary>
		public Vector2 Location { get; private set; }

		/// <summary>
		/// Delta value since the last <see cref="LocationEvent"/>
		/// </summary>
		public Vector2 Delta { get; private set; }
	}
}
