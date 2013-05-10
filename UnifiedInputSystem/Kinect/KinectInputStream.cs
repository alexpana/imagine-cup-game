#if USE_KINECT
namespace UnifiedInputSystem.Kinect
{
	public class KinectInputStream : IInputStream<KinectPayload>
	{
		private KinectPayload _payload;

		public void Update( Time time )
		{
		}

		public KinectPayload GetState()
		{
			return _payload;
		}
	}
}
#endif