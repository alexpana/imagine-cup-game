#if USE_KINECT
using System.Collections.Generic;
using Microsoft.Xna.Framework;
using UnifiedInputSystem.Extensions;
using UnifiedInputSystem.Input;

namespace UnifiedInputSystem.Kinect
{
	public class KinectPayload
	{
		public int SkeletonId { get; set; }
		public Dictionary<GestureSource, KinectHandPayload> HandPayloads { get; private set; }

		public KinectPayload()
		{
			HandPayloads = new Dictionary<GestureSource, KinectHandPayload>();
		}

		public KinectHandPayload GetHandPayload( GestureSource source )
		{
			return HandPayloads.GetValue( source );
		}
	}

	public class KinectHandPayload
	{
		public Vector2 Position { get; set; }
		public bool? GripState { get; set; }
		public GestureSource Source { get; set; }

		public double PressExtent { get; set; }
	}
}
#endif