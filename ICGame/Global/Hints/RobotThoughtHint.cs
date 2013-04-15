using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using VertexArmy.Global.Managers;

namespace VertexArmy.Global.Hints
{
	public class HintBubble
	{
		public const int TimeToLive = 1600;

		public Vector2 Position { get; set; }
		public float Alpha { get; set; }

		public float Scale { get; set; }
		public int Time { get; set; }
	}

	public class RobotThoughtHint : IDismissableHint
	{
		public Action DismissedCallback { get; set; }

		private const int ThinkingSpeed = 500;
		private const int ThinkingBubbleInterval = (int)(500.0 / 4.7);
		private const float ThinkingBubbleInitialScale = 0.5f;
		private const float ThinkingBubbleScaleIncrement = 0.2f;

		private readonly Texture2D _bubbleTexture = Platform.Instance.Content.Load<Texture2D>( "images/menu/metro/color_hint_bubble"  );

		internal enum HintState
		{
			Created,
			Thinking,
			FadeIn,
			InPlace,
			FadeOut
		}

		public float Time { get; set; } // in milliseconds
		public int Layer { get; set; }

		public List<HintBubble> ThinkingBubbles { get; private set; }
		private int _lastBubbleTime;

		private HintState _state;

		public Vector2 CurrentPosition { get; private set; }
		private readonly Vector2 _finalPosition;
		private readonly Vector2 _startPosition;

		public Color Color { get; private set; }

		public int LinesCount { get; private set; }
		public string Text { get; set; }

		#region Thinking
		private int _thinkTime;
		#endregion

		#region Fading
		private readonly uint _fadeTime;
		private int _currentFadeTime;
		private int _currentFadeOperation; // -1 fade In, 0, 1 fade Out
		public float Alpha { get; private set; }
		#endregion
		
		public RobotThoughtHint( string text, Vector2 startPosition, Vector2 endPosition, float msTime, uint fadeTime )
		{
			ThinkingBubbles = new List<HintBubble>();

			Text = text;
			//TODO: rework this
			LinesCount = text.Split( '\n' ).Length;

			Color = new Color( 32.0f / 255.0f, 40.0f / 255.0f, 50.0f / 255.0f );
			_currentFadeOperation = 1;
			Layer = 0;
			_startPosition = startPosition;
			_finalPosition = endPosition;
			CurrentPosition = startPosition;

			_fadeTime = fadeTime;

			if ( _fadeTime <= 0 )
			{
				Alpha = 1.0f;
			}

			// total time contains the fade in and fade out times (besides the normal msTime)
			Time = msTime + fadeTime * 2 + ThinkingSpeed;

			_state = HintState.Thinking;
		}

		public RobotThoughtHint( string text, Vector2 finalPosition, float msTime, uint fadeTime )
			: this( text, finalPosition, finalPosition, msTime, fadeTime )
		{
			_state = HintState.FadeIn;
		}

		public void Update( GameTime gameTime )
		{
			Time -= gameTime.ElapsedGameTime.Milliseconds;
			for ( int i = 0; i < ThinkingBubbles.Count; ++i )
			{
				var thinkingBubble = ThinkingBubbles[i];
				thinkingBubble.Time -= gameTime.ElapsedGameTime.Milliseconds;

				if ( thinkingBubble.Time <= HintBubble.TimeToLive / 2 )
				{
					thinkingBubble.Alpha = thinkingBubble.Time / ( HintBubble.TimeToLive / 2.0f );
				}
				else
				{
					thinkingBubble.Alpha = 1 - ( thinkingBubble.Time - ( HintBubble.TimeToLive / 2.0f ) ) / ( HintBubble.TimeToLive / 2.0f );
				}

				if ( thinkingBubble.Time <= 0 )
				{
					ThinkingBubbles.RemoveAt( i );
					--i;
				}
			}

			if ( _state == HintState.Thinking )
			{
				_thinkTime += gameTime.ElapsedGameTime.Milliseconds;

				float amount = ( float ) _thinkTime / ThinkingSpeed;
				CurrentPosition = Vector2.Lerp( _startPosition, _finalPosition, amount );

				// finished yet?
				if ( amount >= 1 )
				{
					_state = HintState.FadeIn;
				}
				else
				{
					if ( _thinkTime - _lastBubbleTime > ThinkingBubbleInterval )
					{
						ThinkingBubbles.Add( new HintBubble
						{
							Position = CurrentPosition + new Vector2( 10, 50 ),
							Scale = ThinkingBubbles.Count > 0 ? ThinkingBubbles.Last().Scale + ThinkingBubbleScaleIncrement : ThinkingBubbleInitialScale,
							Time = HintBubble.TimeToLive
						} );
						_lastBubbleTime = _thinkTime;
					}
				}
			}
			else if ( _state != HintState.Created )
			{
				if ( _fadeTime > 0 )
				{
					_currentFadeTime += _currentFadeOperation * gameTime.ElapsedGameTime.Milliseconds;

					// fade time is done, stop the fading op
					if ( _currentFadeTime >= _fadeTime )
					{
						_state = HintState.InPlace;
						_currentFadeOperation = 0;
					}
					// last fade time of the hint's life, fade out
					if ( Time <= _fadeTime )
					{
						_state = HintState.FadeOut;
						_currentFadeOperation = -1;
					}

					Alpha = MathHelper.Lerp( 0, 1.0f, ( float ) _currentFadeTime / _fadeTime );
				}
			}
		}

		public void Render()
		{
			foreach ( var thinkingBubble in ThinkingBubbles )
			{
				HintManager.Instance.RenderTexture( _bubbleTexture, thinkingBubble.Position, null, Color.White * thinkingBubble.Alpha, 0, Vector2.Zero, thinkingBubble.Scale, SpriteEffects.None, 0 );
			}

			Vector2 offset = new Vector2( 20, 16 );
			HintManager.Instance.RenderHintBackground( CurrentPosition - offset, LinesCount, Alpha );
			HintManager.Instance.RenderString( Text, CurrentPosition, Color * Alpha );
		}

		public bool ShouldDismiss()
		{
			return Time < 0;
		}
	}
}
