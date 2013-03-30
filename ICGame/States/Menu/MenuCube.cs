using System;
using System.Collections.Generic;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Audio;
using VertexArmy.Global;
using VertexArmy.Global.Managers;

namespace VertexArmy.States.Menu
{
	public class MenuCube
	{
		private static readonly Random Random = new Random();

		// remove this?
		public MenuCube PreviousMenu { get; set; }

		public List<MenuItem> Items { get; set; }
		public int SelectedItem { get; set; }
		public SoundEffect SelectionSound { get; set; }

		public string Title { get; set; }

		public string Id { get; private set; }

		private const float DropHeight = 50f;
		private const float RotationTime = 200.0f;
		private const float RotationStep = 90.0f;
		private const int InitialRotationMin = -5;
		private const int InitialRotationMax = 5;

		private bool _requestedRotation;
		private bool _isRotating;
		private float _currentRotationTime;
		private Quaternion _previousRotation = Quaternion.Identity;
		private Quaternion _nextRotation = Quaternion.Identity;
		private Quaternion _rotation = Quaternion.Identity;

		public MenuCube()
		{
			Id = Guid.NewGuid().ToString();
		}

		public void Spawn()
		{
			GameWorldManager.Instance.SpawnEntity( "menu_cube", Id, new Vector3( 0f, DropHeight, 0f ) );

			// set a small horizontal rotation to give a better impression
			_rotation = Quaternion.CreateFromAxisAngle( Vector3.UnitY,
				MathHelper.ToRadians( ( float ) ( Random.NextDouble() + Random.Next( InitialRotationMin, InitialRotationMax ) ) ) );
		}

		public void Destroy()
		{
			GameWorldManager.Instance.RemoveEntity( Id );
		}

		public void Update( GameTime gameTime )
		{
			if ( _requestedRotation )
			{
				_currentRotationTime = MathHelper.Clamp( _currentRotationTime + ( float ) gameTime.ElapsedGameTime.TotalMilliseconds, 0, RotationTime );
				_rotation = Quaternion.Slerp( _previousRotation, _nextRotation, _currentRotationTime / RotationTime );

				if ( _currentRotationTime >= RotationTime )
				{
					_requestedRotation = false;
					_isRotating = false;
				}

				GameWorldManager.Instance.GetEntity( Id ).SetRotation( _rotation );
			}
		}

		public void SelectPreviousItem()
		{
			if ( _isRotating || Items.Count == 1 || SelectedItem == 0 )
			{
				return;
			}

			SelectedItem--;
			StartRotation( RotationStep );

			if ( SelectionSound != null )
			{
				Platform.Instance.SoundManager.PlaySound( SelectionSound );
			}
		}

		public void SelectNextItem()
		{
			if ( _isRotating || Items.Count == 1 || SelectedItem >= Items.Count - 1 )
			{
				return;
			}

			SelectedItem++;
			StartRotation( -RotationStep );

			if ( SelectionSound != null )
			{
				Platform.Instance.SoundManager.PlaySound( SelectionSound );
			}
		}

		private void StartRotation( float angle )
		{
			_requestedRotation = true;
			if ( !_isRotating )
			{
				_previousRotation = _rotation;
				_nextRotation = Quaternion.Concatenate( _rotation, Quaternion.CreateFromAxisAngle( Vector3.UnitY, MathHelper.ToRadians( angle ) ) );
				_currentRotationTime = 0;

				_isRotating = true;
			}
		}
	}
}
