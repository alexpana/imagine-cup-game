using Microsoft.Xna.Framework;

namespace UnifiedInputSystem.Mouse
{
	public struct MousePayload
	{
		public Vector2 Position { get; set; }
		public int ScrollValue { get; set; }

		public bool RightButtonPressed { get; set; }
		public bool LeftButtonPressed { get; set; }
	}
}
