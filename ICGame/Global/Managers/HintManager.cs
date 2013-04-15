
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
		private readonly List<RobotThoughtHint> _robotThoughtHints;
		private readonly List<FadeHint> _fadeHints;
		private readonly SpriteBatch _spriteBatch;
		private readonly SpriteFont _font;

		private readonly Texture2D _tooltipBackgroundTop = Platform.Instance.Content.Load<Texture2D>( "images/menu/metro/tooltip_top" );
		private readonly Texture2D _tooltipBackgroundMiddle = Platform.Instance.Content.Load<Texture2D>( "images/menu/metro/tooltip_middle" );
		private readonly Texture2D _tooltipBackgroundBottom = Platform.Instance.Content.Load<Texture2D>( "images/menu/metro/tooltip_bottom" );

		private readonly Texture2D _thinkingBubbleTexture = Platform.Instance.Content.Load<Texture2D>( "images/menu/metro/color_hint_bubble" );

		private const int DefaultHintFadeTime = 1000;

		public HintManager()
		{
			_fadeHints = new List<FadeHint>();
			_robotThoughtHints = new List<RobotThoughtHint>();
			_spriteBatch = new SpriteBatch( Platform.Instance.Device );
			_font = Platform.Instance.Content.Load<SpriteFont>( "fonts/Impact" );
		}

		public void SpawnHint( FadeHint hint )
		{
			if(!_fadeHints.Contains(hint))
				_fadeHints.Add( hint );
		}

		public FadeHint SpawnHint( string text, Vector2 position, float fadeInTime, float fadeOutTime )
		{
			FadeHint hint = new FadeHint( text, position, fadeInTime, fadeOutTime );
			_fadeHints.Add( hint );
			hint.StartAsync();
			return hint;
		}

		public void RemoveHint ( FadeHint hint )
		{
			_fadeHints.Remove( hint );
		}

		public void SpawnHint( string text, Vector2 startPosition, Vector2? endPosition, float msTime, int layer = 0,
							  Action dismissedCallback = null, uint fadeTime = DefaultHintFadeTime )
		{
			if ( layer != 0 )
			{
				foreach ( RobotThoughtHint h in _robotThoughtHints )
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

			_robotThoughtHints.Add( hint );

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
			lock ( _robotThoughtHints )
			{
				List<RobotThoughtHint> hintsToRemove = new List<RobotThoughtHint>();
				foreach ( var activeHint in _robotThoughtHints )
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

					_robotThoughtHints.Remove( activeHint );
				}
			}

			foreach (FadeHint fadeHint in _fadeHints)
			{
				fadeHint.Update( gameTime );
			}
		}

		public void Render( float dt )
		{
			_spriteBatch.Begin( SpriteSortMode.Immediate, BlendState.NonPremultiplied, SamplerState.LinearClamp, DepthStencilState.Default, RasterizerState.CullCounterClockwise, null );

			foreach ( var activeHint in _robotThoughtHints )
			{
				Render( activeHint );
			}

			foreach (FadeHint fadeHint in _fadeHints)
			{
				if(fadeHint.IsRenderable())
				{
					Vector2 offset = new Vector2( 20, 16 );
					RenderHintBackground(fadeHint.Position - offset, fadeHint.Lines, fadeHint.Color.A / 255f);
					_spriteBatch.DrawString( _font, fadeHint.Text, fadeHint.Position, fadeHint.Color );
				}
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

		public void Clear()
		{
			_fadeHints.Clear();
			_robotThoughtHints.Clear();
		}
	}
}