using Microsoft.Xna.Framework;

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
