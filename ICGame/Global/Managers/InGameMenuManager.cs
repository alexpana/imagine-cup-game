using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using VertexArmy.Global.Behaviours;
using Microsoft.Xna.Framework.Graphics;

namespace VertexArmy.Global.Managers
{
	class InGameMenuManager : IUpdatable
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

			public Vector2 GetOffset( MenuItem item )
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

			public Vector2 GetSize( )
			{
				return _size;
			}

			public Vector2 GetItemSize( )
			{
				return new Vector2( 342, 48 );
			}
		};

		private MenuMetrics _metrics = new MenuMetrics( );

		private String _prefix = "images/menu/metro/";

		private Texture2D _texBackground = null;
		private Texture2D _texTitle = null;
		private Texture2D _texHighlight = null;
		private Texture2D _texItemContinue = null;
		private Texture2D _texItemExit = null;

		private Vector2 _offset = new Vector2( 274, 192 );

		private SpriteBatch _spriteBatch;

		private MenuItem _selectedItem = MenuItem.MI_CONTINUE;

		private Texture2D LoadTexture( String name )
		{
			return Platform.Instance.Content.Load<Texture2D>( _prefix + name );
		}

		public InGameMenuManager()
		{
			_texBackground = LoadTexture( "ig_background" );
			_texTitle = LoadTexture( "ig_title" );
			_texHighlight = LoadTexture( "ig_highlight" );
			_texItemContinue = LoadTexture( "ig_item_continue" );
			_texItemExit = LoadTexture( "ig_item_exit" );

			_spriteBatch = new SpriteBatch( Platform.Instance.Device );
			_offset.X = ( Platform.Instance.Device.Viewport.Width - _metrics.GetSize( ).X ) / 2;
			_offset.Y = ( Platform.Instance.Device.Viewport.Height - _metrics.GetSize( ).Y ) / 2;

		}

		private void RenderTexture( Texture2D texture, Vector2 position )
		{
			int x = ( int )( _offset.X + position.X);
			int y = ( int )( _offset.Y + position.Y);
			_spriteBatch.Draw( texture, new Rectangle( x, y, texture.Width, texture.Height ), Color.White );
		}

		public void Render( )
		{
			_spriteBatch.Begin( SpriteSortMode.Immediate, BlendState.AlphaBlend, SamplerState.LinearClamp, DepthStencilState.Default, RasterizerState.CullCounterClockwise, null );

			RenderTexture( _texBackground, Vector2.Zero );
			RenderTexture( _texTitle, Vector2.Zero );
			
			// Render the highlight
			if ( _selectedItem != MenuItem.MI_NONE )
			{
				RenderTexture( _texHighlight, _metrics.GetOffset( _selectedItem ) );
			}

			RenderTexture( _texItemContinue, _metrics.GetOffset( MenuItem.MI_CONTINUE ) );
			RenderTexture( _texItemExit, _metrics.GetOffset( MenuItem.MI_EXIT ) );

			_spriteBatch.End();
		}

		private bool Vector2InRange( Vector2 v, int x, int y, int w, int h )
		{
			return x <= v.X && v.X <= x + w && y <= v.Y && v.Y <= y + h;
		}

		private bool Vector2InRange( Vector2 v, Vector2 position, Vector2 size )
		{
			return Vector2InRange( v, position.X, position.Y, size.X, size.Y );
		}

		private bool PointIntersectsMenuItem( Vector2 point, MenuItem item )
		{
			return Vector2InRange( cursor, _metrics.GetOffset( item ), _metrics.GetItemSize( item );
		}

		private void CheckSelectedItem()
		{
			Vector2 cursor = Platform.Instance.Input.PointerPosition;

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
				CallbackContinue( );
			}
			if ( _selectedItem == MenuItem.MI_EXIT && Platform.Instance.Input.IsLeftPointerFirstTimePressed )
			{
				CallbackExit( );
			}
		}

		private void CallbackContinue()
		{
		}

		private void CallbackExit()
		{
		
		}
	}
}
