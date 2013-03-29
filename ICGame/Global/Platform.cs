using FarseerPhysics.Dynamics;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using VertexArmy.Global.Managers;
using VertexArmy.Input;

namespace VertexArmy.Global
{
	public class Platform
	{
		public static Platform Instance
		{
			get { return PlatformInstanceHolder.Instance; }
		}

		public Game Game { get; set; }

		public GraphicsDeviceManager DeviceManager { get; set; }

		public GraphicsDevice Device
		{
			get { return Game.GraphicsDevice; }
		}

		public ContentManager Content
		{
			get { return Game.Content; }
		}

		public IInputSystem Input { get; set; }

		public World PhysicsWorld { get; set; }

		public Settings Settings { get; set; }

		public SoundManager SoundManager { get; set; }

		#region Singleton

		private Platform()
		{
		}

		private static class PlatformInstanceHolder
		{
			// ReSharper disable MemberHidesStaticFromOuterClass
			public static readonly Platform Instance = new Platform();
			// ReSharper restore MemberHidesStaticFromOuterClass
		}

		#endregion
	}
}
