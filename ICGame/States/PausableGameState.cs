using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Input;
using VertexArmy.Global;

namespace VertexArmy.States
{
	internal abstract class PausableGameState : IGameState
	{
		public virtual void OnUpdate( GameTime gameTime )
		{
			if ( GamePad.GetState( PlayerIndex.One ).Buttons.Back == ButtonState.Pressed ||
			     Platform.Instance.Input.IsKeyPressed( Keys.Escape, false ) )
			{
				StateManager.Instance.PushState( GameState.Pause );
			}
		}

		public abstract void OnRender( GameTime gameTime );
		public abstract void OnEnter();
		public abstract void OnClose();
	}
}
