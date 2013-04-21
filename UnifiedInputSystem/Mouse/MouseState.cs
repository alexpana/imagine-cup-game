using Microsoft.Xna.Framework;

namespace UnifiedInputSystem.Mouse
{
	public struct MousePayload
	{
		public Vector2 Position { get; set; }
		public int ScrollDelta { get; set; }

		public bool RightButtonPressed { get; set; }
		public bool LeftButtonPressed { get; set; }
	}
}
