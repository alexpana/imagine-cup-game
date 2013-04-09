using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace VertexArmy.Graphics
{
	//maybe name not cool
	public class Attachable
	{
		public SceneNode Parent;

		public virtual void Render(float dt)
		{
			
		}

		public virtual void RenderDepth (float dt)
		{
			
		}
	}
}
