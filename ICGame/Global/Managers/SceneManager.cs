using System;
using System.Collections.Generic;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using VertexArmy.Graphics;

namespace VertexArmy.Global.Managers
{
	public class SceneManager
	{
		private static volatile SceneManager _instance;
		private static readonly object _syncRoot = new Object();

		private readonly List<SceneNode> _registeredNodes = new List<SceneNode>();

		private readonly List<CameraAttachable> _sceneCameras = new List<CameraAttachable>();
		private readonly List<LightAttachable> _sceneLights = new List<LightAttachable>();

		private Plane _zeroZPlane;

		public static SceneManager Instance
		{
			get
			{
				if ( _instance == null )
				{
					lock ( _syncRoot )
					{
						if ( _instance == null )
							_instance = new SceneManager();
					}
				}
				return _instance;
			}
		}

		public SceneManager()
		{
			_zeroZPlane = new Plane( Vector3.UnitZ, 1000f );
		}

		public void Clear()
		{
			_registeredNodes.Clear();
			_sceneCameras.Clear();
			_sceneLights.Clear();
		}

		public void RegisterSceneTree( SceneNode node )
		{

			Queue<SceneNode> knodes = new Queue<SceneNode>();

			knodes.Enqueue( node );

			while ( knodes.Count != 0 )
			{
				SceneNode head = knodes.Dequeue();

				/* protect against multiple register */
				if ( _registeredNodes.Contains( head ) )
					continue;

				_registeredNodes.Add( head );

				foreach ( var child in head.Children )
				{
					knodes.Enqueue( child );
				}

				foreach ( var attachable in head.Attachable )
				{
					LightAttachable lightAttachable = attachable as LightAttachable;
					if ( lightAttachable != null )
						_sceneLights.Add( lightAttachable );

					CameraAttachable cameraAttachable = attachable as CameraAttachable;
					if ( cameraAttachable != null )
						_sceneCameras.Add( cameraAttachable );
				}
			}
		}

		public void UnregisterSceneTree( SceneNode node )
		{
			Queue<SceneNode> knodes = new Queue<SceneNode>();

			knodes.Enqueue( node );

			while ( knodes.Count != 0 )
			{
				SceneNode head = knodes.Dequeue();

				_registeredNodes.Remove( head );

				foreach ( var child in head.Children )
				{
					knodes.Enqueue( child );
				}

				foreach ( var attachable in head.Attachable )
				{
					LightAttachable lightAttachable = attachable as LightAttachable;
					if ( lightAttachable != null )
						_sceneLights.Remove( lightAttachable );

					CameraAttachable cameraAttachable = attachable as CameraAttachable;
					if ( cameraAttachable != null )
						_sceneCameras.Remove( cameraAttachable );
				}
			}
		}

		public CameraAttachable GetCurrentCamera()
		{
			return _sceneCameras[0];
		}

		public void Render( float dt )
		{
			if ( _sceneCameras.Count == 0 )
				return;

			Platform.Instance.Device.BlendState = new BlendState();


			CameraAttachable currentCam = _sceneCameras[0];

			Matrix view = currentCam.GetViewMatrix();
			Matrix projection = currentCam.GetPerspectiveMatrix();

			Renderer.Instance.LoadMatrix( EMatrix.Projection, projection );
			Renderer.Instance.LoadMatrix( EMatrix.View, view );

			MouseState mouseState = Mouse.GetState();
			int mouseX = mouseState.X;
			int mouseY = mouseState.Y;

			Vector3 nearsource = new Vector3( ( float ) mouseX, ( float ) mouseY, 0f );
			Vector3 farsource = new Vector3( ( float ) mouseX, ( float ) mouseY, 1f );

			Vector3 nearPoint = Platform.Instance.Device.Viewport.Unproject( nearsource, projection, view, Matrix.Identity );
			Vector3 farPoint = Platform.Instance.Device.Viewport.Unproject( farsource, projection, view, Matrix.Identity );

			Vector3 direction = farPoint - nearPoint;
			direction.Normalize();
			Ray cursorRay = new Ray( nearPoint, direction );

			float? distance = cursorRay.Intersects( _zeroZPlane );
			if ( distance != null )
			{
				CursorManager.Instance.SceneNode.SetPosition( cursorRay.Position + cursorRay.Direction * distance.Value );
			}

			Renderer.Instance.SetParameter( "eyePosition", currentCam.Parent.GetPosition() );
			Renderer.Instance.SetParameter( "lightPosition", new Vector3( 0, 40000, 0 ) );

			foreach ( var registeredNode in _registeredNodes )
			{
				Renderer.Instance.LoadMatrix( EMatrix.World, registeredNode.GetAbsoluteTransformation() );
				Renderer.Instance.SetParameter( "matWorld", Renderer.Instance.MatWorld );
				Renderer.Instance.SetParameter( "matWorldInverseTranspose", Renderer.Instance.MatWorldInverseTranspose );
				Renderer.Instance.SetParameter( "matWorldViewProj", Renderer.Instance.MatWorldViewProjection );


				foreach ( var attachable in registeredNode.Attachable )
				{
					attachable.Render( dt );
				}
			}

		}
	}
}
