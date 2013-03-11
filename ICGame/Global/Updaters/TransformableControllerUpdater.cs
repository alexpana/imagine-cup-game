using System.Collections.Generic;
using Microsoft.Xna.Framework;
using VertexArmy.Common;

namespace VertexArmy.Global.Updaters
{
	class TransformableControllerUpdater : IUpdateableObject
	{
		private List<IUpdateableObject> _updateables = new List<IUpdateableObject>();

		public static TransformableControllerUpdater Instance
		{
			get { return TransformableControllerUpdaterInstanceHolder.Instance; }
		}

		public void Update(GameTime dt)
		{
			foreach ( var obj in _updateables)
			{
				obj.Update(dt);
			}
		}

		public void RegisterUpdatable( IUpdateableObject obj )
		{
			if ( !_updateables.Contains( obj ) )
			{
				_updateables.Add( obj );
			}
		}

		public void UnregisterUpdatable( IUpdateableObject obj )
		{
			_updateables.Remove( obj );
		}

		private static class TransformableControllerUpdaterInstanceHolder
		{
			// ReSharper disable MemberHidesStaticFromOuterClass
			public static readonly TransformableControllerUpdater Instance = new TransformableControllerUpdater( );
			// ReSharper restore MemberHidesStaticFromOuterClass
		}
	}
}
