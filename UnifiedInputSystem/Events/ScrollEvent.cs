
namespace UnifiedInputSystem.Events
{
	/// <summary>
	/// Represents a scroll delta event 
	/// </summary>
	public class ScrollEvent : IInputEvent
	{
		public ScrollEvent( int delta )
		{
			Delta = delta;
		}

		/// <summary>
		/// Gets the scroll delta value
		/// </summary>
		public int Delta { get; private set; }
	}
}
