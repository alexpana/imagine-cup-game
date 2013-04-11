
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

		private readonly Texture2D _thinkingBubbleTexture = Platform.Instance.Content.Load<Texture2D>( "images/white" );

		private const int DefaultHintFadeTime = 1000;

		public HintManager()
		{
			_activeHints = new List<Hint>();
			_spriteBatch = new SpriteBatch( Platform.Instance.Device );
			_font = Platform.Instance.Content.Load<SpriteFont>( "fonts/Impact" );
		}

		public void SpawnHint( string text, Vector2 startPosition, Vector2? endPosition, float msTime, int layer = 0,
							  Action dismissedCallback = null, uint fadeTime = DefaultHintFadeTime )
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

			var hint = endPosition.HasValue
				? new Hint( startPosition, endPosition.Value, msTime, fadeTime )
				: new Hint( startPosition, msTime, fadeTime );

			_activeHints.Add( hint );

			hint.Text = text;
			hint.Layer = layer;
			hint.DismissedCallback = dismissedCallback;
			hint.ThinkingBubbleTexture = _thinkingBubbleTexture;
			hint.BackgroundTexture = text.Contains( "\n" ) ? _backgroundDoubleLine : _backgroundSingleLine;
			hint.Font = _font;
		}

		public void SpawnHint( string text, Vector2 position, float msTime, int layer = 0,
							  Action dismissedCallback = null, uint fadeTime = DefaultHintFadeTime )
		{
			SpawnHint( text, position, null, msTime, layer, dismissedCallback, fadeTime );
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

		public void Render( float dt )
		{
			_spriteBatch.Begin( SpriteSortMode.Immediate, BlendState.AlphaBlend, SamplerState.LinearClamp, DepthStencilState.Default, RasterizerState.CullCounterClockwise, null );

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
}