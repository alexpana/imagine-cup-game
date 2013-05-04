#if XNA
using System.Collections.Generic;
using UnifiedInputSystem.Events;
using XnaKeyboard = Microsoft.Xna.Framework.Input.Keyboard;
#endif

namespace UnifiedInputSystem.Keyboard
{
	public class KeyboardInputStream : IInputStream<KeyboardPayload>
	{
		private KeyboardPayload _currentPayload;

		public void Update( Time time )
		{
#if XNA
			var state = XnaKeyboard.GetState();

			List<Buttons> buttons = new List<Buttons>();
			foreach ( var key in state.GetPressedKeys() )
			{
				buttons.Add( ( Buttons ) key );
			}

			_currentPayload = new KeyboardPayload
			{
				Buttons = buttons
			};
#else
			throw new System.NotImplementedException("No implementation for this platform!");
#endif
		}

		public KeyboardPayload GetState()
		{
			return _currentPayload;
		}
	}
}
