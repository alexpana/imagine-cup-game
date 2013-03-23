
using Microsoft.Xna.Framework.Media;
using VertexArmy.Global;

namespace VertexArmy.Audio
{
	public class SoundPlayer
	{
		private readonly Settings _settings;

		public SoundPlayer( Settings settings )
		{
			_settings = settings;
			_settings.SettingChanged += SettingChanged;
		}

		private void SettingChanged( object sender, SettingEventArgs settingEventArgs )
		{
			if ( settingEventArgs.SettingName == Settings.IsMusicEnabled )
			{
				bool musicOn = _settings.GetValue( Settings.IsMusicEnabled, true );
				if ( !musicOn )
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
			if ( !_settings.GetValue( Settings.IsMusicEnabled, true ) )
			{
				return;
			}

			MediaPlayer.Play( song );
		}

		public void StopMusic()
		{
			MediaPlayer.Stop();
		}
	}
}
