using Windows.Storage;

namespace System.IO
{
	public static class File
	{
		public static bool Exists( string fileName )
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
