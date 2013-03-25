using FarseerPhysics.Dynamics;
using Microsoft.Xna.Framework;
using VertexArmy.Global.Behaviours;
using VertexArmy.Graphics;

namespace VertexArmy.Global.Controllers
{
	public class ParameterCamera : IParameter
	{
		public bool Null { get; set; }
		public bool Input { get; set; }
		public bool Output { get; set; }
		public bool Alive { get; set; }

		public CameraAttachable Value;
	}

	public class ParameterTransformable : IParameter
	{
		public bool Null { get; set; }
		public bool Input { get; set; }
		public bool Output { get; set; }
		public bool Alive { get; set; }

		public ITransformable Value;
	}

	public class ParameterGameTime : IParameter
	{
		public bool Null { get; set; }
		public bool Input { get; set; }
		public bool Output { get; set; }
		public bool Alive { get; set; }

		public GameTime Value;
	}

	public class ParameterBody : IParameter
	{
		public bool Null { get; set; }
		public bool Input { get; set; }
		public bool Output { get; set; }
		public bool Alive { get; set; }

		public Body Value;
	}
}
