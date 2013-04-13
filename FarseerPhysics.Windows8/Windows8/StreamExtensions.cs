
using System.IO;
using Windows.Storage;

namespace FarseerPhysics.Windows8.Windows8
{
	public static class StreamExtensions
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
	}
}
