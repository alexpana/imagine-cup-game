using System;

namespace UnifiedInputSystem.Events
{
	/// <summary>
	/// The type of the gesture
	/// </summary>
	[Flags]
	public enum GestureType
	{
		Activate = 1,
		HoldActivate = 2,
	}
}
