using System;
using System.IO;
using Windows.Storage;

namespace VertexArmy.Windows8
{
	internal static class LocalStorageExtensions
	{
		public static Stream OpenStreamForRead( string path )
		{
			var task = ApplicationData.Current.LocalFolder.OpenStreamForReadAsync( path );
			task.Wait();

			return task.Result;
		}

		public static Stream OpenStreamForWrite( string path, CreationCollisionOption option )
		{
			var task = ApplicationData.Current.LocalFolder.OpenStreamForWriteAsync( path, option );
			task.Wait();

			return task.Result;
		}

		public static bool FileExists( string fileName )
		{
			try
			{
				var fileTask = ApplicationData.Current.LocalFolder.GetFileAsync( fileName );
				fileTask.AsTask().Wait();
				return true;
			}
			catch
			{
				return false;
			}
		}
	}
}
