using System.Collections.Generic;
using Microsoft.Xna.Framework;
using VertexArmy.Common;
namespace VertexArmy.Global.Controllers
{
	class ControllerManager : IUpdateableObject
	{
		private List<IController> _controllers = new List<IController>( );

		public static ControllerManager Instance
		{
			get { return ControllerManagerInstanceHolder.Instance; }
		}

		public void Update( GameTime dt )
		{
			foreach ( var obj in _controllers )
			{
				obj.Update( dt );
			}
		}

		public void RegisterController( IController obj )
		{
			if ( !_controllers.Contains( obj ) )
			{
				_controllers.Add( obj );
			}
		}

		public void UnregisterController( IController obj )
		{
			_controllers.Remove( obj );
		}

		private static class ControllerManagerInstanceHolder
		{
			// ReSharper disable MemberHidesStaticFromOuterClass
			public static readonly ControllerManager Instance = new ControllerManager( );
			// ReSharper restore MemberHidesStaticFromOuterClass
		}
	}
}
