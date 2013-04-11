using System;
using System.Collections.Generic;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace VertexArmy.Global.Managers
{
	internal class Hint
	{
		private const int ThinkingSpeed = 1000;
		private const int ThinkingBubbleInterval = 1000;

		internal enum HintState
		{
			Created,
			Thinking,
			FadeIn,
			InPlace,
			FadeOut
		}

		public string Text { get; set; }
		public float Time { get; set; } // in miliseconds
		public int Layer { get; set; }
		public Texture2D BackgroundTexture { get; set; }
		public SpriteFont Font { get; set; }

		private readonly List<Vector2> _thinkingBubbles;
		private int _lastBubbleTime;

		private HintState _state;

		private Vector2 _currentPosition;
		private readonly Vector2 _finalPosition;
		private readonly Vector2 _startPosition;

		private readonly Color _color;

		#region Thinking
		public Texture2D ThinkingBubbleTexture { get; set; }
		private int _thinkTime;
		#endregion

		#region Fading
		private readonly uint _fadeTime;
		private int _currentFadeTime;
		private int _currentFadeOperation; // -1 fade In, 0, 1 fade Out
		private float _alpha;
		#endregion

		public Action DismissedCallback;

		public Hint( Vector2 startPosition, Vector2 endPosition, float msTime, uint fadeTime )
		{
			_thinkingBubbles = new List<Vector2>();

			_color = new Color( 32.0f / 255.0f, 40.0f / 255.0f, 50.0f / 255.0f );
			_currentFadeOperation = 1;
			Layer = 0;
			_startPosition = startPosition;
			_finalPosition = endPosition;
			_currentPosition = startPosition;

			_fadeTime = fadeTime;

			if ( _fadeTime <= 0 )
			{
				_alpha = 1.0f;
			}

			// total time contains the fade in and fade out times (besides the normal msTime)
			Time = msTime + fadeTime * 2 + ThinkingSpeed;

			_state = HintState.Thinking;
		}

		public Hint( Vector2 finalPosition, float msTime, uint fadeTime )
			: this( finalPosition, finalPosition, msTime, fadeTime )
		{
			_state = HintState.FadeIn;
		}

		public void Render( SpriteBatch spriteBatch )
		{
			spriteBatch.Draw( BackgroundTexture, _currentPosition - new Vector2( 20, 16 ), Color.White * _alpha );

			spriteBatch.DrawString( Font, Text, _currentPosition, _color * _alpha );
		}

		public void Update( GameTime gameTime )
		{
			Time -= gameTime.ElapsedGameTime.Milliseconds;
			if ( _state == HintState.Thinking )
			{
				_thinkTime += gameTime.ElapsedGameTime.Milliseconds;

				float amount = ( float ) _thinkTime / ThinkingSpeed;
				_currentPosition = Vector2.Lerp( _startPosition, _finalPosition, amount );

				// finished yet?
				if ( amount >= 1 )
				{
					_state = HintState.FadeIn;
				}
				else
				{
					if ( _thinkTime - _lastBubbleTime > ThinkingBubbleInterval )
					{
						_thinkingBubbles.Add( _currentPosition );
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

					_alpha = MathHelper.Lerp( 0, 1.0f, ( float ) _currentFadeTime / _fadeTime );
				}
			}
		}
	}
}
