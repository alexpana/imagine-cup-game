
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

		private readonly Texture2D _tooltipBackgroundTop = Platform.Instance.Content.Load<Texture2D>( "menu/metro/tooltip_top" );
		private readonly Texture2D _tooltipBackgroundMiddle = Platform.Instance.Content.Load<Texture2D>( "menu/metro/tooltip_middle" );
		private readonly Texture2D _tooltipBackgroundBottom = Platform.Instance.Content.Load<Texture2D>( "menu/metro/tooltip_bottom" );

		private const int DefaultHintFadeTime = 1000;

		public HintManager()
		{
			_activeHints = new List<Hint>();
			_spriteBatch = new SpriteBatch( Platform.Instance.Device );
			_font = Platform.Instance.Content.Load<SpriteFont>( "fonts/Impact" );
		}

		public void SpawnHint( string text, Vector2 position, float msTime, int layer = 0, Action dismissedCallback = null, int fadeTime = DefaultHintFadeTime )
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
				Time = msTime + fadeTime * 2,
				FadeTime = fadeTime,
				Layer = layer,
				DismissedCallback = dismissedCallback,
				BackgroundTexture = text.Contains( "\n" ) ? _backgroundDoubleLine : _backgroundSingleLine,
				Font = _font
			} );
		}


		public void Update( GameTime gameTime )
		{
			lock ( _activeHints )
			{
				List<Hint> hintsToRemove = new List<Hint>();
				foreach ( var activeHint in _activeHints )
				{
					activeHint.Update( gameTime );

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

		private void RenderHintBackground( Vector2 position, int linecount, float alpha )
		{
			// Delta position should be 20, 16
			spriteBatch.Draw( _tooltipBackgroundTop, position, Color.White * Alpha );
			position.Y += _tooltipBackgroundTop.Height;
			for ( int i = 0; i < linecount - 1; ++i )
			{
				spriteBatch.Draw( _tooltipBackgroundMiddle, position, Color.White * Alpha );
				position += _tooltipBackgroundMiddle.Height;
			}
			_spriteBatch.Draw( _tooltipBackgroundTop, position, Color.White * Alpha );
		}

		public void Render( float dt )
		{
			_spriteBatch.Begin( SpriteSortMode.Immediate, BlendState.AlphaBlend, SamplerState.LinearClamp, DepthStencilState.Default, RasterizerState.CullCounterClockwise, null );

			Vector2 offset = new Vector2( 20, 16 );
			foreach ( var activeHint in _activeHints )
			{
				activeHint.Render( _spriteBatch );
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
			CurrentFadeOperation = 1;
			Layer = 0;
		}

		public string Text;
		public Vector2 Position;

		public float Time; // in miliseconds
		public int Layer;

		public Color Color;
		public Texture2D BackgroundTexture;
		public SpriteFont Font;

		public int FadeTime;
		public int CurrentFadeTime;
		public int CurrentFadeOperation; // -1 fade In, 0, 1 fade out
		public float Alpha;

		public Action DismissedCallback;

		public void Render( SpriteBatch spriteBatch )
		{
			spriteBatch.DrawString( Font, Text, Position,
				Color * Alpha );
		}

		public void Update( GameTime gameTime )
		{
			Time -= gameTime.ElapsedGameTime.Milliseconds;
			if ( FadeTime > 0 )
			{
				CurrentFadeTime += CurrentFadeOperation * gameTime.ElapsedGameTime.Milliseconds;

				// fade time is done, stop the fading op
				if ( CurrentFadeTime >= FadeTime )
				{
					CurrentFadeOperation = 0;
				}
				// last fadetime of the hint's life, fade out
				if ( Time <= FadeTime )
				{
					CurrentFadeOperation = -1;
				}

				Alpha = MathHelper.Lerp( 0, 1.0f, ( float ) CurrentFadeTime / FadeTime );
			}
			else
			{
				Alpha = 1.0f;
			}

		}
	}
}