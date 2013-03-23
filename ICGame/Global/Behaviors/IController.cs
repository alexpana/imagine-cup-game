using System.Collections.Generic;

namespace VertexArmy.Global.Behaviors
{
	public interface IController
	{
		void DirectCompute( ref List<IParameter> data );
		List<IParameter> Data { get; set; }
	}
}
