using System.Collections.Generic;
using VertexArmy.Global.Behaviours;

namespace VertexArmy.Global.Controllers.Components
{
	public class SentientForceComponent : BaseComponent
	{
		public SentientForceComponent()
		{
			_type = ComponentType.SentientForce;
		}

		public virtual void DirectCompute( ref List<IParameter> data )
		{
		}
	}
}