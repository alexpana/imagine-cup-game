namespace UnifiedInputSystem
{
	/// <summary>
	/// Represents a stream of an input source for the system
	/// <typeparam name="T">The type of the payload the stream carries</typeparam>
	/// </summary>
	interface IInputStream<T>
	{
		/// <summary>
		/// Updates the stream's state
		/// </summary>
		/// <param name="time">A snapshot of the time in the system</param>
		void Update( Time time );

		/// <summary>
		/// Returns the current stream state
		/// </summary>
		/// <returns>Current state's payload</returns>
		T GetState();
	}
}
