using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;

namespace VertexArmy.States
{
	//note to myself. Need to provide support for pushing states around in a stack like structure in order for this to work
	internal class GameStatePaused : IGameState
	{
		public GameStatePaused( ContentManager content )
		{
		}

		public void OnUpdate( GameTime dt )
		{
			throw new System.NotImplementedException();
		}

		public void OnRender( GameTime dt )
		{
			throw new System.NotImplementedException();
		}

		public void OnEnter()
		{
			throw new System.NotImplementedException();
		}

		public void OnClose()
		{
			throw new System.NotImplementedException();
		}
	}
}
