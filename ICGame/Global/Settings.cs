using System;
using System.Collections.Generic;
using System.IO;
using System.Runtime.Serialization.Json;


#if NETFX_CORE
using VertexArmy.Windows8;
using Windows.Storage;
#endif

namespace VertexArmy.Global
{
	public class SettingEventArgs : EventArgs
	{
		public string SettingName { get; set; }
	}

	public class Settings
	{
		public const string IsMusicEnabledSetting = "IsMusicEnabled";

		private Dictionary<string, object> _settings;

		public Settings()
		{
			_settings = new Dictionary<string, object>();

			// default settings
			_settings[IsMusicEnabledSetting] = true;
		}

		public bool IsMusicEnabled
		{
			get { return GetValue( IsMusicEnabledSetting, true ); }
			set { SetValue( IsMusicEnabledSetting, value ); }
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

		public void Save( string fileName = "Settings.txt" )
		{
#if WINDOWS
			using ( var stream = new FileStream( fileName, FileMode.OpenOrCreate ) )
#else
			using ( var stream = StreamExtensions.OpenStreamForWrite( fileName, CreationCollisionOption.ReplaceExisting ) )
#endif
			{
				using ( StreamWriter sw = new StreamWriter( stream ) )
				{
					DataContractJsonSerializer serializer = new DataContractJsonSerializer( _settings.GetType() );
					serializer.WriteObject( sw.BaseStream, _settings );
				}
			}
		}

		public void Load( string fileName = "Settings.txt" )
		{
			if ( !File.Exists( fileName ) )
			{
				Save( fileName );
			}

			try
			{
#if WINDOWS
				using ( var stream = new FileStream( fileName, FileMode.OpenOrCreate ) )
#else
				using ( var stream = StreamExtensions.OpenStreamForRead( fileName) )
#endif
				{
					using ( StreamReader sr = new StreamReader( stream ) )
					{
						DataContractJsonSerializer serializer = new DataContractJsonSerializer( _settings.GetType() );
						_settings = ( Dictionary<string, object> ) serializer.ReadObject( sr.BaseStream );
					}
				}
			}
			catch
			{
			}
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
