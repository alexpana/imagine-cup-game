#if USE_KINECT
using System;
using System.Collections.Generic;
using System.Diagnostics;
using Microsoft.Kinect;
using Microsoft.Kinect.Toolkit.Interaction;
using Microsoft.Xna.Framework;
using UnifiedInputSystem.Input;
using MathHelper = UnifiedInputSystem.Utils.MathHelper;

namespace UnifiedInputSystem.Kinect
{
	public class KinectInputStream : IInputStream<Dictionary<int, KinectPayload>>, IInteractionClient
	{
		private Dictionary<int, KinectPayload> _payload = new Dictionary<int, KinectPayload>();
		private readonly KinectSensor _sensor;

		private readonly int _interactionRegionWidth;
		private readonly int _interactionRegionHeight;

		private Skeleton[] _skeletons;
		private UserInfo[] _userInfos;
		private InteractionStream _interactionStream;
		private bool _needsInitialization;

		public KinectInputStream( KinectSensor kinectSensor, int interactionRegionWidth, int interactionRegionHeight )
		{
			_sensor = kinectSensor;
			_interactionRegionWidth = interactionRegionWidth;
			_interactionRegionHeight = interactionRegionHeight;

			_needsInitialization = true;
		}

		public void Update( Time time )
		{
			if ( _needsInitialization )
			{
				if ( _sensor.Status == KinectStatus.Connected )
				{
					InitializeInteractions( _sensor );

					_needsInitialization = false;
				}
			}
		}

		public Dictionary<int, KinectPayload> GetState()
		{
			return _payload;
		}

		//TODO: cleanup!
		private void InitializeInteractions( KinectSensor sensor )
		{
			_skeletons = new Skeleton[sensor.SkeletonStream.FrameSkeletonArrayLength];
			_userInfos = new UserInfo[InteractionFrame.UserInfoArrayLength];

			sensor.DepthFrameReady += SensorOnDepthFrameReady;
			sensor.SkeletonFrameReady += SensorOnSkeletonFrameReady;

			_interactionStream = new InteractionStream( sensor, this );
			_interactionStream.InteractionFrameReady += InteractionStreamOnInteractionFrameReady;
		}

		private void SensorOnSkeletonFrameReady( object sender, SkeletonFrameReadyEventArgs skeletonFrameReadyEventArgs )
		{
			if ( _sensor != sender )
			{
				return;
			}

			using ( var skeletonFrame = skeletonFrameReadyEventArgs.OpenSkeletonFrame() )
			{
				if ( skeletonFrame != null )
				{
					try
					{
						skeletonFrame.CopySkeletonDataTo( _skeletons );
						var accelerometerReading = _sensor.AccelerometerGetCurrentReading();

						_interactionStream.ProcessSkeleton( _skeletons, accelerometerReading, skeletonFrame.Timestamp );
					}
					catch ( Exception e )
					{
						Debug.WriteLine( e.Message );
					}
				}
			}
		}

		private void SensorOnDepthFrameReady( object sender, DepthImageFrameReadyEventArgs depthImageFrameReadyEventArgs )
		{
			if ( _sensor != sender )
			{
				return;
			}

			using ( var depthFrame = depthImageFrameReadyEventArgs.OpenDepthImageFrame() )
			{
				if ( depthFrame != null )
				{
					try
					{
						_interactionStream.ProcessDepth( depthFrame.GetRawPixelData(), depthFrame.Timestamp );
					}
					catch ( Exception e )
					{
						Debug.WriteLine( e.Message );
					}
				}
			}
		}

		private void InteractionStreamOnInteractionFrameReady( object sender, InteractionFrameReadyEventArgs interactionFrameReadyEventArgs )
		{
			if ( _userInfos == null )
			{
				return;
			}

			using ( var interactionFrame = interactionFrameReadyEventArgs.OpenInteractionFrame() )
			{
				if ( interactionFrame != null )
				{
					interactionFrame.CopyInteractionDataTo( _userInfos );
				}
			}

			_payload = CreatePayloadFromUserInfo( _userInfos );
		}

		private Dictionary<int, KinectPayload> CreatePayloadFromUserInfo( UserInfo[] userInfos )
		{
			Dictionary<int, KinectPayload> payload = new Dictionary<int, KinectPayload>();

			foreach ( var userInfo in userInfos )
			{
				int id = userInfo.SkeletonTrackingId;
				if ( id != 0 )
				{
					KinectPayload userPayload;
					payload.TryGetValue( id, out userPayload );

					foreach ( var pointer in userInfo.HandPointers )
					{
						if ( !pointer.IsActive || pointer.HandType == InteractionHandType.None )
						{
							continue;
						}

						bool? gripState = null;
						if ( pointer.HandEventType == InteractionHandEventType.GripRelease ) gripState = false;
						else if ( pointer.HandEventType == InteractionHandEventType.Grip ) gripState = true;

						var source = ToGestureSource( pointer.HandType );

						if ( userPayload == null )
						{
							payload[id] = userPayload = new KinectPayload();
						}

						userPayload.HandPayloads.Add( source, new KinectHandPayload
						{
							Position = new Vector2(
								( float ) MathHelper.Clamp( pointer.X * _interactionRegionWidth, 0, _interactionRegionWidth ),
								( float ) MathHelper.Clamp( pointer.Y * _interactionRegionHeight, 0, _interactionRegionHeight ) ),
							GripState = gripState,
							PressExtent = pointer.PressExtent,
							Source = source
						} );
					}
				}
			}

			return payload;
		}

		private GestureSource ToGestureSource( InteractionHandType handType )
		{
			if ( handType == InteractionHandType.Left ) return GestureSource.Left;
			if ( handType == InteractionHandType.Left ) return GestureSource.Left;

			return GestureSource.None;
		}

		public InteractionInfo GetInteractionInfoAtLocation( int skeletonTrackingId, InteractionHandType handType,
			double x, double y )
		{
			return new InteractionInfo
			{
				IsGripTarget = false,
				//IsPressTarget = true,
				//PressAttractionPointX = MathHelper.Clamp( x * _interactionRegionWidth, 0, _interactionRegionWidth ),
				//PressAttractionPointY = MathHelper.Clamp( y * _interactionRegionHeight, 0, _interactionRegionHeight )
			};
		}
	}
}
#endif