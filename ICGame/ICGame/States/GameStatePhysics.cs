using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Audio;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using VertexArmy.Global;

namespace VertexArmy.States
{
	internal class GameStatePhysics : IGameState
	{
		private ContentManager _contentManager;

		public GameStatePhysics( ContentManager content )
		{
			_contentManager = content;
		}

		public void OnUpdate( GameTime dt )
		{
			
		}

		public void RenderScene()
		{
		
		}

		public void OnRender( GameTime dt )
		{
			
		}

		public void OnEnter()
		{
			
		}

		public void OnClose()
		{
			_contentManager.Unload( );
		}
	}
}
