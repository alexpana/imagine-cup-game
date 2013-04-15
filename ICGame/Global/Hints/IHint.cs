using Microsoft.Xna.Framework;

namespace VertexArmy.Global.Hints
{
	interface IHint
	{
		void Update(GameTime time);
		void Render();
	}
}
