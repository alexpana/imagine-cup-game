using System.Collections.Generic;

namespace VertexArmy.Global.Behaviours
{
	public interface IController
	{
		void DirectCompute( ref List<IParameter> data );
		List<IParameter> Data { get; set; }
	}
}
