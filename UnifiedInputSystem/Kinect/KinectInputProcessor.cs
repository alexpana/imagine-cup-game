#if USE_KINECT
using System.Collections.Generic;
using UnifiedInputSystem.Events;
using UnifiedInputSystem.Extensions;
using UnifiedInputSystem.Input;

namespace UnifiedInputSystem.Kinect
{
	public class KinectInputProcessor : IInputProcessor
	{
		private List<IInputEvent> _lastEvents = new List<IInputEvent>();
		private Dictionary<int, KinectPayload> _previousPayload =
			new Dictionary<int, KinectPayload>();

		private readonly KinectInputStream _kinectInputStream;

		public KinectInputProcessor( KinectInputStream inputStream )
		{
			_kinectInputStream = inputStream;
		}

		public List<IInputEvent> GetEvents()
		{
			return _lastEvents;
		}

		public void Update( Time time )
		{
			_kinectInputStream.Update( time );

			List<IInputEvent> events = new List<IInputEvent>();
			Dictionary<int, KinectPayload> payload = _kinectInputStream.GetState();

			foreach ( var kinectPayload in payload )
			{
				KinectPayload currentUserPayload = kinectPayload.Value;

				KinectPayload previousUserPayload;
				_previousPayload.TryGetValue( kinectPayload.Key, out previousUserPayload );

				AddEventsForHand( events, GestureSource.Left, currentUserPayload, previousUserPayload );
				AddEventsForHand( events, GestureSource.Left, currentUserPayload, previousUserPayload );
			}

			_lastEvents = events;
			_previousPayload = payload;
		}

		private void AddEventsForHand( List<IInputEvent> events, GestureSource hand,
			KinectPayload currentUserPayload, KinectPayload previousUserPayload )
		{
			KinectHandPayload handPayload = currentUserPayload.HandPayloads.GetValue( hand );
			KinectHandPayload previousHandPayload = null;
			if ( previousUserPayload != null )
			{
				previousHandPayload = previousUserPayload.HandPayloads.GetValue( hand );
			}

			if ( handPayload != null )
			{
				events.Add( new MovementEvent( handPayload.Position,
					previousHandPayload == null
					? handPayload.Position
					: previousHandPayload.Position - handPayload.Position ) );
			}
		}

	}
}
#endif