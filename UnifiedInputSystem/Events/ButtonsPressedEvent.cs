using System.Collections.Generic;
using UnifiedInputSystem.Input;

namespace UnifiedInputSystem.Events
{
	/// <summary>
	/// An event that is risen when buttons are pressed
	/// </summary>
	public class ButtonsPressedEvent : IInputEvent
	{
		public ButtonsPressedEvent( Dictionary<UISButton, bool> buttons )
		{
			PressedButtons = buttons;
		}

		/// <summary>
		/// Gets the dictionary of pressed keys
		/// Each pair has the button as key and a correspondent 
		/// boolean that specifies if the button is new (was not pressed before)
		/// </summary>
		public Dictionary<UISButton, bool> PressedButtons { get; private set; }
	}
}
