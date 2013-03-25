using System.Collections.Generic;
using VertexArmy.Global.Behaviours;

namespace VertexArmy.Global.Managers
{
	public class ControllerRepository
	{
		private readonly Dictionary<string, IController> _controllers = new Dictionary<string, IController>();

		public static ControllerRepository Instance
		{
			get { return ControllerRepositoryInstanceHolder.Instance; }
		}

		public void RegisterController( string name, IController obj )
		{
			if ( !_controllers.ContainsValue( obj ) )
				_controllers[name] = obj;
		}

		public void UnregisterController( string name )
		{
			_controllers.Remove( name );
		}

		public IController GetController( string name )
		{
			return _controllers[name];
		}

		public void Clear()
		{
			_controllers.Clear();
		}

		private static class ControllerRepositoryInstanceHolder
		{
			// ReSharper disable MemberHidesStaticFromOuterClass
			public static readonly ControllerRepository Instance = new ControllerRepository();
			// ReSharper restore MemberHidesStaticFromOuterClass
		}
	}
}
