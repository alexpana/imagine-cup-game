using System.Collections.Generic;
using Microsoft.Xna.Framework;
using VertexArmy.Common;


namespace VertexArmy.Global
{
	class Updateables : IUpdateableObject
	{
		private List<IUpdateableObject> _updatables = new List<IUpdateableObject>();

		public static Updateables Instance
		{
			get { return UpdateablesInstanceHolder.Instance; }
		}

		public void Update(GameTime dt)
		{
			foreach ( var obj in _updatables)
			{
				obj.Update(dt);
			}
		}

		public void RegisterUpdatable( IUpdateableObject obj )
		{
			_updatables.Add( obj );
		}

		public void UnregisterUpdatable( IUpdateableObject obj )
		{
			_updatables.Remove( obj );
		}

		private static class UpdateablesInstanceHolder
		{
			// ReSharper disable MemberHidesStaticFromOuterClass
			public static readonly Updateables Instance = new Updateables( );
			// ReSharper restore MemberHidesStaticFromOuterClass
		}
	}
}
