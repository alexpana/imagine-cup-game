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

		public float Time { get; set; } // in miliseconds
		public int Layer { get; set; }
		public Texture2D BackgroundTopTexture { get; set; }
		public Texture2D BackgroundMiddleTexture { get; set; }
		public Texture2D BackgroundBottomTexture { get; set; }
		public SpriteFont Font { get; set; }

		private readonly List<Vector2> _thinkingBubbles;
		private int _lastBubbleTime;

		private HintState _state;

		private Vector2 _currentPosition;
		private readonly Vector2 _finalPosition;
		private readonly Vector2 _startPosition;

		private readonly Color _color;

		private readonly int _linesCount;
		private string _text;

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

		public Hint( string text, Vector2 startPosition, Vector2 endPosition, float msTime, uint fadeTime )
		{
			_thinkingBubbles = new List<Vector2>();

			_text = text;
			//TODO: rework this
			_linesCount = text.Split( '\n' ).Length;

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

		public Hint( string text, Vector2 finalPosition, float msTime, uint fadeTime )
			: this( text, finalPosition, finalPosition, msTime, fadeTime )
		{
			_state = HintState.FadeIn;
		}

		private void RenderHintBackground( SpriteBatch spriteBatch, Vector2 position, int linecount, float alpha )
		{
			// Delta position should be 20, 16
			spriteBatch.Draw( BackgroundTopTexture, position, Color.White * alpha );
			position.Y += BackgroundTopTexture.Height;
			for ( int i = 0; i < linecount - 1; ++i )
			{
				spriteBatch.Draw( BackgroundMiddleTexture, position, Color.White * alpha );
				position.Y += BackgroundMiddleTexture.Height;
			}
			spriteBatch.Draw( BackgroundBottomTexture, position, Color.White * alpha );
		}
		public void Render( SpriteBatch spriteBatch )
		{
			Vector2 offset = new Vector2( 20, 16 );
			RenderHintBackground( spriteBatch, _currentPosition - offset, _linesCount, _alpha );

			spriteBatch.DrawString( Font, _text, _currentPosition, _color * _alpha );
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
