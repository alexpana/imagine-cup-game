using System;
using System.Collections.Generic;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using UnifiedInputSystem.Events;
using VertexArmy.Global.Behaviours;
using VertexArmy.Graphics;

namespace VertexArmy.Global.Managers
{
	internal enum CursorType
	{
		PanHandOpen,
		PanHandClosed,
		Arrow
	};

	internal class Cursor
	{
		public Texture2D Sprite { get; private set; }

		public Rectangle Bounds
		{
			get { return Sprite.Bounds; }
		}

		public Cursor( String texture )
		{
			Sprite = Platform.Instance.Content.Load<Texture2D>( texture );
		}
	}

	internal class CursorManager
	{
		private readonly Dictionary<CursorType, Cursor> _cursors = new Dictionary<CursorType, Cursor>();
		private Cursor _activeCursor;
		private Vector2 _cursorPosition;
		private readonly SpriteBatch _tempSpriteBatch;
		private static CursorManager _instance;
		private ITransformable _cursorNode;
		private bool _isVisible = true;
		private bool _hideOnLeave = true;

		// Unimplemented
		//private static bool _constrainToWindow = true;

		public static CursorManager Instance
		{
			get { return _instance ?? ( _instance = new CursorManager() ); }
		}

		private CursorManager()
		{
			Initialize();
			_cursorNode = new SceneNode();
			_tempSpriteBatch = new SpriteBatch( Platform.Instance.Device );
		}

		private void Initialize()
		{
			RegisterCursor( CursorType.PanHandOpen, new Cursor( "images/cursor_hand_open" ) );
			RegisterCursor( CursorType.PanHandClosed, new Cursor( "images/cursor_hand_closed" ) );
			RegisterCursor( CursorType.Arrow, new Cursor( "images/cursor_arrow" ) );
			SetActiveCursor( CursorType.Arrow );
		}

		public void RegisterCursor( CursorType name, Cursor cursor )
		{
			_cursors.Add( name, cursor );
		}

		public void UnregisterCursor( CursorType name )
		{
			_cursors.Remove( name );
		}

		public void SetActiveCursor( CursorType name )
		{
			_cursors.TryGetValue( name, out _activeCursor );
		}

		public void Update()
		{
			var locationEvent = Platform.Instance.InputAggregator.GetEvent<LocationEvent>();
			if ( locationEvent != null )
			{
				_cursorPosition = locationEvent.Location;
			}
		}

		public void Render()
		{
			if ( _isVisible )
			{
				_tempSpriteBatch.Begin( SpriteSortMode.Immediate, BlendState.AlphaBlend, SamplerState.LinearClamp, DepthStencilState.Default, RasterizerState.CullCounterClockwise, null );
				_tempSpriteBatch.Draw( _activeCursor.Sprite, new Rectangle( ( int ) _cursorPosition.X, ( int ) _cursorPosition.Y, 32, 32 ), Color.White );
				_tempSpriteBatch.End();
			}
		}

		private bool IsCursorInsideWindow()
		{
			int screenWidth = Platform.Instance.Game.Window.ClientBounds.Width;
			int screenHeight = Platform.Instance.Game.Window.ClientBounds.Height;
			return _cursorPosition.X >= 0 && _cursorPosition.Y >= 0 && _cursorPosition.X < screenWidth && _cursorPosition.Y < screenHeight;
		}

		private Rectangle GetDestinationRectangle()
		{
			Rectangle destination = _activeCursor.Bounds;
			destination.X += ( int ) _cursorPosition.X;
			destination.Y += ( int ) _cursorPosition.Y;
			return destination;
		}

		public void SetVisible( bool visible )
		{
			_isVisible = visible;
		}

		public void SetConstrainToWindow( bool constrain )
		{
			//_constrainToWindow = constrain;
		}
	}
}
