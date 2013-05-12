using System.Collections.Generic;
using UnifiedInputSystem.Events;
using UnifiedInputSystem.Input;
using UnifiedInputSystem.Utils;

namespace UnifiedInputSystem.Keyboard
{
	public class KeyboardProcessor : IInputProcessor
	{
		private readonly KeyboardInputStream _inputStream;
		private List<IInputEvent> _lastEvents = new List<IInputEvent>();

		private BitSet _previousKeys;
		private BitSet _currentKeys;

		public KeyboardProcessor( KeyboardInputStream inputStream )
		{
			_inputStream = inputStream;

			_currentKeys = new BitSet();
			_previousKeys = new BitSet();
		}

		public IEnumerable<IInputEvent> GetEvents()
		{
			return _lastEvents;
		}

		public void Update( Time time )
		{
			_inputStream.Update( time );
			_currentKeys = new BitSet();

			var state = _inputStream.GetState();

			Dictionary<UISButton, bool> buttons = new Dictionary<UISButton, bool>();
			foreach ( var button in state.Buttons )
			{
				_currentKeys[( int ) button] = true;
				buttons[button] = !_previousKeys[( int ) button];
			}

			_lastEvents = new List<IInputEvent> { new ButtonsPressedEvent( buttons ) };
			_previousKeys = _currentKeys;
		}
	}
}
