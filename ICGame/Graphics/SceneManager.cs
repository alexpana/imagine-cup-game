
using System;
using System.Collections.Generic;
using Microsoft.Xna.Framework;

namespace VertexArmy.Graphics
{
	public class SceneManager
	{
		private static volatile SceneManager _instance;
		private static readonly object _syncRoot = new Object( );
		

		private readonly List<SceneNode> _registeredNodes = new List<SceneNode>();

		private readonly List<Camera> _sceneCameras = new List<Camera>();
		private readonly List<Light> _sceneLights = new List<Light>();


		public static SceneManager Instance
		{
			get
			{
				if (_instance == null)
				{
					lock (_syncRoot)
					{
						if (_instance == null)
							_instance = new SceneManager();
					}
				}
				return _instance;
			}
		}
		
		public void Clear()
		{
			_registeredNodes.Clear ( );
			_sceneCameras.Clear ( );
			_sceneLights.Clear ( );
		}

		public void RegisterSceneTree( SceneNode node )
		{
			
			Queue<SceneNode> knodes = new Queue<SceneNode>();

			knodes.Enqueue(node);

			while(knodes.Count != 0)
			{
				SceneNode head = knodes.Dequeue( );

				/* protect against multiple register */
				if(_registeredNodes.Contains(head))
					continue;

				_registeredNodes.Add( head );

				foreach(var child in head.Children)
				{
					knodes.Enqueue(child);
				}

				foreach (var attachable in head.Attachable)
				{
					Light light = attachable as Light;
					if (light != null)
						_sceneLights.Add(light);

					Camera camera = attachable as Camera;
					if ( camera != null )
						_sceneCameras.Add( camera );
				}
			}
		}

		public void Render(float dt)
		{
			//to do: link camera & lights, blah blah
			Renderer.Instance.LoadMatrix( EMatrix.Projection, Matrix.CreatePerspectiveFieldOfView( MathHelper.PiOver4, Global.Platform.Instance.Device.Viewport.AspectRatio, 1, 10000 ) );
			Renderer.Instance.LoadMatrix( EMatrix.View, Matrix.CreateLookAt( new Vector3( 0, 0, -300 ), new Vector3( 0, 0, 0 ), new Vector3( 0, 1, 0 ) ) );
			Renderer.Instance.SetParameter("eyePosition", new Vector3(0,0,-300));
			Renderer.Instance.SetParameter("lightPosition", new Vector3( 0, 4000, 0 ) );

			foreach (var registeredNode in _registeredNodes)
			{
				Renderer.Instance.LoadMatrix( EMatrix.World, registeredNode.GetAbsoluteTransformation( ) );

				foreach ( var attachable in registeredNode.Attachable)
				{
					attachable.Render(dt);
				}
			}

		}
	}
}
