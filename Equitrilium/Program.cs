using System;
using System.Runtime.InteropServices;

namespace VertexArmy
{
#if WINDOWS || XBOX
	internal static class Program
	{
		[DllImport( "user32.dll", CharSet = CharSet.Auto )]
		public static extern uint MessageBox( IntPtr hWnd, String text, String caption, uint type );

		[STAThread]
		private static void Main()
		{
			AppDomain.CurrentDomain.UnhandledException += CurrentDomainOnUnhandledException;

			MainGame game = new MainGame();
			game.Run();
		}

		private static void CurrentDomainOnUnhandledException( object sender, UnhandledExceptionEventArgs unhandledExceptionEventArgs )
		{
			MessageBox( IntPtr.Zero, unhandledExceptionEventArgs.ExceptionObject == null ? "" : unhandledExceptionEventArgs.ExceptionObject.ToString(), "Exception", 0x00000010 );
		}
	}
#endif
}
