using System;
using System.Collections.Generic;

namespace VertexArmy.Global
{
	public class SettingEventArgs : EventArgs
	{
		public string SettingName { get; set; }
	}

	//TODO: save/load from file
	public class Settings
	{
		public const string IsMusicEnabled = "IsMusicEnabled";

		private readonly Dictionary<string, object> _settings;

		public Settings()
		{
			_settings = new Dictionary<string, object>();

			// default settings
			_settings[IsMusicEnabled] = true;
		}

		public T GetValue<T>( string setting, T defaultValue )
		{
			object value;
			if ( _settings.TryGetValue( setting, out value ) )
			{
				return ( T ) value;
			}

			return defaultValue;
		}

		public void SetValue<T>( string setting, T value )
		{
			_settings[setting] = value;
			OnSettingChanged( setting );
		}

		public EventHandler<SettingEventArgs> SettingChanged { get; set; }

		private void OnSettingChanged( string setting )
		{
			if ( SettingChanged == null )
			{
				return;
			}

			SettingChanged( this, new SettingEventArgs { SettingName = setting } );
		}
	}
}
