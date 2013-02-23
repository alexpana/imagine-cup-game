using System;

namespace VertexArmy
{
#if WINDOWS || XBOX
	internal static class Program
	{
		[STAThread]
		private static void Main()
		{
			MainGame game = new MainGame();
			game.Run();
		}
	}
#endif
}
