using System.Collections.Generic;

namespace VertexArmy.Global.Behaviours
{
	public interface IController : IUpdatable
	{
		List<object> Data { get; set; }
		void Clean();
	}
}
