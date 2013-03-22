using System;

namespace VertexArmy.States.Menu
{
	public class MenuCubeItem
	{
		public Action<object> Activated { get; set; }
		public string Title { get; set; }

		public void Activate()
		{
			if ( Activated != null )
			{
				Activated( this );
			}
		}
	}
}
