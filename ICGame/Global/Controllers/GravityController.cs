using System.Collections.Generic;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Input;
using VertexArmy.Global.Behaviours;

namespace VertexArmy.Global.Controllers
{
	public class GravityController : IController, IUpdatable
	{
		private bool _actionChangeGravity;
		private Vector2 _startPosition, _endPosition;
		public GravityController()
		{
			_actionChangeGravity = false;
		}

		public void Update( GameTime dt )
		{
			if ( Mouse.GetState().MiddleButton.Equals( ButtonState.Pressed ) && !_actionChangeGravity )
			{
				_actionChangeGravity = true;
				_startPosition = new Vector2( Mouse.GetState().X, Mouse.GetState().Y );
			}
			else if ( Mouse.GetState().MiddleButton.Equals( ButtonState.Released ) && _actionChangeGravity )
			{
				_actionChangeGravity = false;
				_endPosition = new Vector2( Mouse.GetState().X, Mouse.GetState().Y );
				Vector2 gravity = ( _endPosition - _startPosition );
				gravity.Normalize();
				Platform.Instance.PhysicsWorld.Gravity = gravity * 9.82f;
			}
		}

		public void DirectCompute( ref List<IParameter> data )
		{

		}

		public List<IParameter> Data { get; set; }

		public void Clean()
		{

		}
	}
}
