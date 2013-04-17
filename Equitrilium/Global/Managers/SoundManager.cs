using System;
using System.Collections.Generic;
using FarseerPhysics.Dynamics;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Audio;
using Microsoft.Xna.Framework.Media;
using VertexArmy.Utilities;

namespace VertexArmy.Global.Managers
{
	public class SoundManager
	{
		public const float Distance3DAudio = 0.001f;
		private readonly Dictionary<Body, List<string>> _collisionSounds;
		private readonly Dictionary<Body, SoundEffectInstance> _collisionSoundsInstances;

		private readonly Settings _settings;
		private readonly Random _random;

		public SoundManager( Settings settings )
		{
			_collisionSounds = new Dictionary<Body, List<string>>();
			_collisionSoundsInstances = new Dictionary<Body, SoundEffectInstance>();

			_settings = settings;
			_settings.SettingChanged += SettingChanged;
			_random = new Random();

			MediaPlayer.IsRepeating = true;
		}

		public bool IsMusicPlaying
		{
			get { return MediaPlayer.State == MediaState.Playing; }
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
			if ( !_settings.IsMusicEnabled )
			{
				return;
			}

#if NETFX_CORE
	//TODO: Implement this for Windows 8
			return;
#endif

			if ( _collisionSounds.ContainsKey( body ) )
			{
				if ( _collisionSoundsInstances.ContainsKey( body ) && _collisionSoundsInstances[body].State.Equals( SoundState.Playing ) )
				{
					return;
				}

				int random = _random.Next( _collisionSounds[body].Count );

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

		public void Clear()
		{
			_collisionSounds.Clear();
		}

		private void SettingChanged( object sender, SettingEventArgs settingEventArgs )
		{
			if ( settingEventArgs.SettingName == Settings.IsMusicEnabledSetting )
			{
				if ( !_settings.IsMusicEnabled )
				{
					MediaPlayer.Pause();
				}
				else
				{
					if ( MediaPlayer.State == MediaState.Paused )
					{
						MediaPlayer.Resume();
					}
				}
			}
		}

		public void PlayMusic( Song song )
		{
			if ( !_settings.IsMusicEnabled )
			{
				return;
			}
			MediaPlayer.Volume = 0.3f;
			MediaPlayer.Play( song );
		}

		public void StopMusic()
		{
			MediaPlayer.Stop();
		}

		public void PauseMusic()
		{
			MediaPlayer.Pause();
		}

		public void ResumeMusic()
		{
			if ( !_settings.IsMusicEnabled )
			{
				return;
			}

			MediaPlayer.Resume();
		}

		public void PlaySound( SoundEffect sound )
		{
			if ( !_settings.IsMusicEnabled )
			{
				return;
			}

			sound.Play();
		}
	}
}
