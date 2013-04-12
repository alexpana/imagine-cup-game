using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.Xna.Framework;

namespace VertexArmy.Global.Managers
{
	public class HintBubble
	{
		public const int TimeToLive = 800;

		public Vector2 Position { get; set; }
		public float Alpha { get; set; }

		public float Scale { get; set; }
		public int Time { get; set; }
	}

	public class Hint
	{
		private const int ThinkingSpeed = 1500;
		private const int ThinkingBubbleInterval = 100;
		private const float ThinkingBubbleInitialScale = 0.5f;
		private const float ThinkingBubbleScaleIncrement = 0.05f;

		internal enum HintState
		{
			Created,
			Thinking,
			FadeIn,
			InPlace,
			FadeOut
		}

		public float Time { get; set; } // in miliseconds
		public int Layer { get; set; }

		public List<HintBubble> ThinkingBubbles { get; private set; }
		private int _lastBubbleTime;

		private HintState _state;

		public Vector2 CurrentPosition { get; private set; }
		private readonly Vector2 _finalPosition;
		private readonly Vector2 _startPosition;

		public Color Color { get; private set; }

		public int LinesCount { get; private set; }
		public string Text { get; private set; }

		#region Thinking
		private int _thinkTime;
		#endregion

		#region Fading
		private readonly uint _fadeTime;
		private int _currentFadeTime;
		private int _currentFadeOperation; // -1 fade In, 0, 1 fade Out
		public float Alpha { get; private set; }
		#endregion

		public Action DismissedCallback;

		public Hint( string text, Vector2 startPosition, Vector2 endPosition, float msTime, uint fadeTime )
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

		public Hint( string text, Vector2 finalPosition, float msTime, uint fadeTime )
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
							Position = CurrentPosition,
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
	}
}
