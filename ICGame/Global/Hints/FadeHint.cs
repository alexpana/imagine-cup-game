using System;
using Microsoft.Xna.Framework;
using VertexArmy.Global.Managers;

namespace VertexArmy.Global.Hints
{
	public class FadeHint : IDismissableHint
	{
		private Color _color;
		private string _text;
		private readonly float _fadeInMs;
		private readonly float _fadeOutMs;
		private readonly float _timeToLiveMs;
		private float _timer;
		private HintState _state;

		public Vector2 Position;

		public Color Color
		{
			get { return _color; }
			set
			{
				_color.R = value.R;
				_color.G = value.G;
				_color.B = value.B;
			}
		}

		public string Text
		{
			get { return _text; }
			set
			{
				_text = value;
				Lines = _text.Split( '\n' ).Length;
			}
		}


		public int Lines { get; private set; }

		public FadeHint( string text, Vector2 position, float fadeIn, float fadeOut ) :
			this( text, position, new Color( 0.81f, 0.5f, 0.33f ), float.PositiveInfinity, fadeIn, fadeOut )
		{ }

		public FadeHint( string text, Vector2 position, float timeToLive, float fadeIn, float fadeOut ) :
			this( text, position, new Color( 0.81f, 0.5f, 0.33f ), timeToLive, fadeIn, fadeOut )
		{ }

		public FadeHint( string text, Vector2 position, Color color, float timeToLive, float fadeIn, float fadeOut )
		{
			_fadeInMs = fadeIn;
			_fadeOutMs = fadeOut;
			_timeToLiveMs = timeToLive;
			Position = position;
			Text = text;
			_state = HintState.Stopped;
			Color = color;
			_color.A = 0;
			_timer = 0;
		}

		public bool IsRenderable()
		{
			return _state == HintState.Playing || _state == HintState.FadeIn || _state == HintState.FadeOut;
		}

		public void StopHintAsync()
		{
			switch ( _state )
			{
				case HintState.Playing:
					_timer = 0;
					_state = HintState.FadeOut;
					break;
			}
		}

		public void StartAsync()
		{
			switch ( _state )
			{
				case HintState.Playing:
					_timer = 0;
					break;
				case HintState.FadeIn:
					break;
				case HintState.FadeOut:
					break;
				default:
					_color.A = 0;
					_timer = 0;
					_state = HintState.FadeIn;
					break;
			}
		}

		public void StopHint()
		{
			_timer = 0;
			_state = HintState.Stopped;
		}

		public void Update( GameTime time )
		{
			switch ( _state )
			{
				case HintState.FadeIn:
					_timer += ( float ) time.ElapsedGameTime.TotalMilliseconds;
					_color.A = ( byte ) ( 255f * _timer / _fadeInMs );
					if ( _timer > _fadeInMs )
					{
						_timer = 0;
						_color.A = 255;
						_state = HintState.Playing;
					}
					break;
				case HintState.Playing:
					_timer += ( float ) time.ElapsedGameTime.TotalMilliseconds;
					if ( _timer > _timeToLiveMs )
					{
						_timer = 0;
						_state = HintState.FadeOut;
					}
					break;
				case HintState.FadeOut:
					_timer += ( float ) time.ElapsedGameTime.TotalMilliseconds;
					_color.A = ( byte ) ( 255f * ( 1f - _timer / _fadeOutMs ) );
					if ( _timer > _fadeOutMs )
					{
						_color.A = 0;
						_timer = 0;
						_state = HintState.Stopped;
					}
					break;
				case HintState.Stopped:
					break;
			}
		}

		public void Render()
		{
			if ( IsRenderable() )
			{
				Vector2 offset = new Vector2( 20, 16 );
				HintManager.Instance.RenderHintBackground( Position - offset, Lines, Color.A / 255f );
				HintManager.Instance.RenderString( Text, Position, Color );
			}
		}

		internal enum HintState
		{
			FadeIn,
			Playing,
			FadeOut,
			Stopped,
		}

		public Action DismissedCallback { get; set; }

		public bool ShouldDismiss()
		{
			return _state == HintState.Stopped;
		}
	}
}
