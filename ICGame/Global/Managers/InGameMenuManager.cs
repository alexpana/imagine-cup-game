using System;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace VertexArmy.Global.Managers
{
	class InGameMenuManager
	{
		enum MenuItem
		{
			MI_CONTINUE,
			MI_EXIT,
			MI_NONE
		};

		private class MenuMetrics
		{
			private Vector2 _offsetItemContinue = new Vector2( 0, 116 );
			private Vector2 _offsetItemExit = new Vector2( 0, 164 );
			private Vector2 _size = new Vector2( 342, 266 );
			private Vector2 _offset = new Vector2( 274, 192 );

			public MenuMetrics()
			{
				_offset.X = ( Platform.Instance.Device.Viewport.Width - GetSize().X ) / 2;
				_offset.Y = ( Platform.Instance.Device.Viewport.Height - GetSize().Y ) / 2;
			}

			public Vector2 GetItemOffset( MenuItem item )
			{
				switch ( item )
				{
					case MenuItem.MI_CONTINUE:
						return _offsetItemContinue;
					case MenuItem.MI_EXIT:
						return _offsetItemExit;
				}
				return Vector2.Zero;
			}

			public Vector2 GetMenuOffset()
			{
				return _offset;
			}

			public Vector2 GetSize()
			{
				return _size;
			}

			public Vector2 GetItemSize()
			{
				return new Vector2( 342, 48 );
			}
		};

		private MenuMetrics _metrics = new MenuMetrics();

		private String _prefix = "images/menu/metro/";

		private Texture2D _texBackground = null;
		private Texture2D _texTitle = null;
		private Texture2D _texHighlight = null;
		private Texture2D _texItemContinue = null;
		private Texture2D _texItemExit = null;

		private SpriteBatch _spriteBatch;

		private MenuItem _selectedItem = MenuItem.MI_CONTINUE;

		private Action _callbackContinue;
		private Action _callbackExit;

		private Texture2D LoadTexture( String name )
		{
			return Platform.Instance.Content.Load<Texture2D>( _prefix + name );
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
			int x = ( int ) ( _metrics.GetMenuOffset().X + position.X );
			int y = ( int ) ( _metrics.GetMenuOffset().Y + position.Y );
			_spriteBatch.Draw( texture, new Rectangle( x, y, texture.Width, texture.Height ), Color.White );
		}

		public void Render()
		{
			_spriteBatch.Begin( SpriteSortMode.Immediate, BlendState.AlphaBlend, SamplerState.LinearClamp, DepthStencilState.Default, RasterizerState.CullCounterClockwise, null );

			RenderTexture( _texBackground, Vector2.Zero );
			RenderTexture( _texTitle, Vector2.Zero );

			// Render the highlight
			if ( _selectedItem != MenuItem.MI_NONE )
			{
				RenderTexture( _texHighlight, _metrics.GetItemOffset( _selectedItem ) );
			}

			RenderTexture( _texItemContinue, _metrics.GetItemOffset( MenuItem.MI_CONTINUE ) );
			RenderTexture( _texItemExit, _metrics.GetItemOffset( MenuItem.MI_EXIT ) );

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
			Vector2 cursor = Platform.Instance.Input.PointerPosition - _metrics.GetMenuOffset();

			_selectedItem = MenuItem.MI_NONE;

			if ( PointIntersectsMenuItem( cursor, MenuItem.MI_CONTINUE ) )
			{
				_selectedItem = MenuItem.MI_CONTINUE;
			}

			if ( PointIntersectsMenuItem( cursor, MenuItem.MI_EXIT ) )
			{
				_selectedItem = MenuItem.MI_EXIT;
			}
		}

		public void Update( GameTime gameTime )
		{
			CheckSelectedItem();

			if ( _selectedItem == MenuItem.MI_CONTINUE && Platform.Instance.Input.IsLeftPointerFirstTimePressed )
			{
				CallbackContinue();
			}
			if ( _selectedItem == MenuItem.MI_EXIT && Platform.Instance.Input.IsLeftPointerFirstTimePressed )
			{
				CallbackExit();
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
