using System.Collections.Generic;
using Microsoft.Xna.Framework;
using VertexArmy.Global.Behaviours;
using VertexArmy.Graphics;

namespace VertexArmy.Global.Controllers
{
	public class WorldShapeCollisionController : IController
	{
		WorldShapeCollisionController()
		{
			Data = new List<object>(4);
		}
		/// <param name="node">

		/// </param>
		public void SetSubject( SceneNode node )
		{
			Data[0] = node;
		}
		/// <param name="shapeListener">

		/// </param>
		public void Register( IShapeListener shapeListener )
		{
			if ( Data[1] == null )
				Data[1] = new List<IShapeListener>();
			((List<IShapeListener>)Data[1]).Add(shapeListener);
		}
		/// <param name="box">

		/// </param>
		public void RegisterBox( BoundingBox box )
		{
			if ( Data[2] == null )
				Data[2] = new List<BoundingBox>();
			( ( List<BoundingBox> ) Data[2] ).Add( box );
		}
		/// <param name="sphere">

		/// </param>
		public void RegisterSphere( BoundingSphere sphere )
		{
			if ( Data[3] == null )
				Data[3] = new List<BoundingSphere>();
			( ( List<BoundingSphere> ) Data[3] ).Add( sphere );
		}

		public void Update(GameTime dt)
		{
			throw new System.NotImplementedException();
		}

		public List<object> Data { get; set; }
		public void Clean()
		{
			throw new System.NotImplementedException();
		}
	}
}

