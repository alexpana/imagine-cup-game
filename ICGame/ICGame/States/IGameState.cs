using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;

namespace VertexArmy.States
{
	public interface IGameState
	{
		void OnUpdate( GameTime dt );
		void OnRender( GameTime dt );
		void OnEnter();
		void OnClose();
	}
}
