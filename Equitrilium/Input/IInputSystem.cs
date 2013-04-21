using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Input;

namespace VertexArmy.Input
{
	public interface IInputSystem
	{
		void Update( GameTime gameTime );

		bool IsLeftPointerPressed { get; }
		bool WasLeftPointerPressed { get; }
		bool IsRightPointerPressed { get; }
		bool WasRightPointerPressed { get; }

		bool IsLeftPointerFirstTimePressed { get; }
		bool IsRightPointerFirstTimePressed { get; }

		//Vector2 PointerPosition { get; }
		//Vector2 PointerDelta { get; }
		int ScrollDelta { get; }

		bool IsKeyPressed( Keys key, bool continuous = true );
	}
}
