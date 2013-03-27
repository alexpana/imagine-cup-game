﻿using System;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Input;
using VertexArmy.GameWorld.Prefabs;
using VertexArmy.GameWorld.Prefabs.Structs;
using VertexArmy.Global.Managers;

namespace VertexArmy.States
{
	class GameStateModelViewer : PlayableGameState
	{

		private Quaternion _modelRotation;
		private float _modelScale = 10.0f;

		private bool _dragging = false;
		private int _frames = 0;
		private const int _frameFreeze = 25;
		private Vector2 _lastMousePos;

		public GameStateModelViewer( ContentManager content )
		{
		}

		private void _rotate( float delta, Vector3 axis )
		{
			Vector3 localAxis;
			if ( axis.Equals( new Vector3( 0, 1, 0 ) ) )
			{
				localAxis = Vector3.Transform( axis, Matrix.CreateFromQuaternion( _modelRotation ) );
			}
			else
			{
				localAxis = axis;
			}

			_modelRotation = Quaternion.Concatenate( _modelRotation, new Quaternion(
				localAxis * ( float ) Math.Sin( delta / 2.0f ),
				( float ) Math.Cos( delta / 2.0f ) ) );
		}

		public override void OnUpdate( GameTime gameTime )
		{
			base.OnUpdate( gameTime );

			if ( _frames < _frameFreeze )
			{
				_frames++;
			}
			else if ( _frames == _frameFreeze )
			{
				GameWorldManager.Instance.GetEntity( "mesh1" ).SetPosition( Vector3.Zero );
				GameWorldManager.Instance.GetEntity( "mesh1" ).PhysicsEntity.Enabled = false;
			}


			Vector2 mouseDelta = new Vector2( 0, 0 );

			if ( _dragging )
			{
				switch ( Mouse.GetState().LeftButton )
				{
					case ButtonState.Pressed:
						mouseDelta.X = Mouse.GetState().X - _lastMousePos.X;
						mouseDelta.Y = Mouse.GetState().Y - _lastMousePos.Y;
						_lastMousePos.X = Mouse.GetState().X;
						_lastMousePos.Y = Mouse.GetState().Y;
						break;

					case ButtonState.Released:
						_dragging = false;
						break;
				}
			}

			if ( Mouse.GetState().LeftButton == ButtonState.Pressed && !_dragging )
			{
				_dragging = true;
				_lastMousePos.X = Mouse.GetState().X;
				_lastMousePos.Y = Mouse.GetState().Y;
			}

			int wheel = Mouse.GetState().ScrollWheelValue;
			_modelScale = Mouse.GetState().ScrollWheelValue / 10;

			_rotate( 0.01f * mouseDelta.X, Vector3.UnitY );
			_rotate( 0.01f * mouseDelta.Y, Vector3.UnitX );

			GameWorldManager.Instance.GetEntity( "mesh1" ).SetRotation( _modelRotation );
			GameWorldManager.Instance.GetEntity( "camera1" ).SetPosition(
				new Vector3( 0, 0, 70 - Mouse.GetState().ScrollWheelValue / 4.0f ) );
		}

		public override void OnEnter()
		{
			_modelRotation = Quaternion.Identity;

			SceneManager.Instance.Clear();
			CursorManager.Instance.SetActiveCursor( CursorType.Arrow );
			CursorManager.Instance.SetVisible( true );

			GameWorldManager.Instance.SpawnEntity( "camera", "camera1", new Vector3( 0, 0, 100 ) );


			PrefabEntity mesh = new PrefabEntity();

			MeshSceneNodePrefab crateSceneNode = new MeshSceneNodePrefab
			{
				Material = "CelShadingMaterial",
				Mesh = "models/crate00",
				Name = "Mesh",
				LocalRotation = new Quaternion( new Vector3( 0f, 0f, 0f ), 0f )
			};

			mesh.RegisterMeshSceneNode( crateSceneNode );
			GameWorldManager.Instance.SpawnEntity( "robot", "mesh1", new Vector3( 0f, 0, 0f ) );

		}

		public override void OnClose()
		{
			SceneManager.Instance.Clear();
		}
	}
}
