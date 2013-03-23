
namespace VertexArmy.States.Menu
{
	public class SwitchMenuItem : MenuItem
	{
		public string OnTitle { get; set; }
		public string OffTitle { get; set; }
		public string Prefix { get; set; }

		public override string Title
		{
			get { return Prefix + ": " + ( IsOn ? OnTitle : OffTitle ); }
		}

		public bool IsOn { get; set; }

		public override void Activate()
		{
			IsOn = !IsOn;
			if ( Activated != null )
			{
				Activated( IsOn );
			}
		}
	}
}
