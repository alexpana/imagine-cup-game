
using System;
using System.Collections.Generic;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using VertexArmy.Global.Behaviours;
using VertexArmy.Global.Hints;

namespace VertexArmy.Global.Managers
{
	public class HintManager : IUpdatable
	{
		private readonly List<RobotThoughtHint> _activeHints;
		private readonly SpriteBatch _spriteBatch;
		private readonly SpriteFont _font;

		private readonly Texture2D _tooltipBackgroundTop = Platform.Instance.Content.Load<Texture2D>( "images/menu/metro/tooltip_top" );
		private readonly Texture2D _tooltipBackgroundMiddle = Platform.Instance.Content.Load<Texture2D>( "images/menu/metro/tooltip_middle" );
		private readonly Texture2D _tooltipBackgroundBottom = Platform.Instance.Content.Load<Texture2D>( "images/menu/metro/tooltip_bottom" );

		private readonly Texture2D _thinkingBubbleTexture = Platform.Instance.Content.Load<Texture2D>( "images/menu/metro/color_hint_bubble" );

		private const int DefaultHintFadeTime = 1000;

		public HintManager()
		{
			_activeHints = new List<RobotThoughtHint>();
			_spriteBatch = new SpriteBatch( Platform.Instance.Device );
			_font = Platform.Instance.Content.Load<SpriteFont>( "fonts/Impact" );
		}

		public void SpawnHint( string text, Vector2 startPosition, Vector2? endPosition, float msTime, int layer = 0,
							  Action dismissedCallback = null, uint fadeTime = DefaultHintFadeTime )
		{
			if ( layer != 0 )
			{
				foreach ( RobotThoughtHint h in _activeHints )
				{
					if ( h.Layer == layer )
					{
						h.Time = -1f;
					}
				}
			}

			var hint = endPosition.HasValue
				? new RobotThoughtHint( text, startPosition, endPosition.Value, msTime, fadeTime )
				: new RobotThoughtHint( text, startPosition, msTime, fadeTime );

			_activeHints.Add( hint );

			hint.Layer = layer;
			hint.DismissedCallback = dismissedCallback;
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
				List<RobotThoughtHint> hintsToRemove = new List<RobotThoughtHint>();
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
				Render( activeHint );
			}

			_spriteBatch.End();
		}

		private void RenderHintBackground( Vector2 position, int linecount, float alpha )
		{
			// Delta position should be 20, 16
			_spriteBatch.Draw( _tooltipBackgroundTop, position, Color.White * alpha );
			position.Y += _tooltipBackgroundTop.Height;
			for ( int i = 0; i < linecount - 1; ++i )
			{
				_spriteBatch.Draw( _tooltipBackgroundMiddle, position, Color.White * alpha );
				position.Y += _tooltipBackgroundMiddle.Height;
			}
			_spriteBatch.Draw( _tooltipBackgroundBottom, position, Color.White * alpha );
		}

		public void Render( RobotThoughtHint hint )
		{
			foreach ( var thinkingBubble in hint.ThinkingBubbles )
			{
				_spriteBatch.Draw( _thinkingBubbleTexture, thinkingBubble.Position, null, Color.White * thinkingBubble.Alpha, 0, Vector2.Zero, thinkingBubble.Scale, SpriteEffects.None, 0 );
			}

			Vector2 offset = new Vector2( 20, 16 );
			RenderHintBackground( hint.CurrentPosition - offset, hint.LinesCount, hint.Alpha );

			_spriteBatch.DrawString( _font, hint.Text, hint.CurrentPosition, hint.Color * hint.Alpha );
		}

		// ReSharper disable InconsistentNaming
		private static readonly HintManager _instance = new HintManager();
		// ReSharper restore InconsistentNaming
		public static HintManager Instance
		{
			get { return _instance; }
		}

		public void Clear( )
		{
			_activeHints.Clear( );
		}
	}
}