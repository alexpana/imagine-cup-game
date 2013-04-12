using System;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using VertexArmy.Global;
using VertexArmy.Global.Managers;

namespace VertexArmy.States
{
	internal class GameStateRoomPreview : IGameState
	{
		private readonly ContentManager _contentManager;
		private Texture2D _background;
		private SpriteBatch _spriteBatch;
		private Vector2 _cameraPosition = new Vector2( 0, 0 );
		private bool _isDragging;

		public GameStateRoomPreview( ContentManager contentManager )
		{
			_contentManager = contentManager;
			_isDragging = false;
		}

		public void OnUpdate( GameTime gameTime )
		{
			UpdateMouseInput();
			if ( _isDragging )
			{
				MoveCamera( Platform.Instance.Input.PointerDelta );
			}
		}

		private void UpdateMouseInput()
		{
			if ( Platform.Instance.Input.IsLeftPointerPressed && !_isDragging )
			{
				MouseBeginDrag();
			}
			if ( !Platform.Instance.Input.IsLeftPointerPressed && _isDragging )
			{
				MouseEndDrag();
			}
		}

		private void MouseBeginDrag()
		{
			CursorManager.Instance.SetActiveCursor( CursorType.PanHandClosed );
			_isDragging = true;
		}

		private void MouseEndDrag()
		{
			CursorManager.Instance.SetActiveCursor( CursorType.PanHandOpen );
			_isDragging = false;
		}

		private void MoveCamera( Vector2 delta )
		{
			int screenWidth = Platform.Instance.Game.Window.ClientBounds.Width;
			int screenHeight = Platform.Instance.Game.Window.ClientBounds.Height;
			_cameraPosition.X = Math.Min( 0, Math.Max( _cameraPosition.X + delta.X, screenWidth - _background.Width ) );
			_cameraPosition.Y = Math.Min( 0, Math.Max( _cameraPosition.Y + delta.Y, screenHeight - _background.Height ) );
		}

		public void OnRender( GameTime gameTime )
		{
			Platform.Instance.Device.Clear( Color.SkyBlue );
			RenderBackground();
		}

		private void RenderBackground()
		{
			_spriteBatch.Begin();
			_spriteBatch.Draw( _background, GetBackgroundRenderBounds(), Color.White );
			_spriteBatch.End();
		}

		private Rectangle GetBackgroundRenderBounds()
		{
			return new Rectangle(
				( int ) _cameraPosition.X,
				( int ) _cameraPosition.Y,
				_background.Width,
				_background.Height
				);
		}

		public void OnEnter()
		{
			Platform.Instance.Game.Window.Title = "Room Preview";
			Platform.Instance.DeviceManager.PreferredBackBufferWidth = 1024;
			Platform.Instance.DeviceManager.PreferredBackBufferHeight = 600;
			Platform.Instance.DeviceManager.ApplyChanges();

			_background = _contentManager.Load<Texture2D>( @"images\level_sketch" );
			_spriteBatch = new SpriteBatch( Platform.Instance.Device );
			CenterCamera();
		}

		private void CenterCamera()
		{
			_cameraPosition.X = -( ( _background.Bounds.Width - Platform.Instance.Game.Window.ClientBounds.Width ) / 2 );
			_cameraPosition.Y = -( ( _background.Bounds.Height - Platform.Instance.Game.Window.ClientBounds.Height ) / 2 );
		}

		public void OnClose()
		{
			_contentManager.Unload( );
		}
	}
}
