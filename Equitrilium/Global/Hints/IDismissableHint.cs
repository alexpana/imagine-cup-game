using System;

namespace VertexArmy.Global.Hints
{
	internal interface IDismissableHint : IHint
	{
		Action DismissedCallback { get; set; }
		bool ShouldDismiss();
	}
}
