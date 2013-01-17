using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Input;

namespace VertexArmy.Input
{
	/// <summary>
	/// For PC:
	/// - Pointer = Mouse
	/// </summary>
	internal class PCInputSystem : IInputSystem
	{
		public bool IsLeftPointerPressed { get; private set; }
		public Vector2 PointerPosition { get; private set; }
		public Vector2 PointerDelta { get; private set; }

		private MouseState _previousMouseState;
		private MouseState _currentMouseState;

		public void Update( GameTime gameTime )
		{
			UpdateMouse();
		}

		private void UpdateMouse()
		{
			_previousMouseState = _currentMouseState;
			_currentMouseState = Mouse.GetState();

			IsLeftPointerPressed = ( _currentMouseState.LeftButton == ButtonState.Pressed );

			Vector2 currentPosition = new Vector2( _currentMouseState.X, _currentMouseState.Y );
			PointerDelta = currentPosition - PointerPosition;
			PointerPosition = currentPosition;
		}
	}
}
