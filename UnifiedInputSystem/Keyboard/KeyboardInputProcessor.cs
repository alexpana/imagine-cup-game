using System.Collections.Generic;
using System.Collections.Specialized;
using UnifiedInputSystem.Events;
using UnifiedInputSystem.Input;

namespace UnifiedInputSystem.Keyboard
{
	public class KeyboardProcessor : IInputProcessor
	{
		private readonly KeyboardInputStream _inputStream;
		private List<IInputEvent> _lastEvents;

		private BitVector32 _previousKeys;
		private BitVector32 _currentKeys;

		public KeyboardProcessor( KeyboardInputStream inputStream )
		{
			_inputStream = inputStream;

			_currentKeys = new BitVector32();
			_previousKeys = new BitVector32();
		}

		public List<IInputEvent> GetEvents()
		{
			return _lastEvents;
		}

		public void Update( Time time )
		{
			_inputStream.Update( time );
			_currentKeys = new BitVector32();

			var state = _inputStream.GetState();

			List<KeyValuePair<Button, bool>> buttons = new List<KeyValuePair<Button, bool>>();
			foreach ( var button in state.Buttons )
			{
				_currentKeys[( int ) button] = true;
				buttons.Add( new KeyValuePair<Button, bool>( button, _previousKeys[( int ) button] ) );
			}

			_lastEvents = new List<IInputEvent> { new ButtonsPressedEvent( buttons ) };
			_previousKeys = _currentKeys;
		}
	}
}
