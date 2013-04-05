using System;
using System.Collections.Generic;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Audio;
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

		private AudioListener _audioListener;

		private Vector3 _lightPosition = new Vector3(0, 40000, 20000);

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
			_audioListener = new AudioListener();
		}

		public void Clear()
		{
			_registeredNodes.Clear();
			_sceneCameras.Clear();
			_sceneLights.Clear();
		}

		public void SetLightPosition (Vector3 newposition)
		{
			_lightPosition = newposition;
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

		public AudioListener GetCurrentCameraAudioListener()
		{
			_audioListener.Position = _sceneCameras[0].Parent.GetPosition();
			return _audioListener;
		}

		public void Render( float dt )
		{
			if ( _sceneCameras.Count == 0 )
				return;

			Platform.Instance.Device.BlendState = new BlendState();
			Platform.Instance.Device.RasterizerState = RasterizerState.CullCounterClockwise;

			CameraAttachable currentCam = _sceneCameras[0];

			Matrix view = currentCam.GetViewMatrix();
			Matrix projection = currentCam.GetPerspectiveMatrix();

			Renderer.Instance.LoadMatrix( EMatrix.Projection, projection );
			Renderer.Instance.LoadMatrix( EMatrix.View, view );

			MouseState mouseState = Mouse.GetState();
			int mouseX = mouseState.X;
			int mouseY = mouseState.Y;

			Matrix vp = view * projection;

			Vector4 zerov = Vector4.Transform( Vector3.Zero, ( vp ) );

			Vector3 boardpoint = new Vector3( ( float ) mouseX, ( float ) mouseY, zerov.Z / zerov.W );

			Vector3 boardpointW = Platform.Instance.Device.Viewport.Unproject( boardpoint, projection, view, Matrix.Identity );

			CursorManager.Instance.SceneNode.SetPosition( boardpointW );

			Renderer.Instance.SetParameter( "eyePosition", currentCam.Parent.GetPosition() );
			Renderer.Instance.SetParameter( "lightPosition", _lightPosition );

			foreach ( var registeredNode in _registeredNodes )
			{
				Renderer.Instance.LoadMatrix( EMatrix.World, registeredNode.GetAbsoluteTransformation() );
				Renderer.Instance.SetParameter( "matWorld", Renderer.Instance.MatWorld );
				Renderer.Instance.SetParameter( "matWorldInverseTranspose", Renderer.Instance.MatWorldInverseTranspose );
				Renderer.Instance.SetParameter( "matWorldViewProj", Renderer.Instance.MatWorldViewProjection );


				foreach ( var attachable in registeredNode.Attachable )
				{
					if ( !attachable.Parent.Invisible )
					{
						attachable.Render( dt );
					}
				}
			}

		}
	}
}
