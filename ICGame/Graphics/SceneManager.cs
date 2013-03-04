
using System.Collections.Generic;
using Microsoft.Xna.Framework;

namespace VertexArmy.Graphics
{
	public class SceneManager
	{
		private List<SceneNode> _registeredNodes;

		public void RegisterSceneNode( SceneNode node )
		{
			_registeredNodes.Add(node);
		}

		public void OnRender(float dt)
		{
			foreach (var registeredNode in _registeredNodes)
			{
				
			}
		}
	}
}
