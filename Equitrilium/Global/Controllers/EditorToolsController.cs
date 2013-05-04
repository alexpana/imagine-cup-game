using System;
using System.Collections.Generic;
using FarseerPhysics.Dynamics;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Input;
using UnifiedInputSystem;
using UnifiedInputSystem.Extensions;
using UnifiedInputSystem.Input;
using VertexArmy.GameWorld;
using VertexArmy.Global.Behaviours;
using VertexArmy.Global.Managers;
using VertexArmy.Graphics;
using VertexArmy.Graphics.Attachables;
using VertexArmy.Input;
using VertexArmy.Utilities;

namespace VertexArmy.Global.Controllers
{
	public class EditorToolsController : IController
	{
		private EditorState _state;

		public EditorState State
		{
			get { return _state; }
		}

		private double _clickTime;
		private double _moveTime, _rotateTime, _scaleTime, _externalRotateTime;


		private int _clicks;
		private const int _requiredClicks = 2;
		private const double _clickInterval = 500.0;
		private const double _moveDelay = 500.0;
		private const double _rotateDelay = 500.0;
		private const double _externalRotateDelay = 500.0;
		private const double _scaleDelay = 500.0;
		private bool _leftClick;

		private bool _dragging;
		private Vector3 _relative;
		private int _selectedPrefab;
		private readonly List<string> _prefabs;

		private GameEntity _selectedEntity;
		private GameEntity _tryEntity;
		private GameEntity cursorLocation;
		private Category _lastLayerSelected;
		private float _lastSelectedZ;
		private readonly float _specialRotationBigOperator;
		private readonly float _specialRotationSmallOperator;

		private readonly IInputSystem _inputSystem;
		private readonly InputAggregator _inputAggregator;
		private const float OperationSmallIncrement = 0.1f;
		private const float OperationBigIncrement = 2f;

		public EditorToolsController()
		{
			_leftClick = false;
			_dragging = false;
			_state = EditorState.None;
			_clicks = 0;
			_moveTime = -1;
			_rotateTime = -1;
			_externalRotateTime = -1;
			_prefabs = new List<string>( PrefabRepository.Instance.PrefabNames );
			_selectedPrefab = 0;
			_lastLayerSelected = Category.Cat1;
			_lastSelectedZ = 0f;
			_specialRotationBigOperator = ( float ) ( Math.PI / 6f );
			_specialRotationSmallOperator = ( float ) ( Math.PI / 18f );

			_inputAggregator = Platform.Instance.InputAggregator;
			_inputSystem = Platform.Instance.Input;
		}

		private void SelectProcess( GameTime dt )
		{
			cursorLocation = TrySelectEntity();

			if ( cursorLocation != null )
			{
				HintManager.Instance.SpawnHint( cursorLocation.Name, new Vector2( 20f, 728f ), 500, 6, null, 1 );
			}

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

				if ( _state.Equals( EditorState.Selected ) && cursorLocation != null && cursorLocation.Equals( _selectedEntity ) )
				{
					Vector3 m3D = SceneManager.Instance.IntersectScreenRayWithPlane( _selectedEntity.GetPosition().Z );
					_dragging = true;
					_relative = new Vector3( m3D.X, m3D.Y, _selectedEntity.GetPosition().Z ) - _selectedEntity.GetPosition();
				}
			}
			else if ( Mouse.GetState().LeftButton.Equals( ButtonState.Released ) && _leftClick )
			{
				_leftClick = false;
				_clickTime = dt.TotalGameTime.TotalMilliseconds;

				_dragging = false;
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

		public void ShowInfoProcess( GameTime dt )
		{
			if ( Keyboard.GetState().IsKeyDown( Keys.V ) )
			{
				Vector3 position = SceneManager.Instance.IntersectScreenRayWithPlane( 0 );
				HintManager.Instance.SpawnHint( "Cursor 3D position: " + position.ToString(), new Vector2( 200f, 20f ), 500, 6, null, 1 );
			}

			//if( dt.ElapsedGameTime.TotalMilliseconds )
		}

		public void MoveProcess( GameTime dt )
		{
			if ( _selectedEntity != null && _dragging )
			{
				Vector3 m3D = SceneManager.Instance.IntersectScreenRayWithPlane( _selectedEntity.GetPosition().Z );
				Vector3 newPosition = new Vector3( m3D.X, m3D.Y, _selectedEntity.GetPosition().Z ) - _relative;


				if ( !newPosition.Equals( _selectedEntity.GetPosition() ) )
				{
					if ( _inputAggregator.HasEvent( Button.LeftControl ) || _inputAggregator.HasEvent( Button.RightControl ) )
					{
						newPosition = TransformUtility.SnapToGridXY( newPosition, OperationBigIncrement );
					}
					else if ( _inputAggregator.HasEvent( Button.LeftShift ) || _inputAggregator.HasEvent( Button.RightShift ) )
					{
						newPosition = TransformUtility.SnapToGridXY( newPosition, OperationSmallIncrement );
					}
					else
					{
						newPosition = TransformUtility.SnapToGridXY( newPosition, 1f );
					}
					_selectedEntity.SetPosition( newPosition );
				}
			}

			if ( _selectedEntity != null && _state.Equals( EditorState.Selected ) )
			{
				Vector3 move = Vector3.Zero;
				float snapStep = 1f;

				if ( _inputAggregator.HasEvent( Button.Up ) )
				{
					move += Vector3.UnitY;
				}
				else if ( _inputAggregator.HasEvent( Button.Down ) )
				{
					move -= Vector3.UnitY;
				}

				if ( _inputAggregator.HasEvent( Button.Left ) )
				{
					move -= Vector3.UnitX;
				}
				else if ( _inputAggregator.HasEvent( Button.Right ) )
				{
					move += Vector3.UnitX;
				}

				if ( _inputAggregator.HasEvent( Button.Minus ) )
				{
					move -= Vector3.UnitZ;
				}
				else if ( _inputAggregator.HasEvent( Button.Plus ) )
				{
					move += Vector3.UnitZ;
				}

				if ( _inputAggregator.HasEvent( Button.LeftControl ) || _inputAggregator.HasEvent( Button.RightControl ) )
				{
					move *= OperationBigIncrement;
					snapStep = OperationBigIncrement;
				}
				else if ( _inputAggregator.HasEvent( Button.LeftShift ) || _inputAggregator.HasEvent( Button.RightShift ) )
				{
					move *= OperationSmallIncrement;
					snapStep = OperationSmallIncrement;
				}

				if ( !move.Equals( Vector3.Zero ) )
				{
					if ( _inputAggregator.HasEvent( Button.Q ) )
					{
						move *= 5;
					}
					if ( _moveTime < 0 )
					{
						_moveTime = dt.TotalGameTime.TotalMilliseconds;
						_selectedEntity.SetPosition( TransformUtility.SnapToGridXY( _selectedEntity.GetPosition() + move, snapStep ) );
						_lastSelectedZ = _selectedEntity.GetPosition().Z;
					}
					else if ( dt.TotalGameTime.TotalMilliseconds - _moveTime > _moveDelay || Keyboard.GetState().IsKeyDown( Keys.Q ) )
					{
						_selectedEntity.SetPosition( TransformUtility.SnapToGridXY( _selectedEntity.GetPosition() + move, snapStep ) );
						_lastSelectedZ = _selectedEntity.GetPosition().Z;
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

				Quaternion externalRotation = Quaternion.Identity;

				if ( _inputAggregator.HasEvent( Button.Left ) )
				{
					rotate -= ( float ) dt.ElapsedGameTime.TotalSeconds / 4f;
				}
				else if ( _inputAggregator.HasEvent( Button.Right ) )
				{
					rotate += ( float ) dt.ElapsedGameTime.TotalSeconds / 4f;
					;
				}

				/* TODO
				if ( Keyboard.GetState().IsKeyDown( Buttons.I ) )
				{
					externalRotation = Quaternion.Slerp( _selectedEntity.GetExternalRotation(), Quaternion.CreateFromAxisAngle( Vector3.UnitY, 0f ), ( float ) ( dt.ElapsedGameTime.TotalMilliseconds / 120f ) );
				}
				else if ( Keyboard.GetState().IsKeyDown( Buttons.O ) )
				{
					externalRotation = Quaternion.Slerp( _selectedEntity.GetExternalRotation(), Quaternion.CreateFromAxisAngle( Vector3.UnitY, 84.78f ), ( float ) ( dt.ElapsedGameTime.TotalMilliseconds / 120f ) );
				}
				 */

				if ( _inputAggregator.HasEvent( Button.LeftControl ) || _inputAggregator.HasEvent( Button.RightControl ) )
				{
					rotate = Math.Sign( rotate ) * _specialRotationBigOperator;
				}
				else if ( _inputAggregator.HasEvent( Button.LeftShift ) || _inputAggregator.HasEvent( Button.RightShift ) )
				{
					rotate = Math.Sign( rotate ) * _specialRotationSmallOperator;
				}

				if ( rotate != 0 )
				{
					if ( _inputAggregator.HasEvent( Button.Q ) )
					{
						rotate *= 5;
					}
					if ( _rotateTime < 0 )
					{
						float lastRotation = _selectedEntity.GetRotationRadians();
						float newRotation = lastRotation + rotate;

						if ( _inputAggregator.HasEvent( Button.LeftControl ) || _inputAggregator.HasEvent( Button.RightControl ) )
						{
							newRotation = _specialRotationBigOperator * ( float ) Math.Round( newRotation / _specialRotationBigOperator );
						}
						else if ( _inputAggregator.HasEvent( Button.LeftShift ) || _inputAggregator.HasEvent( Button.RightShift ) )
						{
							newRotation = _specialRotationSmallOperator * ( float ) Math.Round( newRotation / _specialRotationSmallOperator );
						}

						_rotateTime = dt.TotalGameTime.TotalMilliseconds;
						_selectedEntity.SetRotation( newRotation );
					}
					else if ( dt.TotalGameTime.TotalMilliseconds - _rotateTime > _rotateDelay || _inputAggregator.HasEvent( Button.Q ) )
					{
						_selectedEntity.SetRotation( _selectedEntity.GetRotationRadians() + rotate );
					}
				}
				else
				{
					_rotateTime = -1;
				}

				if ( !externalRotation.Equals( Quaternion.Identity ) )
				{
					externalRotation.Normalize();
					if ( _externalRotateTime < 0 )
					{
						_externalRotateTime = dt.TotalGameTime.TotalMilliseconds;
						_selectedEntity.SetExternalRotation( externalRotation );
					}
					else if ( dt.TotalGameTime.TotalMilliseconds - _externalRotateTime > _externalRotateDelay || _inputAggregator.HasEvent( Button.Q ) )
					{
						_selectedEntity.SetExternalRotation( externalRotation );
					}
				}
				else
				{
					_externalRotateTime = -1;
				}
			}
		}

		public void ScaleProcess( GameTime dt )
		{
			if ( _selectedEntity != null && _state.Equals( EditorState.Scaling ) )
			{
				Vector3 scale = Vector3.Zero;

				if ( _inputAggregator.HasEvent( Button.Up ) )
				{
					scale += Vector3.UnitY / 2;
				}
				else if ( _inputAggregator.HasEvent( Button.Down ) )
				{
					scale -= Vector3.UnitY / 2;
				}

				if ( _inputAggregator.HasEvent( Button.Left ) )
				{
					scale -= Vector3.UnitX / 2;
				}
				else if ( _inputAggregator.HasEvent( Button.Right ) )
				{
					scale += Vector3.UnitX / 2;
				}

				if ( _inputAggregator.HasEvent( Button.Minus ) )
				{
					scale -= Vector3.UnitZ / 2;
				}
				else if ( _inputAggregator.HasEvent( Button.Plus ) )
				{
					scale += Vector3.UnitZ / 2;
				}

				if ( _inputAggregator.HasEvent( Button.LeftControl ) || _inputAggregator.HasEvent( Button.RightControl ) )
				{
					scale *= OperationBigIncrement;
				}
				else if ( _inputAggregator.HasEvent( Button.LeftShift ) || _inputAggregator.HasEvent( Button.RightShift ) )
				{
					scale *= OperationSmallIncrement;
				}

				if ( !scale.Equals( Vector3.Zero ) )
				{
					if ( _scaleTime < 0 )
					{
						_scaleTime = dt.TotalGameTime.TotalMilliseconds;
						SetScale( _selectedEntity.GetScale() + scale );
					}
					else if ( dt.TotalGameTime.TotalMilliseconds - _scaleTime > _scaleDelay )
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
			Quaternion externalRotation = _selectedEntity.GetExternalRotation();

			GameWorldManager.Instance.RemoveEntity( _selectedEntity.Name );
			GameWorldManager.Instance.SpawnEntity( _selectedEntity.Prefab, _selectedEntity.Name, position, newScale, category );
			_selectedEntity = GameWorldManager.Instance.GetEntity( _selectedEntity.Name );
			_selectedEntity.SetRotation( rotation );
			_selectedEntity.SetExternalRotation( externalRotation );

			Platform.Instance.PhysicsWorld.Step( 0f );
		}

		public void SpawnProcess( GameTime dt )
		{
			if ( _state.Equals( EditorState.None ) && Keyboard.GetState().IsKeyDown( Keys.C ) )
			{
				int scrollDelta = _inputSystem.ScrollDelta;
				if ( scrollDelta != 0 )
				{
					double scrollDirection = scrollDelta / 120.0;
					int direction;
					if ( scrollDirection < 0 )
					{
						direction = ( int ) Math.Floor( scrollDirection );
					}
					else
					{
						direction = ( int ) Math.Ceiling( scrollDirection );
					}

					_selectedPrefab += direction;
					while ( _selectedPrefab < 0 )
					{
						_selectedPrefab += _prefabs.Count;
					}
					_selectedPrefab = _selectedPrefab % _prefabs.Count;
				}


				HintManager.Instance.SpawnHint( "Spawning entity:" + _prefabs[_selectedPrefab], new Vector2( 20f, 16f ), 50, 5, null, 1 );

				if ( Mouse.GetState().LeftButton.Equals( ButtonState.Pressed ) )
				{
					string generatedName = GenerateEntityName( _prefabs[_selectedPrefab] );

					Vector3 m3D = SceneManager.Instance.IntersectScreenRayWithPlane( _lastSelectedZ );
					Vector3 position = new Vector3( m3D.X, m3D.Y, _lastSelectedZ );

					if ( _inputAggregator.HasEvent( Button.LeftControl ) || _inputAggregator.HasEvent( Button.RightControl ) )
					{
						position = TransformUtility.SnapToGridXY( position, OperationBigIncrement );
					}
					else if ( _inputAggregator.HasEvent( Button.LeftShift ) || _inputAggregator.HasEvent( Button.RightShift ) )
					{
						position = TransformUtility.SnapToGridXY( position, OperationSmallIncrement );
					}
					else
					{
						position = TransformUtility.SnapToGridXY( position, 1f );
					}

					GameWorldManager.Instance.SpawnEntity( _prefabs[_selectedPrefab], generatedName, position, 1f, _lastLayerSelected );
					_state = EditorState.Selected;
					_selectedEntity = GameWorldManager.Instance.GetEntity( generatedName );
					Platform.Instance.PhysicsWorld.Step( 0f );
				}
			}
		}

		public void SetCategoryProcess( GameTime dt )
		{
			if ( _state.Equals( EditorState.Selected ) && _selectedEntity != null )
			{
				if ( _inputAggregator.HasEvent( Button.D1 ) )
				{
					_lastLayerSelected = Category.Cat1;
					_selectedEntity.PhysicsEntity.SetCollisionLayer( Category.Cat1 );
				}

				if ( _inputAggregator.HasEvent( Button.D2 ) )
				{
					_lastLayerSelected = Category.Cat2;
					_selectedEntity.PhysicsEntity.SetCollisionLayer( Category.Cat2 );
				}

				if ( _inputAggregator.HasEvent( Button.D3 ) )
				{
					_lastLayerSelected = Category.Cat3;
					_selectedEntity.PhysicsEntity.SetCollisionLayer( Category.Cat3 );
				}

				if ( _inputAggregator.HasEvent( Button.D4 ) )
				{
					_lastLayerSelected = Category.Cat4;
					_selectedEntity.PhysicsEntity.SetCollisionLayer( Category.Cat4 );
				}

				if ( _inputAggregator.HasEvent( Button.D5 ) )
				{
					_lastLayerSelected = Category.Cat5;
					_selectedEntity.PhysicsEntity.SetCollisionLayer( Category.Cat5 );
				}
			}
		}

		public void Update( GameTime dt )
		{
			cursorLocation = TrySelectEntity();

			if ( cursorLocation != null )
				HintManager.Instance.SpawnHint( cursorLocation.Name, new Vector2( 100f, 668f ), 100, 6, null, 1 );

			SelectProcess( dt );
			MoveProcess( dt );
			RotateProcess( dt );
			ScaleProcess( dt );
			SpawnProcess( dt );
			SetCategoryProcess( dt );
			ShowInfoProcess( dt );

			if ( _state.Equals( EditorState.Selected ) && _inputAggregator.HasEvent( Button.Delete ) )
			{
				GameWorldManager.Instance.RemoveEntity( _selectedEntity.Name );
				_state = EditorState.None;
				_selectedEntity = null;
				Platform.Instance.PhysicsWorld.Step( 0f );
			}

			if ( _selectedEntity != null )
			{
				Vector2 offset = new Vector2( 20, 16 );
				switch ( _state )
				{
					case EditorState.None:
						_selectedEntity = _tryEntity;
						_state = EditorState.Selected;
						break;
					case EditorState.Selected:
						HintManager.Instance.SpawnHint( "Selected entity:" + _selectedEntity.Name + "\nPosition: " + _selectedEntity.GetPosition() + "\nCollision Layer: " + _selectedEntity.PhysicsEntity.GetCollisionLayer(), offset, 50, 5, null, 1 );
						break;
					case EditorState.Rotating:
						HintManager.Instance.SpawnHint( "Rotating entity:" + _selectedEntity.Name + "\nRotation: " + _selectedEntity.GetRotationRadians() + " Ext: " + _selectedEntity.GetExternalRotation(), offset, 50, 5, null, 1 );
						break;
					case EditorState.Scaling:
						HintManager.Instance.SpawnHint( "Scaling entity:" + _selectedEntity.Name + "\nRotation: " + _selectedEntity.GetScale(), offset, 50, 5, null, 1 );
						break;
				}
			}
			else
			{
				_state = EditorState.None;
			}

			HighlightSelectedEntity();
		}

		private void HighlightSelectedEntity()
		{
			if ( _selectedEntity != null )
			{
				if ( !_state.Equals( EditorState.None ) )
				{
					foreach ( KeyValuePair<string, SceneNode> keyValuePair in _selectedEntity.SceneNodes )
					{
						SceneNode node = keyValuePair.Value;

						foreach ( Attachable attachable in node.Attachable )
						{
							MeshAttachable ma = attachable as MeshAttachable;

							if ( ma != null )
							{
								ma.Highlighted = true;
								ma.HighColor = Vector3.UnitY;
							}
						}
					}
				}
			}
		}

		private GameEntity TrySelectEntity()
		{
			MouseState mouseState = Mouse.GetState();
			int mouseX = mouseState.X;
			int mouseY = mouseState.Y;

			List<SceneNode> lst = SceneManager.Instance.IntersectScreenRayWithSceneNodes( mouseX, mouseY );

			if ( lst.Count == 0 )
				return null;

			SceneNode max = lst[0];
			foreach ( SceneNode sceneNode in lst )
			{
				if ( max.GetAbsolutePosition().Z < sceneNode.GetAbsolutePosition().Z )
					max = sceneNode;
			}

			( ( MeshAttachable ) max.Attachable[0] ).Highlighted = true;
			( ( MeshAttachable ) max.Attachable[0] ).HighColor = Vector3.One;
			return GameWorldManager.Instance.GetEntityByMesh( ( MeshAttachable ) max.Attachable[0] );
		}

		private string GenerateEntityName( string prefab )
		{
			int i = 0;
			string entityName = prefab.ToLower();
			while ( GameWorldManager.Instance.GetEntity( entityName + i ) != null )
			{
				i++;
			}

			return entityName + i;
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
