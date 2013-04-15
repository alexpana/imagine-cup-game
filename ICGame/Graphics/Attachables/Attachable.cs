namespace VertexArmy.Graphics.Attachables
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

		public virtual void PostRender(float dt)
		{
			
		}

		public virtual int GetLayer()
		{
			return 1;
		}
	}
}
