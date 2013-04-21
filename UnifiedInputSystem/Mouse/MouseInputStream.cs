using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Input;
using XnaMouse = Microsoft.Xna.Framework.Input.Mouse;

namespace UnifiedInputSystem.Mouse
{
	public class MouseInputStream : IInputStream<MousePayload>
	{
		private MousePayload _currentPayload;

		public void Update( Time time )
		{
			var state = XnaMouse.GetState();

			_currentPayload = new MousePayload
			{
				Position = new Vector2( state.X, state.Y ),
				LeftButtonPressed = state.LeftButton == ButtonState.Pressed,
				RightButtonPressed = state.RightButton == ButtonState.Pressed,
				ScrollDelta = state.ScrollWheelValue
			};
		}

		public MousePayload GetState()
		{
			return _currentPayload;
		}
	}
}
