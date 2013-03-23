using System;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Input;
using VertexArmy.GameWorld.Prefabs;
using VertexArmy.GameWorld.Prefabs.Structs;
using VertexArmy.Global;
using VertexArmy.Global.Managers;
using VertexArmy.Graphics;

namespace VertexArmy.States
{
	class GameStateModelViewer : PlayableGameState
	{

		private Quaternion _modelRotation;
		private float _modelScale = 10.0f;

		private bool _dragging = false;
		private Vector2 _lastMousePos;

		private ContentManager _cm;
		private SceneManager _sceneManager;
		public GameStateModelViewer( ContentManager content )
		{
			_cm = content;
		}

		private void _rotate( float delta, Vector3 axis )
		{
			_modelRotation *= new Quaternion( axis * ( float ) Math.Sin( 0.01f * delta ),
				( float ) Math.Cos( 0.01f * delta ) );
		}

		public override void OnUpdate( GameTime gameTime )
		{
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

			_rotate( mouseDelta.X, Vector3.UnitY );
			_rotate( -mouseDelta.Y, Vector3.UnitX );

			GameWorldManager.Instance.GetEntity( "mesh1" ).SetRotation( _modelRotation );
			GameWorldManager.Instance.GetEntity( "camera1" ).SetPosition(
				new Vector3( 0, 0, -100 + Mouse.GetState().ScrollWheelValue / 4.0f ) );
		}

		public override void OnRender( GameTime gameTime )
		{
		}

		public override void OnEnter()
		{
			_modelRotation = Quaternion.Identity;

			SceneManager.Instance.Clear();
			CursorManager.Instance.SetActiveCursor( CursorType.Arrow );
			CursorManager.Instance.SetVisible( true );

			GameWorldManager.Instance.SpawnEntity( "camera", "camera1", new Vector3( 0, 0, -100 ) );


			PrefabEntity mesh = new PrefabEntity();

			MeshSceneNodePrefab crateSceneNode = new MeshSceneNodePrefab
			{
				Material = "RobotMaterial",
				Mesh = "models/crate00",
				Name = "Mesh",
				LocalRotation = new Quaternion( new Vector3( 0f, 0f, 0f ), 0f )
			};

			crateSceneNode.LocalRotation.Normalize();

			mesh.RegisterMeshSceneNode( crateSceneNode );
			GameWorldManager.Instance.SpawnEntity( mesh, "mesh1", new Vector3( 0f, 0, 0f ) );
			GameWorldManager.Instance.GetEntity( "mesh1" ).SetScale( new Vector3( 10, 10, 10 ) );
		}

		public override void OnClose()
		{
			SceneManager.Instance.Clear();
		}
	}
}
