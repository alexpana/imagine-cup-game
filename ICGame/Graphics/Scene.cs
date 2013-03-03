
using System.Collections.Generic;
using Microsoft.Xna.Framework;

namespace VertexArmy.Graphics
{
	public class Scene
	{

		private List<SceneNode> _registeredNodes;
		public Vector3 Light;
		public Vector3 Eye;


		public void RegisterSceneNode( SceneNode node )
		{
			_registeredNodes.Add(node);
		}

		public void OnRender(float dt)
		{
			foreach (var registeredNode in _registeredNodes)
			{
				registeredNode.OnRender(dt, this, 0);
			}
		}
	}
}
