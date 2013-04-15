using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace VertexArmy.Global.Hints
{
	interface IDismissableHint : IHint
	{
		Action DismissedCallback { get; set; }
		bool ShouldDismiss();
	}
}
