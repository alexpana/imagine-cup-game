using System;
using System.Collections.Generic;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Audio;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using VertexArmy.Global.Behaviours;
using VertexArmy.Graphics;
using VertexArmy.Graphics.Attachables;

namespace VertexArmy.Global.Managers
{
	public class SceneManager : IUpdatable
	{
		private static volatile SceneManager _instance;
		private static readonly object _syncRoot = new Object();

		private readonly List<SceneNode> _registeredNodes = new List<SceneNode>();

		private readonly List<CameraAttachable> _sceneCameras = new List<CameraAttachable>();
		private readonly List<LightAttachable> _sceneLights = new List<LightAttachable>();

		private AudioListener _audioListener;

		private Vector3 _lightPosition = new Vector3( 0, 40000, 20000 );

		private RenderTarget2D _color;
		private RenderTarget2D _depth;

		private Quad _screenQuad;

		private Quad GetScreenQuad()
		{
			return _screenQuad ?? ( _screenQuad = new Quad() );
		}

		private RenderTarget2D GetColorRt()
		{
			if ( _color == null )
			{
				PresentationParameters pp = Platform.Instance.Device.PresentationParameters;
				_color = new RenderTarget2D( Platform.Instance.Device, pp.BackBufferWidth, pp.BackBufferHeight, false, Platform.Instance.Device.DisplayMode.Format, DepthFormat.Depth24,
					Platform.Instance.Device.PresentationParameters.MultiSampleCount, RenderTargetUsage.PreserveContents );
			}
			return _color;
		}

		private RenderTarget2D GetDepthRt()
		{
			if ( _depth == null )
			{
				PresentationParameters pp = Platform.Instance.Device.PresentationParameters;
				_depth = new RenderTarget2D( Platform.Instance.Device, pp.BackBufferWidth, pp.BackBufferHeight, false, SurfaceFormat.Single, Platform.Instance.Device.PresentationParameters.DepthStencilFormat, 
					Platform.Instance.Device.PresentationParameters.MultiSampleCount, RenderTargetUsage.PreserveContents);
			}
			return _depth;
		}


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

		public void SetLightPosition( Vector3 newposition )
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


		private void RenderBlurred( float dt )
		{

			Platform.Instance.Device.Clear( ClearOptions.Target | ClearOptions.DepthBuffer, Color.DarkSlateBlue, 1.0f, 0 );

			RenderDepthRenderTarget( dt );
			//RenderColorRenderTarget( dt );



			Platform.Instance.Device.BlendState = new BlendState();
			Platform.Instance.Device.RasterizerState = RasterizerState.CullCounterClockwise;


			Quad scquad = GetScreenQuad();
			Material blur = Renderer.Instance.GetBlurMaterial();


			blur.SetParameter( "matWorldViewProj", Matrix.Identity );
			blur.SetParameter( "ColorMap", _depth );
			blur.SetParameter( "blurDistance", 0.01f );

			scquad.Draw( blur );

			Renderer.Instance.LastFrame = Renderer.Instance.CurrentFrame;
		}

		private void RenderWithDof( float dt )
		{
			RenderDepthRenderTarget( dt );
			RenderColorRenderTarget( dt );


			Platform.Instance.Device.Clear( ClearOptions.Target | ClearOptions.DepthBuffer, Color.DarkSlateBlue, 1.0f, 0 );

			Platform.Instance.Device.BlendState = BlendState.Opaque;
			Platform.Instance.Device.RasterizerState = RasterizerState.CullCounterClockwise;


			Quad scquad = GetScreenQuad();
			Material dof = Renderer.Instance.GetDepthOfFieldMaterial();


			dof.SetParameter( "ColorMap", _color );
			dof.SetParameter( "DepthMap", _depth );

			scquad.Draw( dof );

			Renderer.Instance.LastFrame = Renderer.Instance.CurrentFrame;
		}

		private void RenderWithoutPostProcessing( float dt )
		{
			DrawScene( dt );
		}

		public bool UseDof = false;

		
		public  void Render ( float dt )
		{
			if ( UseDof )
			{
				RenderWithDof(dt);
			}
			else
			{
				RenderWithoutPostProcessing( dt );
			}
			HintManager.Instance.Render( dt );
		}

		private void RenderDepth( float dt )
		{
			Platform.Instance.Device.Clear( ClearOptions.Target | ClearOptions.DepthBuffer, Color.DarkSlateBlue, 1.0f, 0 );

			RenderDepthRenderTarget( dt );
			RenderColorRenderTarget( dt );

			Platform.Instance.Device.BlendState = new BlendState( );
			Platform.Instance.Device.RasterizerState = RasterizerState.CullCounterClockwise;


			Quad scquad = GetScreenQuad( );
			Material dof = Renderer.Instance.GetDepthOfFieldMaterial( );


			dof.SetParameter( "matWorldViewProj", Matrix.Identity );
			dof.SetParameter( "ColorMap", _color );
			dof.SetParameter( "DepthMap", _depth );
			scquad.Draw( dof );

			Renderer.Instance.LastFrame = Renderer.Instance.CurrentFrame;
		}

		public void RenderColorRenderTarget(float dt)
		{
			RenderTarget2D colorRenderTarget = GetColorRt();
			Platform.Instance.Device.SetRenderTarget( colorRenderTarget );

			Platform.Instance.Device.Clear( ClearOptions.Target | ClearOptions.DepthBuffer, Color.DarkSlateBlue, 1.0f, 0 );

			DrawScene( dt );

			Renderer.Instance.CurrentFrame = colorRenderTarget;

			Platform.Instance.Device.SetRenderTarget( null );
		}

		public void RenderDepthRenderTarget( float dt )
		{

			RenderTarget2D depthRenderTarget = GetDepthRt();

			Platform.Instance.Device.SetRenderTarget( depthRenderTarget );


			Platform.Instance.Device.Clear( ClearOptions.Target | ClearOptions.DepthBuffer, Color.DarkSlateBlue, 1.0f, 0 );

			DrawDepth( dt );

			Renderer.Instance.Depth = depthRenderTarget;
			Platform.Instance.Device.SetRenderTarget( null );
		}

		private void DrawDepth( float dt )
		{
			if ( _sceneCameras.Count == 0 )
				return;

			DepthStencilState depthState = new DepthStencilState();
			depthState.DepthBufferEnable = true; /* Enable the depth buffer */
			depthState.DepthBufferWriteEnable = true; /* When drawing to the screen, write to the depth buffer */
			depthState.DepthBufferFunction = CompareFunction.Less;
			

			




			Platform.Instance.Device.BlendState = BlendState.Opaque;
			Platform.Instance.Device.DepthStencilState = depthState;
			Platform.Instance.Device.RasterizerState = RasterizerState.CullNone;

			CameraAttachable currentCam = _sceneCameras[0];

			Renderer.Instance.LoadMatrix( EMatrix.Projection, currentCam.GetPerspectiveMatrix() );
			Renderer.Instance.LoadMatrix( EMatrix.View, currentCam.GetViewMatrix() );


			foreach ( var registeredNode in _registeredNodes )
			{
				Renderer.Instance.LoadMatrix( EMatrix.World, registeredNode.GetAbsoluteTransformation() );
				Renderer.Instance.SetParameter( "matWorldViewProj", Renderer.Instance.MatWorldViewProjection );


				foreach ( var attachable in registeredNode.Attachable )
				{
					if ( !attachable.Parent.Invisible )
					{
						attachable.RenderDepth( dt );
					}
				}
			}
		}

		private void DrawScene( float dt )
		{
			if ( _sceneCameras.Count == 0 )
				return;

			Platform.Instance.Device.BlendState = new BlendState();
			Platform.Instance.Device.RasterizerState = RasterizerState.CullCounterClockwise;

			CameraAttachable currentCam = _sceneCameras[0];

			Renderer.Instance.LoadMatrix( EMatrix.Projection, currentCam.GetPerspectiveMatrix() );
			Renderer.Instance.LoadMatrix( EMatrix.View, currentCam.GetViewMatrix() );

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
		
		public void Update( GameTime dt )
		{
			Renderer.Instance.SetParameter( "fTimeMs", (float)dt.TotalGameTime.TotalMilliseconds );

			Matrix view = Renderer.Instance.GetMatrix( EMatrix.View );
			Matrix projection = Renderer.Instance.GetMatrix( EMatrix.Projection );
			MouseState mouseState = Mouse.GetState();
			int mouseX = mouseState.X;
			int mouseY = mouseState.Y;

			Matrix vp = view * projection;

			Vector4 zerov = Vector4.Transform( Vector3.Zero, ( vp ) );

			Vector3 boardpoint = new Vector3( mouseX, mouseY, zerov.Z / zerov.W );

			Vector3 boardpointW = Platform.Instance.Device.Viewport.Unproject( boardpoint, projection, view, Matrix.Identity );

			CursorManager.Instance.SceneNode.SetPosition( boardpointW );
		}

		public List<SceneNode> IntersectRayWithSceneNodes( int screenX, int screenY )
		{
			Vector3 nearPoint = new Vector3( screenX, screenY, 0.0f );
			Vector3 farPoint = new Vector3( screenX, screenY, 1.0f );

			Matrix view = Renderer.Instance.GetMatrix( EMatrix.View );
			Matrix projection = Renderer.Instance.GetMatrix( EMatrix.Projection );

			Vector3 nearPointW = Platform.Instance.Device.Viewport.Unproject( nearPoint, projection, view, Matrix.Identity );
			Vector3 farPointW = Platform.Instance.Device.Viewport.Unproject( farPoint, projection, view, Matrix.Identity );


			Ray ray = new Ray( nearPointW, Vector3.Normalize(farPointW - nearPointW) );


			List<SceneNode> nodes = new List<SceneNode>();

			foreach (SceneNode registeredNode in _registeredNodes)
			{
				bool isMesh = false;

				foreach (Attachable attachable in registeredNode.Attachable)
				{
					if ((attachable as MeshAttachable) != null)
					{
						isMesh = true;
					}
				}

				if (isMesh)
				{
					BoundingBox tBox = registeredNode.GetTransformedBoundingBox();
					if ( ray.Intersects( tBox ) != null )
						nodes.Add(registeredNode);
				}
			}
			return nodes;
		}
	}
}
