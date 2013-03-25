
using System.Collections.Generic;
using FarseerPhysics.Dynamics;
using FarseerPhysics.Dynamics.Contacts;

namespace VertexArmy.Global.Managers
{
	public class PhysicsContactManager
	{
		public static PhysicsContactManager Instance
		{
			get { return PhysicsContactManagerInstanceHolder.Instance; }
		}

		public delegate bool BeginContactCallback( Contact c );
		public delegate void EndContactCallback( Contact c );

		private Dictionary<Body, BeginContactCallback> _fixtureABeginCallbacks;
		private Dictionary<Body, BeginContactCallback> _fixtureBBeginCallbacks;
		private Dictionary<Body, EndContactCallback> _fixtureAEndCallbacks;
		private Dictionary<Body, EndContactCallback> _fixtureBEndCallbacks;

		private PhysicsContactManager()
		{
		}

		public void Initialize()
		{
			Platform.Instance.PhysicsWorld.ContactManager.BeginContact = BeginContact;
			Platform.Instance.PhysicsWorld.ContactManager.EndContact = EndContact;

			_fixtureABeginCallbacks = new Dictionary<Body, BeginContactCallback>();
			_fixtureAEndCallbacks = new Dictionary<Body, EndContactCallback>();
			_fixtureBBeginCallbacks = new Dictionary<Body, BeginContactCallback>();
			_fixtureBEndCallbacks = new Dictionary<Body, EndContactCallback>();
		}

		public void RegisterBeginCallback( ContactCallbackType type, Body body, BeginContactCallback callback )
		{
			switch ( type )
			{
				case ContactCallbackType.FixtureABegin:
					_fixtureABeginCallbacks.Add( body, callback );
					break;

				case ContactCallbackType.FixtureBBegin:
					_fixtureBBeginCallbacks.Add( body, callback );
					break;
			}
		}

		public void RegisterEndCallback( ContactCallbackType type, Body body, EndContactCallback callback )
		{
			switch ( type )
			{
				case ContactCallbackType.FixtureAEnd:
					_fixtureAEndCallbacks.Add( body, callback );
					break;

				case ContactCallbackType.FixtureBEnd:
					_fixtureBEndCallbacks.Add( body, callback );
					break;
			}
		}

		public void UnregisterCallback( ContactCallbackType type, Body body )
		{
			switch ( type )
			{
				case ContactCallbackType.FixtureABegin:
					_fixtureABeginCallbacks.Remove( body );
					break;

				case ContactCallbackType.FixtureAEnd:
					_fixtureAEndCallbacks.Remove( body );
					break;

				case ContactCallbackType.FixtureBBegin:
					_fixtureBBeginCallbacks.Remove( body );
					break;

				case ContactCallbackType.FixtureBEnd:
					_fixtureBEndCallbacks.Remove( body );
					break;
			}
		}

		public bool BeginContact( Contact c )
		{
			if ( _fixtureABeginCallbacks.ContainsKey( c.FixtureA.Body ) )
			{
				return _fixtureABeginCallbacks[c.FixtureA.Body]( c );
			}

			if ( _fixtureBBeginCallbacks.ContainsKey( c.FixtureB.Body ) )
			{
				return _fixtureBBeginCallbacks[c.FixtureB.Body]( c );
			}

			return true;
		}

		public void EndContact( Contact c )
		{
			if ( _fixtureAEndCallbacks.ContainsKey( c.FixtureA.Body ) )
			{
				_fixtureAEndCallbacks[c.FixtureA.Body]( c );
			}

			if ( _fixtureBEndCallbacks.ContainsKey( c.FixtureB.Body ) )
			{
				_fixtureBEndCallbacks[c.FixtureB.Body]( c );
			}
		}

		public void Clear()
		{
			_fixtureABeginCallbacks.Clear();
			_fixtureBBeginCallbacks.Clear();
			_fixtureAEndCallbacks.Clear();
			_fixtureBEndCallbacks.Clear();
		}

		private static class PhysicsContactManagerInstanceHolder
		{
			// ReSharper disable MemberHidesStaticFromOuterClass
			public static readonly PhysicsContactManager Instance = new PhysicsContactManager();
			// ReSharper restore MemberHidesStaticFromOuterClass
		}

	}

	public enum ContactCallbackType
	{
		FixtureABegin,
		FixtureAEnd,
		FixtureBBegin,
		FixtureBEnd
	}
}
