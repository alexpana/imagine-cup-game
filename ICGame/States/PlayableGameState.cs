using System;
using Microsoft.Xna.Framework;
using VertexArmy.Global;
using VertexArmy.Global.Managers;

namespace VertexArmy.States
{
	internal abstract class PlayableGameState : PausableGameState
	{
		protected bool _pausePhysics = false;
		public override void OnUpdate( GameTime gameTime )
		{
			base.OnUpdate( gameTime );
			if ( !_pausePhysics )
			{
				Platform.Instance.PhysicsWorld.Step( Math.Min( ( float ) gameTime.ElapsedGameTime.TotalMilliseconds * 0.001f, ( 1f / 30f ) ) );
			}
			FrameUpdateManager.Instance.Update( gameTime );
		}
		public override void OnRender( GameTime gameTime )
		{
			SceneManager.Instance.Render( gameTime.ElapsedGameTime.Milliseconds );
		}
	}
}
