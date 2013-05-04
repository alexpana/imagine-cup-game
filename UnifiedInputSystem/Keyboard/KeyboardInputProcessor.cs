using System.Collections.Generic;
using UnifiedInputSystem.Events;
using UnifiedInputSystem.Input;
using UnifiedInputSystem.Utils;

namespace UnifiedInputSystem.Keyboard
{
	public class KeyboardProcessor : IInputProcessor
	{
		private readonly KeyboardInputStream _inputStream;
		private List<IInputEvent> _lastEvents;

		private BitSet _previousKeys;
		private BitSet _currentKeys;

		public KeyboardProcessor( KeyboardInputStream inputStream )
		{
			_inputStream = inputStream;

			_currentKeys = new BitSet();
			_previousKeys = new BitSet();
		}

		public List<IInputEvent> GetEvents()
		{
			return _lastEvents;
		}

		public void Update( Time time )
		{
			_inputStream.Update( time );
			_currentKeys = new BitSet();

			var state = _inputStream.GetState();

			List<KeyValuePair<Button, bool>> buttons = new List<KeyValuePair<Button, bool>>();
			foreach ( var button in state.Buttons )
			{
				_currentKeys[( int ) button] = true;
				buttons.Add( new KeyValuePair<Button, bool>( button, !_previousKeys[( int ) button] ) );
			}

			_lastEvents = new List<IInputEvent> { new ButtonsPressedEvent( buttons ) };
			_previousKeys = _currentKeys;
		}
	}
}
