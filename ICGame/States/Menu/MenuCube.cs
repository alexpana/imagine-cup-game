using System;
using System.Collections.Generic;
using Microsoft.Xna.Framework;
using VertexArmy.GameWorld.Prefabs;
using VertexArmy.GameWorld.Prefabs.Structs;
using VertexArmy.Global.Managers;

namespace VertexArmy.States.Menu
{
	public class MenuCube
	{
		// remove this?
		public MenuCube PreviousMenu { get; set; }

		public List<MenuItem> Items { get; set; }
		public int SelectedItem { get; set; }

		public string Title { get; set; }

		public string Id { get; private set; }
		public PrefabEntity Mesh { get; private set; }

		private bool _requestedRotation;
		private bool _isRotating;
		private float _currentRotationTime;
		private const float RotationTime = 200.0f;
		private const float RotationStep = 90.0f;
		private Quaternion _previousRotation = Quaternion.Identity;
		private Quaternion _nextRotation = Quaternion.Identity;
		private Quaternion _rotation = Quaternion.Identity;

		public MenuCube()
		{
			Id = Guid.NewGuid().ToString();
			InitMesh();
		}

		public void Spawn()
		{
			GameWorldManager.Instance.SpawnEntity( Mesh, Id, new Vector3( 0f, 0, 0f ) );
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
				_rotation = Quaternion.Lerp( _previousRotation, _nextRotation, _currentRotationTime / RotationTime );

				if ( _currentRotationTime >= RotationTime )
				{
					_requestedRotation = false;
					_isRotating = false;
				}
			}

			GameWorldManager.Instance.GetEntity( Id ).SetRotation( _rotation );
		}

		public void SelectPreviousItem()
		{
			if ( _isRotating )
			{
				return;
			}

			if ( SelectedItem == 0 )
			{
				SelectedItem = Items.Count - 1;
			}
			else
			{
				SelectedItem--;
			}

			InitRotation( -RotationStep );
		}

		public void SelectNextItem()
		{
			if ( _isRotating )
			{
				return;
			}

			SelectedItem = ( SelectedItem + 1 ) % Items.Count;
			InitRotation( RotationStep );
		}

		private void InitMesh()
		{
			Mesh = new PrefabEntity();

			MeshSceneNodePrefab crateSceneNode = new MeshSceneNodePrefab
			{
				Material = "CelShadingMaterial",
				Mesh = "models/menu_cube",
				Name = Id + "Mesh",
				LocalRotation = new Quaternion( new Vector3( 0f, 0f, 0f ), 0f )
			};

			Mesh.RegisterMeshSceneNode( crateSceneNode );
		}

		private void InitRotation( float angle )
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
