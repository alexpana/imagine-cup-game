using System;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using UnifiedInputSystem.Events;

namespace VertexArmy.Global.Managers
{
	internal class InGameMenuManager
	{
		private enum MenuItem
		{
			Continue,
			Exit,
			None
		};

		private class MenuMetrics
		{
			private readonly Vector2 _offsetItemContinue = new Vector2( 0, 116 );
			private readonly Vector2 _offsetItemExit = new Vector2( 0, 164 );
			private readonly Vector2 _size = new Vector2( 342, 266 );
			private readonly Vector2 _offset = new Vector2( 274, 192 );

			public MenuMetrics()
			{
				_offset.X = ( Platform.Instance.Device.Viewport.Width - GetSize().X ) / 2;
				_offset.Y = ( Platform.Instance.Device.Viewport.Height - GetSize().Y ) / 2;
			}

			public Vector2 GetItemOffset( MenuItem item )
			{
				switch ( item )
				{
					case MenuItem.Continue:
						return _offsetItemContinue;
					case MenuItem.Exit:
						return _offsetItemExit;
				}
				return Vector2.Zero;
			}

			public Vector2 GetMenuOffset()
			{
				return _offset;
			}

			private Vector2 GetSize()
			{
				return _size;
			}

			public Vector2 GetItemSize()
			{
				return new Vector2( 342, 48 );
			}
		};

		private readonly MenuMetrics _metrics = new MenuMetrics();

		private const String Prefix = "images/menu/metro/";

		private readonly Texture2D _texBackground;
		private readonly Texture2D _texTitle;
		private readonly Texture2D _texHighlight;
		private readonly Texture2D _texItemContinue;
		private readonly Texture2D _texItemExit;

		private readonly SpriteBatch _spriteBatch;

		private MenuItem _selectedItem = MenuItem.Continue;

		private readonly Action _callbackContinue;
		private readonly Action _callbackExit;

		private Texture2D LoadTexture( String name )
		{
			return Platform.Instance.Content.Load<Texture2D>( Prefix + name );
		}

		public InGameMenuManager( Action callbackContine, Action callbackExit )
		{
			_texBackground = LoadTexture( "ig_background" );
			_texTitle = LoadTexture( "ig_title" );
			_texHighlight = LoadTexture( "ig_highlight" );
			_texItemContinue = LoadTexture( "ig_item_continue" );
			_texItemExit = LoadTexture( "ig_item_exit" );

			_spriteBatch = new SpriteBatch( Platform.Instance.Device );

			_callbackContinue = callbackContine;
			_callbackExit = callbackExit;
		}

		private void RenderTexture( Texture2D texture, Vector2 position )
		{
			var x = ( int ) ( _metrics.GetMenuOffset().X + position.X );
			var y = ( int ) ( _metrics.GetMenuOffset().Y + position.Y );
			_spriteBatch.Draw( texture, new Rectangle( x, y, texture.Width, texture.Height ), Color.White );
		}

		public void Render()
		{
			_spriteBatch.Begin( SpriteSortMode.Immediate, BlendState.AlphaBlend, SamplerState.LinearClamp, DepthStencilState.Default, RasterizerState.CullCounterClockwise, null );

			RenderTexture( _texBackground, Vector2.Zero );
			RenderTexture( _texTitle, Vector2.Zero );

			if ( _selectedItem != MenuItem.None )
			{
				RenderTexture( _texHighlight, _metrics.GetItemOffset( _selectedItem ) );
			}

			RenderTexture( _texItemContinue, _metrics.GetItemOffset( MenuItem.Continue ) );
			RenderTexture( _texItemExit, _metrics.GetItemOffset( MenuItem.Exit ) );

			_spriteBatch.End();
		}

		private bool Vector2InRange( Vector2 v, float x, float y, float w, float h )
		{
			return x <= v.X && v.X <= x + w && y <= v.Y && v.Y <= y + h;
		}

		private bool Vector2InRange( Vector2 v, Vector2 position, Vector2 size )
		{
			return Vector2InRange( v, position.X, position.Y, size.X, size.Y );
		}

		private bool PointIntersectsMenuItem( Vector2 point, MenuItem item )
		{
			return Vector2InRange( point, _metrics.GetItemOffset( item ), _metrics.GetItemSize() );
		}

		private void CheckSelectedItem()
		{
			var locationEvent = Platform.Instance.InputAggregator.GetEvent<LocationEvent>();
			Vector2 cursor = locationEvent.Location - _metrics.GetMenuOffset();

			if ( locationEvent.Delta.Length() > 0 )
			{
				if ( PointIntersectsMenuItem( cursor, MenuItem.Continue ) )
				{
					_selectedItem = MenuItem.Continue;
				}

				if ( PointIntersectsMenuItem( cursor, MenuItem.Exit ) )
				{
					_selectedItem = MenuItem.Exit;
				}
			}

			if ( Platform.Instance.Input.IsKeyPressed( Keys.Down, false ) )
			{
				MoveSelectionDown();
			}
			if ( Platform.Instance.Input.IsKeyPressed( Keys.Up, false ) )
			{
				MoveSelectionUp();
			}
		}

		private void MoveSelectionUp()
		{
			_selectedItem = _selectedItem == MenuItem.Continue ? MenuItem.Exit : MenuItem.Continue;
		}

		private void MoveSelectionDown()
		{
			MoveSelectionUp();
		}

		public void Update( GameTime gameTime )
		{
			CheckSelectedItem();

			if ( Platform.Instance.InputAggregator.GetGesture( GestureType.Activate ) != null ||
				Platform.Instance.Input.IsKeyPressed( Keys.Enter, false ) )
			{
				CallbackSelection();
			}
		}

		private void CallbackSelection()
		{
			switch ( _selectedItem )
			{
				case MenuItem.Continue:
					CallbackContinue();
					break;
				case MenuItem.Exit:
					CallbackExit();
					break;
			}
		}

		private void CallbackContinue()
		{
			_callbackContinue();
		}

		private void CallbackExit()
		{
			_callbackExit();
		}
	}
}
