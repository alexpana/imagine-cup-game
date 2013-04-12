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
		public bool IsRightPointerPressed { get; private set; }
		public bool IsLeftPointerFirstTimePressed { get; private set; }
		public bool IsRightPointerFirstTimePressed { get; private set; }

		public Vector2 PointerPosition { get; private set; }
		public Vector2 PointerDelta { get; private set; }

		private MouseState _previousMouseState;
		private MouseState _currentMouseState;

		private KeyboardState _previousKeyboardState;
		private KeyboardState _currentKeyboardState;

		public void Update( GameTime gameTime )
		{
			UpdateMouse();
			UpdateKeyboard();
		}

		private void UpdateKeyboard()
		{
			_previousKeyboardState = _currentKeyboardState;
			_currentKeyboardState = Keyboard.GetState();
		}

		private void UpdateMouse()
		{
			_previousMouseState = _currentMouseState;
			_currentMouseState = Mouse.GetState();

			IsLeftPointerPressed = ( _currentMouseState.LeftButton == ButtonState.Pressed );
			IsRightPointerPressed = ( _currentMouseState.RightButton == ButtonState.Pressed );

			IsLeftPointerFirstTimePressed = IsLeftPointerPressed && _previousMouseState.LeftButton == ButtonState.Released;
			IsRightPointerFirstTimePressed = IsRightPointerPressed && _previousMouseState.RightButton == ButtonState.Released;

			Vector2 currentPosition = new Vector2( _currentMouseState.X, _currentMouseState.Y );
			PointerDelta = currentPosition - PointerPosition;
			PointerPosition = currentPosition;
		}

		public bool IsKeyPressed( Keys key, bool continuous = true )
		{
			return continuous
					   ? _currentKeyboardState.IsKeyDown( key )
					   : _currentKeyboardState.IsKeyDown( key ) && _previousKeyboardState.IsKeyUp( key );
		}
	}
}
