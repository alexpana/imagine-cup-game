using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using VertexArmy.Global;

namespace VertexArmy.States.Menu
{
	public class GameStateSelectLevelMenu : BaseMenuGameState
	{
		public GameStateSelectLevelMenu( ContentManager contentManager )
			: base( contentManager )
		{

		}

		public override void OnUpdate( GameTime gameTime )
		{
			base.OnUpdate( gameTime );
		}

		public override void OnEnter()
		{
			base.OnEnter();

			Platform.Instance.SoundManager.ResumeMusic();
		}

		public override void OnClose()
		{
			base.OnClose();
		}
	}
}
