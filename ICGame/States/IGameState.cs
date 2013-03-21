using Microsoft.Xna.Framework;

namespace VertexArmy.States
{
	public interface IGameState
	{
		void OnUpdate( GameTime gameTime );
		void OnRender( GameTime gameTime );
		void OnEnter();
		void OnClose();
	}
}
