using System.Collections.Generic;
using UnifiedInputSystem.Events;

namespace UnifiedInputSystem
{
	/// <summary>
	/// A processor that processes an input stream and generates events
	/// </summary>
	public interface IInputProcessor
	{
		/// <summary>
		/// Gets the events processed
		/// </summary>
		/// <returns>A list of input events</returns>
		IEnumerable<IInputEvent> GetEvents();

		/// <summary>
		/// Updates the processor's state
		/// </summary>
		/// <param name="time">A snapshot of the time in the system</param>
		void Update( Time time );
	}
}
