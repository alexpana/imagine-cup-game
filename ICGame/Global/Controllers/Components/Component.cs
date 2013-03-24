using System.Collections.Generic;
using Microsoft.Xna.Framework;
using VertexArmy.GameWorld;
using VertexArmy.Global.Behaviours;

namespace VertexArmy.Global.Controllers.Components
{
	public abstract class Component : IController, IUpdatable
	{
		private readonly GameEntity _entity;
		public GameEntity Entity
		{
			get { return _entity; }
		}

		public Component( GameEntity entity )
		{
			_entity = entity;
		}

		public virtual void Update( GameTime dt )
		{
		}

		public virtual void DirectCompute( ref List<IParameter> data )
		{
		}

		public List<IParameter> Data { get; set; }
	}
}
