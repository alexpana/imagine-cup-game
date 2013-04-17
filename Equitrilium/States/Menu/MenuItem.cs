using System;

namespace VertexArmy.States.Menu
{
	public class MenuItem
	{
		public Action<object> Activated { get; set; }
		public virtual string Title { get; set; }

		public virtual void Activate()
		{
			if ( Activated != null )
			{
				Activated( null );
			}
		}
	}
}
