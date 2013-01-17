namespace VertexArmy
{
#if WINDOWS || XBOX
	internal static class Program
	{
		private static void Main()
		{
			MainGame game = new MainGame();
			game.Run();
		}
	}
#endif
}
