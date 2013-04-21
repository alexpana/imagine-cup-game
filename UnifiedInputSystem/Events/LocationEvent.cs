using Microsoft.Xna.Framework;

namespace UnifiedInputSystem.Events
{
	public struct LocationEvent : IInputEvent
	{
		public LocationEvent( Vector2 location, Vector2 delta )
			: this()
		{
			Delta = delta;
			Location = location;
		}

		public Vector2 Location { get; private set; }
		public Vector2 Delta { get; private set; }
	}
}
