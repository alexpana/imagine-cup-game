using System;
using FarseerPhysics.Dynamics;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using UnifiedInputSystem;
using VertexArmy.Global.Managers;
#if NETFX_CORE
using System.IO;
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
				using ( var memoryStream = new MemoryStream() )
				{
					stream.CopyTo( memoryStream );
					memoryStream.Position = 0;

					return new Effect( Device, memoryStream.ToArray() );
				}
			}
#else
			return Content.Load<Effect>( "effects/" + path );
#endif
		}

		public InputAggregator Input { get; set; }

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
