using System;
using System.Collections.Generic;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Input;
using VertexArmy.GameWorld;
using VertexArmy.Global.Behaviours;
using VertexArmy.Global.Managers;
using VertexArmy.Graphics;
using VertexArmy.Graphics.Attachables;

namespace VertexArmy.Global.Controllers
{
	public class EditorToolsController : IController
	{
		private EditorState _state;
		public EditorState State { get { return _state; } }
		private double _clickTime;
		private double _moveTime;

		private int _clicks;
		private const int _requiredClicks = 2;
		private const double _clickInterval = 500.0;
		private const double _moveDelay = 500.0;
		private bool _leftClick;

		private GameEntity _selectedEntity;
		private GameEntity _tryEntity;

		public EditorToolsController()
		{
			_leftClick = false;
			_state = EditorState.None;
			_clicks = 0;
			_moveTime = -1;
		}

		public void Update( GameTime dt )
		{
			GameEntity cursorLocation = TrySelectEntity();
			HintManager.Instance.SpawnHint( cursorLocation.Name, new Vector2( 100f, 500f ), 500, 6, null, 1 );

			if ( Mouse.GetState().LeftButton.Equals( ButtonState.Pressed ) && !_leftClick )
			{
				_leftClick = true;
				GameEntity select = cursorLocation;
				if ( _tryEntity == null )
				{
					_tryEntity = select;
					_clicks++;
				}
				else if ( _tryEntity != null )
				{
					if ( _tryEntity.Equals( select ) )
					{
						_clicks++;
					}
					else
					{
						_clicks = 0;
						_tryEntity = select;
					}
				}

			}
			else if ( Mouse.GetState().LeftButton.Equals( ButtonState.Released ) && _leftClick )
			{
				_leftClick = false;
				_clickTime = dt.TotalGameTime.TotalMilliseconds;
			}
			else if ( Mouse.GetState().LeftButton.Equals( ButtonState.Released ) && !_leftClick )
			{
				if ( dt.TotalGameTime.TotalMilliseconds - _clickTime > _clickInterval )
				{
					_clicks = 0;
				}
			}

			if ( _clicks == _requiredClicks )
			{
				_clicks = 0;
				_selectedEntity = _tryEntity;
				_tryEntity = null;
			}

			if ( _selectedEntity != null )
			{
				HintManager.Instance.SpawnHint( "Selected Entity:" + _selectedEntity.Name + "\nPosition: " + _selectedEntity.GetPosition(), new Vector2( 1f, 1f ), 500, 5, null, 1 );
			}
			else
			{
				_state = EditorState.None;
			}

			if ( _selectedEntity != null )
			{
				Vector3 move = Vector3.Zero;
				if ( Keyboard.GetState().IsKeyDown( Keys.Up ) )
				{
					move += Vector3.UnitY;
				}
				else if ( Keyboard.GetState().IsKeyDown( Keys.Down ) )
				{
					move -= Vector3.UnitY;
				}

				if ( Keyboard.GetState().IsKeyDown( Keys.Left ) )
				{
					move -= Vector3.UnitX;
				}
				else if ( Keyboard.GetState().IsKeyDown( Keys.Right ) )
				{
					move += Vector3.UnitX;
				}

				if ( !move.Equals( Vector3.Zero ) )
				{
					if ( Keyboard.GetState().IsKeyDown( Keys.Q ) )
					{
						move *= 5;
					}
					if ( _moveTime < 0 )
					{
						_moveTime = dt.TotalGameTime.TotalMilliseconds;
						_selectedEntity.SetPosition( _selectedEntity.GetPosition() + move );
					}
					else if ( dt.TotalGameTime.TotalMilliseconds - _moveTime > _moveDelay || Keyboard.GetState().IsKeyDown( Keys.Q ) )
					{

						_selectedEntity.SetPosition( _selectedEntity.GetPosition() + move );
					}
				}
				else
				{
					_moveTime = -1;
				}
			}
		}

	private GameEntity TrySelectEntity()
		{
			MouseState mouseState = Mouse.GetState();
			int mouseX = mouseState.X;
			int mouseY = mouseState.Y;

			List<SceneNode> lst = SceneManager.Instance.IntersectRayWithSceneNodes(mouseX, mouseY);


			lst.Sort( ( x, y ) => (int)(y.GetAbsolutePosition().Z - x.GetAbsolutePosition().Z) );


			if ( lst.Count > 0 )
				return GameWorldManager.Instance.GetEntityByMesh((MeshAttachable)lst[0].Attachable[0]);

			return null;
		}


		public List<object> Data { get; set; }

		public void Clean()
		{

		}
	}

	public enum EditorState
	{
		None,
		Rotating
	}
}
