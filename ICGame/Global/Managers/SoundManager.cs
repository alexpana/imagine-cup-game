
using System;
using System.Collections.Generic;
using FarseerPhysics.Dynamics;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Audio;
using VertexArmy.Utilities;

namespace VertexArmy.Global.Managers
{
	public class SoundManager
	{
		public const float Distance3DAudio = 0.01f;
		private Dictionary<Body, List<string>> _collisionSounds;
		private Dictionary<Body, SoundEffectInstance> _collisionSoundsInstances;

		public static SoundManager Instance
		{
			get { return SoundManagerInstanceHolder.Instance; }
		}

		public void RegisterCollisionSound( Body body, string sound )
		{
			if ( !_collisionSounds.ContainsKey( body ) )
			{
				_collisionSounds[body] = new List<string>();
			}

			_collisionSounds[body].Add( sound );
		}

		public void RegisterCollisionSound( Body body, List<string> sounds )
		{
			if ( !_collisionSounds.ContainsKey( body ) )
			{
				_collisionSounds[body] = new List<string>();
			}

			_collisionSounds[body].AddRange( sounds );
		}

		public void UnregisterCollisionSounds( Body body )
		{
			if ( _collisionSounds.ContainsKey( body ) )
			{
				_collisionSounds.Remove( body );
			}
		}

		public void PlayCollisionFor( Body body )
		{
			if ( _collisionSounds.ContainsKey( body ) )
			{
				if ( _collisionSoundsInstances.ContainsKey( body ) && _collisionSoundsInstances[body].State.Equals( SoundState.Playing ) )
				{
					return;
				}

				int random = new Random().Next( _collisionSounds[body].Count );

				SoundEffect soundToPlay = Platform.Instance.Content.Load<SoundEffect>( _collisionSounds[body][random] );

				if ( _collisionSoundsInstances.ContainsKey( body ) )
				{
					_collisionSoundsInstances.Remove( body );
				}

				var instance = soundToPlay.CreateInstance();

				AudioListener li = SceneManager.Instance.GetCurrentCameraAudioListener();
				Vector3 emitterPosition = new Vector3( UnitsConverter.ToDisplayUnits( body.Position ), 0f );
				emitterPosition = li.Position + ( emitterPosition - li.Position ) * Distance3DAudio;
				AudioEmitter em = new AudioEmitter { Position = emitterPosition };

				instance.Apply3D( li, em );
				instance.IsLooped = false;
				instance.Volume = 1.0f;
				instance.Play();
				_collisionSoundsInstances.Add( body, instance );
			}
		}


		public SoundManager()
		{
			_collisionSounds = new Dictionary<Body, List<string>>();
			_collisionSoundsInstances = new Dictionary<Body, SoundEffectInstance>();
		}

		public void Clear()
		{
			_collisionSounds.Clear();
		}

		private static class SoundManagerInstanceHolder
		{
			// ReSharper disable MemberHidesStaticFromOuterClass
			public static readonly SoundManager Instance = new SoundManager();
			// ReSharper restore MemberHidesStaticFromOuterClass
		}
	}
}