using Microsoft.Xna.Framework;

namespace VertexArmy.Input
{
	internal interface IInputSystem
	{
		void Update( GameTime gameTime );

		bool IsLeftPointerPressed { get; }
		Vector2 PointerPosition { get; }
		Vector2 PointerDelta { get; }
	}
}
