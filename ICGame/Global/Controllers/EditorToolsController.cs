using System.Collections.Generic;
using FarseerPhysics.Dynamics;
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
		private double _moveTime, _rotateTime, _scaleTime;


		private int _clicks;
		private const int _requiredClicks = 2;
		private const double _clickInterval = 500.0;
		private const double _moveDelay = 500.0;
		private const double _rotateDelay = 500.0;
		private const double _scaleDelay = 500.0;
		private bool _leftClick;

		private GameEntity _selectedEntity;
		private GameEntity _tryEntity;
		private GameEntity cursorLocation;

		public EditorToolsController()
		{
			_leftClick = false;
			_state = EditorState.None;
			_clicks = 0;
			_moveTime = -1;
			_rotateTime = -1;
		}

		private void SelectProcess( GameTime dt )
		{
			GameEntity cursorLocation = TrySelectEntity();

			if(cursorLocation != null)
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
				if ( _selectedEntity != null && !_selectedEntity.Equals( _tryEntity ) )
				{
					_state = EditorState.None;
				}

				switch ( _state )
				{
					case EditorState.None:
						_selectedEntity = _tryEntity;
						_state = EditorState.Selected;
						break;
					case EditorState.Selected:
						_state = EditorState.Rotating;
						break;
					case EditorState.Rotating:
						_state = EditorState.Scaling;
						break;
					case EditorState.Scaling:
						_state = EditorState.Selected;
						break;
				}

				_tryEntity = null;
			}
			/*end selection*/

			/*deselection*/
			if ( Mouse.GetState().RightButton.Equals( ButtonState.Pressed ) )
			{
				_selectedEntity = null;
				_state = EditorState.None;
			}
		}

		public void MoveProcess( GameTime dt )
		{
			if ( _selectedEntity != null && _state.Equals( EditorState.Selected ) )
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

		public void RotateProcess( GameTime dt )
		{
			if ( _selectedEntity != null && _state.Equals( EditorState.Rotating ) )
			{
				float rotate = 0f;

				if ( Keyboard.GetState().IsKeyDown( Keys.Left ) )
				{
					rotate -= ( float ) dt.ElapsedGameTime.TotalSeconds / 4f;
				}
				else if ( Keyboard.GetState().IsKeyDown( Keys.Right ) )
				{
					rotate += ( float ) dt.ElapsedGameTime.TotalSeconds / 4f; ;
				}

				if ( rotate != 0 )
				{
					if ( Keyboard.GetState().IsKeyDown( Keys.Q ) )
					{
						rotate *= 5;
					}
					if ( _rotateTime < 0 )
					{
						_rotateTime = dt.TotalGameTime.TotalMilliseconds;
						_selectedEntity.SetRotation( _selectedEntity.GetRotationRadians() + rotate );
					}
					else if ( dt.TotalGameTime.TotalMilliseconds - _rotateTime > _rotateDelay || Keyboard.GetState().IsKeyDown( Keys.Q ) )
					{

						_selectedEntity.SetRotation( _selectedEntity.GetRotationRadians() + rotate );
					}
				}
				else
				{
					_rotateTime = -1;
				}
			}
		}

		public void ScaleProcess( GameTime dt )
		{
			if ( _selectedEntity != null && _state.Equals( EditorState.Scaling ) )
			{
				Vector3 scale = Vector3.Zero;
				if ( Keyboard.GetState().IsKeyDown( Keys.Up ) )
				{
					scale += Vector3.UnitY;
				}
				else if ( Keyboard.GetState().IsKeyDown( Keys.Down ) )
				{
					scale -= Vector3.UnitY;
				}

				if ( Keyboard.GetState().IsKeyDown( Keys.Left ) )
				{
					scale -= Vector3.UnitX;
				}
				else if ( Keyboard.GetState().IsKeyDown( Keys.Right ) )
				{
					scale += Vector3.UnitX;
				}

				if ( !scale.Equals( Vector3.Zero ) )
				{

					if ( Keyboard.GetState().IsKeyDown( Keys.Q ) )
					{
						scale *= 5;
					}
					if ( _scaleTime < 0 )
					{
						_scaleTime = dt.TotalGameTime.TotalMilliseconds;
						SetScale( _selectedEntity.GetScale() + scale );

					}
					else if ( dt.TotalGameTime.TotalMilliseconds - _scaleTime > _scaleDelay || Keyboard.GetState().IsKeyDown( Keys.Q ) )
					{
						SetScale( _selectedEntity.GetScale() + scale );
					}
				}
				else
				{
					_scaleTime = -1;
				}
			}
		}

		private void SetScale( Vector3 newScale )
		{
			if ( newScale.X <= 0 )
			{
				newScale = new Vector3( 1f, newScale.Y, newScale.Z );
			}
			if ( newScale.Y <= 0 )
			{
				newScale = new Vector3( newScale.X, 1f, newScale.Z );
			}
			if ( newScale.Z <= 0 )
			{
				newScale = new Vector3( newScale.X, newScale.Y, 1f );
			}

			Vector3 position = _selectedEntity.GetPosition();
			Quaternion rotation = _selectedEntity.GetRotation();
			Category category = _selectedEntity.PhysicsEntity.GetCollisionLayer();

			GameWorldManager.Instance.RemoveEntity( _selectedEntity.Name );
			GameWorldManager.Instance.SpawnEntity( _selectedEntity.Prefab, _selectedEntity.Name, position, newScale, category );
			_selectedEntity = GameWorldManager.Instance.GetEntity( _selectedEntity.Name );
			_selectedEntity.SetRotation( rotation );

			Platform.Instance.PhysicsWorld.Step( 0f );
		}

		public void Update( GameTime dt )
		{
			cursorLocation = TrySelectEntity();
			HintManager.Instance.SpawnHint( cursorLocation.Name, new Vector2( 100f, 500f ), 100, 6, null, 1 );

			SelectProcess( dt );
			MoveProcess( dt );
			RotateProcess( dt );
			ScaleProcess( dt );

			if ( _selectedEntity != null )
			{
				switch ( _state )
				{
					case EditorState.None:
						_selectedEntity = _tryEntity;
						_state = EditorState.Selected;
						break;
					case EditorState.Selected:
						HintManager.Instance.SpawnHint( "Selected entity:" + _selectedEntity.Name + "\nPosition: " + _selectedEntity.GetPosition(), new Vector2( 1f, 1f ), 50, 5, null, 1 );
						break;
					case EditorState.Rotating:
						HintManager.Instance.SpawnHint( "Rotating entity:" + _selectedEntity.Name + "\nRotation: " + _selectedEntity.GetRotationRadians(), new Vector2( 1f, 1f ), 50, 5, null, 1 );
						break;
					case EditorState.Scaling:
						HintManager.Instance.SpawnHint( "Scaling entity:" + _selectedEntity.Name + "\nRotation: " + _selectedEntity.GetScale(), new Vector2( 1f, 1f ), 50, 5, null, 1 );
						break;
				}

			}
			else
			{
				_state = EditorState.None;
			}
		}

		private GameEntity TrySelectEntity()
		{
			MouseState mouseState = Mouse.GetState();
			int mouseX = mouseState.X;
			int mouseY = mouseState.Y;

			List<SceneNode> lst = SceneManager.Instance.IntersectRayWithSceneNodes( mouseX, mouseY );

			lst.Sort( ( x, y ) => ( int ) ( y.GetAbsolutePosition().Z - x.GetAbsolutePosition().Z ) );

			if ( lst.Count > 0 )
				return GameWorldManager.Instance.GetEntityByMesh( ( MeshAttachable ) lst[0].Attachable[0] );

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
		Selected,
		Rotating,
		Scaling
	}
}
