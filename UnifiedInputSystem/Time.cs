using System;

namespace UnifiedInputSystem
{
	/// <summary>
	/// Holds information about the time passed in the system
	/// </summary>
	public class Time
	{
		public Time()
		{
		}
		/// <summary>
		/// Gets or sets the delta time since the last system update
		/// </summary>
		public TimeSpan DeltaTime { get; set; }

		/// <summary>
		/// Gets or sets the total running time of the system
		/// </summary>
		public TimeSpan TotalTime { get; set; }

#if XNA
		public Time( Microsoft.Xna.Framework.GameTime gameTime )
		{
			DeltaTime = gameTime.ElapsedGameTime;
			TotalTime = gameTime.TotalGameTime;
		}
#endif
	}
}
