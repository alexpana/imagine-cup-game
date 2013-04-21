using Microsoft.Xna.Framework;

namespace UnifiedInputSystem.Events
{
	public class LocationEvent : IInputEvent
	{
		public LocationEvent( Vector2 location )
		{
			Location = location;
		}

		public Vector2 Location { get; private set; }
	}
}
