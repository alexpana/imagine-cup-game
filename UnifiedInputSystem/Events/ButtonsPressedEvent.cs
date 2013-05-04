using System.Collections.Generic;

namespace UnifiedInputSystem.Events
{
	/// <summary>
	/// An event that is risen when buttons are pressed
	/// </summary>
	public class ButtonsPressedEvent : IInputEvent
	{
		public ButtonsPressedEvent( List<KeyValuePair<Buttons, bool>> buttons )
		{
			PressedButtons = buttons;
		}

		/// <summary>
		/// Gets the list of key value pairs 
		/// Each pair has the button as key and a correspondent 
		/// boolean that specifies if the button is new (was not pressed before)
		/// </summary>
		public List<KeyValuePair<Buttons, bool>> PressedButtons { get; private set; }
	}
}
