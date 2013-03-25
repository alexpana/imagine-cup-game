using System.Collections.Generic;
using System.Diagnostics;
using Microsoft.Xna.Framework;
using VertexArmy.Global.Behaviours;

namespace VertexArmy.Global.Managers
{
	public class FrameUpdateManager
	{
		public static FrameUpdateManager Instance
		{
			get { return FrameUpdateManagerInstanceHolder.Instance; }
		}


		private readonly List<IUpdatable> _updatables = new List<IUpdatable>();


		public void Register( IUpdatable obj )
		{
			if ( !_updatables.Contains( obj ) )
			{
				_updatables.Add( obj );
			}
		}

		public void Unregister( IUpdatable obj )
		{
			_updatables.Remove( obj );
		}

		public void Update( GameTime dTime )
		{
			foreach ( IUpdatable updatable in _updatables )
			{
				Debug.Assert( updatable != null, "updatable != null" );
				updatable.Update( dTime );
			}
		}

		public void Clear()
		{
			_updatables.Clear();
		}

		private static class FrameUpdateManagerInstanceHolder
		{
			// ReSharper disable MemberHidesStaticFromOuterClass
			public static readonly FrameUpdateManager Instance = new FrameUpdateManager();
			// ReSharper restore MemberHidesStaticFromOuterClass
		}
	}
}
