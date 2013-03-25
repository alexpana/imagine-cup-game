using System.Collections.Generic;
using Microsoft.Xna.Framework;
using VertexArmy.GameWorld;
using VertexArmy.Global.Behaviours;

namespace VertexArmy.Global.Controllers.Components
{
	public abstract class BaseComponent : IController, IUpdatable
	{
		public GameEntity Entity { get; set; }

		public virtual void InitEntity()
		{
		}

		protected ComponentType _type;

		public ComponentType Type
		{
			get { return _type; }
		}

		public virtual void Update( GameTime dt )
		{
		}

		public virtual void DirectCompute( ref List<IParameter> data )
		{
		}

		public virtual void Clean()
		{
		}

		public List<IParameter> Data { get; set; }
	}
}
