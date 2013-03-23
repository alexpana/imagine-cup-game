using System.Collections.Generic;
using Microsoft.Xna.Framework;

namespace VertexArmy.States.Menu
{
	public class MenuCube
	{
		public MenuCube PreviousMenu { get; set; }
		public List<MenuItem> Items { get; set; }
		public int SelectedItem { get; set; }

		public string Title { get; set; }

		public Vector2 Position { get; set; }
	}
}
