
using System.Collections.Generic;
using Microsoft.Xna.Framework;

namespace VertexArmy.Graphics
{
	public class SceneManager
	{
		private readonly List<SceneNode> _registeredNodes = new List<SceneNode>();

		private readonly List<Camera> _sceneCameras = new List<Camera>();
		private readonly List<Light> _sceneLights = new List<Light>();

		public void RegisterSceneNode( SceneNode node )
		{
			
			Queue<SceneNode> knodes = new Queue<SceneNode>();

			knodes.Enqueue(node);

			while(knodes.Peek() != null)
			{
				SceneNode head;
				_registeredNodes.Add( head = knodes.Dequeue( ) );

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

		public void Update(float dt)
		{
			//to do: link camera & lights, blah blah
			GlobalMatrix.Instance.LoadMatrix( EMatrix.Projection, Matrix.CreatePerspectiveFieldOfView( MathHelper.PiOver4, Global.Platform.Instance.Device.Viewport.AspectRatio, 1, 10000 ) );
			GlobalMatrix.Instance.LoadMatrix( EMatrix.View, Matrix.CreateLookAt( new Vector3( 0, 0, -300 ), new Vector3( 0, 0, 0 ), new Vector3( 0, 1, 0 ) ) );

		}
	}
}
