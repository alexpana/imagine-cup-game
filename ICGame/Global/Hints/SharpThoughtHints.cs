using System;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using VertexArmy.Global.Managers;

namespace VertexArmy.Global.Hints
{
	class SharpThoughtHint : IDismissableHint
	{

		private readonly Texture2D _background = Platform.Instance.Content.Load<Texture2D>("images/empty_dark_blue");
		private readonly Vector2 _position;
		private float _timeToLive;

		public SharpThoughtHint( string text, Vector2 position, float timeToLive )
		{
			_position = position;
			_timeToLive = timeToLive;
		}

		public string Text { get; set; }

		public void Update( GameTime time )
		{
			_timeToLive -= time.ElapsedGameTime.Milliseconds;
		}

		public void Render()
		{
			HintManager.Instance.RenderTexture( _background, _position, null, Color.White, 0, new Vector2( 0, 0 ), 1 );
		}

		public Action DismissedCallback { get; set; }

		public bool ShouldDismiss()
		{
			return _timeToLive <= 0;
		}
	}
}
