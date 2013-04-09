
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

		private Texture2D _backgroundDoubleLine = Platform.Instance.Content.Load<Texture2D>( "images/tooltip_double_line" );

		private Texture2D _backgroundSingleLine = Platform.Instance.Content.Load<Texture2D>( "images/tooltip_single_line" );
		public HintManager()
		{
			_activeHints = new List<Hint>();
			_spriteBatch = new SpriteBatch( Platform.Instance.Device );
			_font = Platform.Instance.Content.Load<SpriteFont>( "fonts/Impact" );
		}

		public void SpawnHint( string text, Vector2 position, float time, int layer = 0 )
		{
			if ( layer != 0 )
			{
				foreach ( Hint h in _activeHints )
				{
					if ( h.Layer == layer )
					{
						h.Time = -1f;
					}
				}
			}
			_activeHints.Add( new Hint() { Text = text, Position = position, Time = time, Layer = layer } );
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

		public void Update( GameTime dt )
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
					_activeHints.Remove( activeHint );
				}
			}


		}

		public void Render( float dt )
		{

			_spriteBatch.Begin( SpriteSortMode.Immediate, BlendState.AlphaBlend, SamplerState.LinearClamp, DepthStencilState.Default, RasterizerState.CullCounterClockwise, null );


			foreach ( var activeHint in _activeHints )
			{
				Texture2D background = activeHint.Text.Contains( "\n" ) ? _backgroundDoubleLine : _backgroundSingleLine;

				_spriteBatch.Draw( background, activeHint.Position - new Vector2( 20, 16 ), Color.White );

				_spriteBatch.DrawString( _font, activeHint.Text, activeHint.Position, activeHint.Color );
			}

			_spriteBatch.End();
		}
	}

	public class Hint
	{
		public Hint()
		{
			Color = new Color( 212.0f / 255.0f, 131.0f / 255.0f, 87.0f / 255.0f );
			Layer = 0;
		}
		public string Text;
		public Vector2 Position;
		public float Time; // in seconds
		public Color Color;
		public int Layer;
	}
}