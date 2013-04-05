
using System.Collections.Generic;
using Microsoft.Xna.Framework;

namespace VertexArmy.Global.Managers
{
	public class HintManager
	{
		private List<Hint> _activeHints;

		public HintManager()
		{
			_activeHints = new List<Hint>();
		}

		public void SpawnHint( string text, Vector2 position, float time )
		{
			_activeHints.Add( new Hint() { Text = text, Position = position, Time = time } );
		}
		public static HintManager Instance
		{
			get { return HintManagerInstanceHolder.Instance; }
		}

		private static class HintManagerInstanceHolder
		{
			// ReSharper disable MemberHidesStaticFromOuterClass
			public static readonly HintManager Instance = new HintManager();
			// ReSharper restore MemberHidesStaticFromOuterClass
		}
	}

	public class Hint
	{
		public string Text;
		public Vector2 Position;
		public float Time; // in seconds
		public float RegisteredTime;
	}

}