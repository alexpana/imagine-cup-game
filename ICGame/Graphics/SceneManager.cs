using System;
using System.Collections.Generic;
using Microsoft.Xna.Framework;

namespace VertexArmy.Graphics
{
	public class SceneManager
	{
		private static volatile SceneManager _instance;
		private static readonly object _syncRoot = new Object( );

		private readonly List<SceneNode> _registeredNodes = new List<SceneNode>( );

		private readonly List<CameraAttachable> _sceneCameras = new List<CameraAttachable>( );
		private readonly List<LightAttachable> _sceneLights = new List<LightAttachable>( );


		public static SceneManager Instance
		{
			get
			{
				if ( _instance == null )
				{
					lock ( _syncRoot )
					{
						if ( _instance == null )
							_instance = new SceneManager( );
					}
				}
				return _instance;
			}
		}

		public void Clear()
		{
			_registeredNodes.Clear( );
			_sceneCameras.Clear( );
			_sceneLights.Clear( );
		}

		public void RegisterSceneTree( SceneNode node )
		{

			Queue<SceneNode> knodes = new Queue<SceneNode>( );

			knodes.Enqueue( node );

			while ( knodes.Count != 0 )
			{
				SceneNode head = knodes.Dequeue( );

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
			Queue<SceneNode> knodes = new Queue<SceneNode>( );

			knodes.Enqueue( node );

			while ( knodes.Count != 0 )
			{
				SceneNode head = knodes.Dequeue( );

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

		public void Render( float dt )
		{
			if ( _sceneCameras.Count == 0 )
				return;

			CameraAttachable currentCam = _sceneCameras[0];

			Renderer.Instance.LoadMatrix( EMatrix.Projection, currentCam.GetPerspectiveMatrix( ) );
			Renderer.Instance.LoadMatrix( EMatrix.View, currentCam.GetViewMatrix( ) );


			Renderer.Instance.SetParameter( "eyePosition", currentCam.Parent.GetPosition( ) );
			Renderer.Instance.SetParameter( "lightPosition", new Vector3( 0, 40000, 0 ) );

			foreach ( var registeredNode in _registeredNodes )
			{
				Renderer.Instance.LoadMatrix( EMatrix.World, registeredNode.GetAbsoluteTransformation( ) );
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
