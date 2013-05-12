using System.Collections.Generic;
using UnifiedInputSystem.Events;
using UnifiedInputSystem.Input;

namespace UnifiedInputSystem.Mouse
{
	public class MouseInputProcessor : IInputProcessor
	{
		private readonly MouseInputStream _inputStream;
		private List<IInputEvent> _lastEvents;

		private MousePayload _previousPayload;
		private MousePayload _currentPayload;

		public MouseInputProcessor( MouseInputStream inputStream )
		{
			_inputStream = inputStream;
			_lastEvents = new List<IInputEvent>();
		}

		public IEnumerable<IInputEvent> GetEvents()
		{
			return _lastEvents;
		}

		public void Update( Time time )
		{
			_inputStream.Update( time );
			_currentPayload = _inputStream.GetState();

			List<IInputEvent> events = new List<IInputEvent>();

			events.Add( new MovementEvent(
				_currentPayload.Position,
				_currentPayload.Position - _previousPayload.Position ) );

			events.Add( new ScrollEvent( _currentPayload.ScrollValue - _previousPayload.ScrollValue ) );

			if ( _currentPayload.LeftButtonPressed )
			{
				GestureType gesture = _previousPayload.LeftButtonPressed ? GestureType.HoldActivate : GestureType.Activate;

				events.Add( new GestureEvent( gesture, GestureSource.Left, _currentPayload.Position ) );
			}

			if ( _currentPayload.RightButtonPressed )
			{
				GestureType gesture = _previousPayload.RightButtonPressed ? GestureType.HoldActivate : GestureType.Activate;

				events.Add( new GestureEvent( gesture, GestureSource.Right, _currentPayload.Position ) );
			}

			_lastEvents = events;
			_previousPayload = _currentPayload;
		}
	}
}
