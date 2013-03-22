using System.Collections.Generic;

namespace VertexArmy.States.Menu
{
	public class MenuCube
	{
		public List<MenuCubeItem> Items { get; set; }
		public int SelectedItem { get; set; }

		public string Title { get; set; }

		public MenuCube PreviousMenu { get; set; }
	}
}
