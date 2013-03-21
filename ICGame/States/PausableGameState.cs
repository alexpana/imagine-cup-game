using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Input;

namespace VertexArmy.States
{
	internal abstract class PausableGameState : IGameState
	{
		public virtual void OnUpdate( GameTime dt )
		{
			if ( GamePad.GetState( PlayerIndex.One ).Buttons.Back == ButtonState.Pressed ||
				Keyboard.GetState( PlayerIndex.One ).IsKeyDown( Keys.Escape ) )
			{
				StateManager.Instance.PushState( GameState.Pause );
			}
		}

		public abstract void OnRender( GameTime dt );
		public abstract void OnEnter();
		public abstract void OnClose();
	}
}
