
using System;
using System.Collections.Generic;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using VertexArmy.Global.Behaviours;

namespace VertexArmy.Global.Managers
{
	public class HintManager : IUpdatable
	{
		private readonly List<Hint> _activeHints;
		private readonly SpriteBatch _spriteBatch;
		private readonly SpriteFont _font;

		private readonly Texture2D _backgroundDoubleLine = Platform.Instance.Content.Load<Texture2D>( "images/tooltip_double_line" );
		private readonly Texture2D _backgroundSingleLine = Platform.Instance.Content.Load<Texture2D>( "images/tooltip_single_line" );

		private const int FadeTime = 1000;

		public HintManager()
		{
			_activeHints = new List<Hint>();
			_spriteBatch = new SpriteBatch( Platform.Instance.Device );
			_font = Platform.Instance.Content.Load<SpriteFont>( "fonts/Impact" );
		}

		public void SpawnHint( string text, Vector2 position, float msTime, int layer = 0, Action dismissedCallback = null )
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
			_activeHints.Add( new Hint
			{
				Text = text,
				Position = position,
				// total time contains the fade in and fade out times (besides the normal msTime)
				Time = msTime + FadeTime * 2,
				Layer = layer,
				DismissedCallback = dismissedCallback
			} );
		}


		public void Update( GameTime dt )
		{
			lock ( _activeHints )
			{
				List<Hint> hintsToRemove = new List<Hint>();
				foreach ( var activeHint in _activeHints )
				{
					activeHint.Time -= dt.ElapsedGameTime.Milliseconds;
					activeHint.FadeTime += activeHint.FadeOperation * dt.ElapsedGameTime.Milliseconds;

					// fade time is done, stop the fading op
					if ( activeHint.FadeTime >= FadeTime )
					{
						activeHint.FadeOperation = 0;
					}
					// last fadetime of the hint's life, fade out
					if ( activeHint.Time <= FadeTime )
					{
						activeHint.FadeOperation = -1;
					}

					activeHint.Alpha = MathHelper.Lerp( 0, 1.0f, ( float ) activeHint.FadeTime / FadeTime );

					if ( activeHint.Time < 0 )
					{
						hintsToRemove.Add( activeHint );
					}
				}

				foreach ( var activeHint in hintsToRemove )
				{
					if ( activeHint.DismissedCallback != null )
					{
						activeHint.DismissedCallback();
					}

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

				_spriteBatch.Draw( background, activeHint.Position - new Vector2( 20, 16 ),
					Color.White * activeHint.Alpha );

				_spriteBatch.DrawString( _font, activeHint.Text, activeHint.Position,
					activeHint.Color * activeHint.Alpha );
			}

			_spriteBatch.End();
		}

		// ReSharper disable InconsistentNaming
		private static readonly HintManager _instance = new HintManager();
		// ReSharper restore InconsistentNaming
		public static HintManager Instance
		{
			get { return _instance; }
		}
	}

	public class Hint
	{
		public Hint()
		{
			Color = new Color( 32.0f / 255.0f, 40.0f / 255.0f, 50.0f / 255.0f );
			FadeOperation = 1;
			Layer = 0;
		}

		public string Text;
		public Vector2 Position;

		public float Time; // in miliseconds
		public int Layer;

		public Color Color;

		public int FadeTime;
		public int FadeOperation; // -1 fade In, 0, 1 fade out
		public float Alpha;

		public Action DismissedCallback;
	}
}