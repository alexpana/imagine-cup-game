
using System;
using System.Collections.Generic;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using VertexArmy.Global.Behaviours;

namespace VertexArmy.Global.Managers
{
	public class HintManager : IUpdatable
	{
		private List<Hint> _activeHints;
		private SpriteBatch _spriteBatch;
		private SpriteFont _font;
		private Object _lockThis = new Object();

		public HintManager()
		{
			_activeHints = new List<Hint>();
			_spriteBatch = new SpriteBatch(Platform.Instance.Device);
			_font = Platform.Instance.Content.Load<SpriteFont>( "fonts/SpriteFont1" );
		}

		public void SpawnHint( string text, Vector2 position, float time )
		{
			_activeHints.Add( new Hint() { Text = text, Position = position, Time = time } );
		}
		public static HintManager Instance
		{
			get { return HintManagerInstanceHolder.Instance; }
		}


		private static class HintManagerInstanceHolder
		{
			// ReSharper disable MemberHidesStaticFromOuterClass
			public static readonly HintManager Instance = new HintManager();
			// ReSharper restore MemberHidesStaticFromOuterClass
		}

		public void Update(GameTime dt)
		{
			lock ( _activeHints )
			{
				List<Hint> removethis = new List<Hint>();
				foreach ( var activeHint in _activeHints )
				{
					activeHint.Time -= dt.ElapsedGameTime.Milliseconds;
					if ( activeHint.Time < 0 )
						removethis.Add( activeHint );
				}

				foreach ( var activeHint in removethis )
				{
					_activeHints.Remove(activeHint);
				}
			}
			
			
		}

		public void Render(float dt)
		{
			

			_spriteBatch.Begin( SpriteSortMode.Immediate, BlendState.AlphaBlend, SamplerState.LinearClamp, DepthStencilState.Default, RasterizerState.CullCounterClockwise, null );
			
			
			foreach (var activeHint in _activeHints)
			{
				_spriteBatch.DrawString( _font, activeHint.Text, activeHint.Position, activeHint.Color );
			}

			_spriteBatch.End();
		}
	}

	public class Hint
	{
		public Hint ()
		{
			Color = Color.White;
		}
		public string Text;
		public Vector2 Position;
		public float Time; // in seconds
		public Color Color;
	}
}