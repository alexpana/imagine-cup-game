#if USE_KINECT
using System.Collections.Generic;
using UnifiedInputSystem.Events;

namespace UnifiedInputSystem.Kinect
{
	public class KinectInputProcessor : IInputProcessor
	{
		private List<IInputEvent> _lastEvents = new List<IInputEvent>();

		public List<IInputEvent> GetEvents()
		{
			return _lastEvents;
		}

		public void Update( Time time )
		{
		}
	}
}
#endif