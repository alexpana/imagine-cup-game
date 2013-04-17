using System;
using System.Collections.Generic;
using System.Linq;
using FarseerPhysics.Dynamics;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Audio;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using VertexArmy.Content.Materials;
using VertexArmy.Content.Prefabs;
using VertexArmy.GameWorld;
using VertexArmy.Global;
using VertexArmy.Global.Managers;
using VertexArmy.Graphics.Attachables;

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

		private string _textImage;
		private bool _requestedRotation;
		private bool _isRotating;
		private float _currentRotationTime;
		private Quaternion _previousRotation = Quaternion.Identity;
		private Quaternion _nextRotation = Quaternion.Identity;
		private Quaternion _rotation = Quaternion.Identity;

		private bool _isSpawned;
		private readonly ContentManager _contentManager;

		public MenuCube( ContentManager contentManager )
		{
			Id = Guid.NewGuid().ToString();
			_contentManager = contentManager;
		}

		public void Spawn( float spawnX )
		{
			if ( _textImage == null )
			{
				throw new InvalidOperationException( "No text image specified!" );
			}

			_isSpawned = true;
			var parameters = new GameEntityParameters
			{
				SceneNodeParameters = new Dictionary<string, object>
				{
					{ MenuCubeMaterial.MenuTextImage, _textImage }
				}
			};

			GameWorldManager.Instance.SpawnEntity( MenuCubePrefab.PrefabName, Id, new Vector3( spawnX, DropHeight, 0f ), 1f, Category.Cat1, parameters );
			GameWorldManager.Instance.GetEntity( Id ).SetRotation( MathHelper.Clamp( ( float ) ( 0.017f - Random.NextDouble() ), -0.017f, 0.017f ) );

			// set a small horizontal rotation to give a better impression
			_rotation = Quaternion.CreateFromAxisAngle( Vector3.UnitY,
				MathHelper.ToRadians( ( float ) ( Random.NextDouble() + Random.Next( InitialRotationMin, InitialRotationMax ) ) ) );
		}

		public void Destroy()
		{
			_isSpawned = false;
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

				GameWorldManager.Instance.GetEntity( Id ).SetExternalRotation( _rotation );
			}
		}

		public void SetTextImage( string backgroundImage )
		{
			_textImage = backgroundImage;

			if ( _isSpawned )
			{
				var mainNode = GameWorldManager.Instance.GetEntity( Id ).MainNode;
				var material = mainNode.Children.SelectMany( n => n.Attachable ).OfType<MeshAttachable>().Select( m => m.Material ).FirstOrDefault();
				if ( material != null )
				{
					var texture = _contentManager.Load<Texture2D>( "images/menu/" + backgroundImage );
					material.SetParameter( MenuCubeMaterial.MenuTextImage, texture );
				}
			}
		}

		public void SelectPreviousItem()
		{
			if ( _isRotating || Items.Count == 1 ||
			     ( SelectedItem == 0 && Items.Count < 4 ) )
			{
				return;
			}

			SelectedItem = SelectedItem == 0 ? Items.Count - 1 : SelectedItem - 1;
			StartRotation( RotationStep );

			if ( SelectionSound != null )
			{
				Platform.Instance.SoundManager.PlaySound( SelectionSound );
			}
		}

		public void SelectNextItem()
		{
			if ( _isRotating || Items.Count == 1 ||
			     ( SelectedItem >= Items.Count - 1 && Items.Count < 4 ) )
			{
				return;
			}

			SelectedItem = ( SelectedItem + 1 ) % Items.Count;
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
