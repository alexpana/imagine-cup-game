#if XNA
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Input;
using XnaMouse = Microsoft.Xna.Framework.Input.Mouse;
#endif

namespace UnifiedInputSystem.Mouse
{
	public class MouseInputStream : IInputStream<MousePayload>
	{
		private MousePayload _currentPayload;

		public void Update( Time time )
		{
#if XNA
			var state = XnaMouse.GetState();

			_currentPayload = new MousePayload
			{
				Position = new Vector2( state.X, state.Y ),
				LeftButtonPressed = state.LeftButton == ButtonState.Pressed,
				RightButtonPressed = state.RightButton == ButtonState.Pressed,
				ScrollValue = state.ScrollWheelValue
			};
#else
			throw new System.NotImplementedException("No implementation for this platform!");
#endif
		}

		public MousePayload GetState()
		{
			return _currentPayload;
		}
	}
}
