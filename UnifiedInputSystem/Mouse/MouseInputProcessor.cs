﻿using System.Collections.Generic;
using UnifiedInputSystem.Events;

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

		public List<IInputEvent> GetEvents()
		{
			return _lastEvents;
		}

		public void Update( Time time )
		{
			_inputStream.Update( time );
			_currentPayload = _inputStream.GetState();

			List<IInputEvent> events = new List<IInputEvent>();

			events.Add( new LocationEvent(
				_currentPayload.Position,
				_currentPayload.Position - _previousPayload.Position ) );

			if ( _currentPayload.LeftButtonPressed )
			{
				GestureType gesture = _previousPayload.LeftButtonPressed ? GestureType.HoldActivate : GestureType.Activate;

				events.Add( new GestureEvent( gesture, _currentPayload.Position ) );
			}

			_lastEvents = events;
			_previousPayload = _currentPayload;
		}
	}
}