using Microsoft.Xna.Framework;

namespace VertexArmy.Global.Hints
{
	public interface IHint
	{
		string Text { get; set; }
		void Update( GameTime time );
		void Render();
	}
}
