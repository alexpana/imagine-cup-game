using System;
using FarseerPhysics.Dynamics;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using VertexArmy.Global.Managers;
using VertexArmy.Input;
#if NETFX_CORE
using System.IO;
using SharpDX.Text;

#endif

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

		//TODO: create a custom Content Manager to handle the content loading in the game better
		public Effect LoadEffect( string path )
		{
#if NETFX_CORE
			using ( var stream = TitleContainer.OpenStream( "Content/effects/" + path + ".mgfxo" ) )
			{
				using ( var sr = new StreamReader( stream ) )
				{
					return new Effect( Device, Encoding.ASCII.GetBytes( sr.ReadToEnd() ) );
				}
			}
#else
			return Content.Load<Effect>( "effects/" + path );
#endif
		}


		public IInputSystem Input { get; set; }

		public World PhysicsWorld { get; set; }

		public Settings Settings { get; set; }

		public SoundManager SoundManager { get; set; }

		public Random GlobalRandom = new Random();

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
