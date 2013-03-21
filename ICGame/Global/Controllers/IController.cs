using VertexArmy.Common;

namespace VertexArmy.Global.Controllers
{
	public interface IController : IUpdateableObject
	{
		ITransformable OutputTransformable { get; set; }
	}
}
