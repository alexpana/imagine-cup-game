using System.Collections.Generic;
using FarseerPhysics.Dynamics;
using Microsoft.Xna.Framework;
using VertexArmy.Global;
using VertexArmy.Utilities;

namespace VertexArmy.GameWorld.Prefabs.Structs
{
	public struct BodyPrefab
	{
		public string Name { get; set; }
		public bool Static { get; set; }
		public float Friction { get; set; }
		public float Restitution { get; set; }
		public Vector2 LocalPosition;

		public List<ShapePrefab> Shapes;

		public Body GetPhysicsBody( float scale )
		{
			Body pBody = new Body( Platform.Instance.PhysicsWorld );

			pBody.Position = UnitsConverter.ToSimUnits( LocalPosition ) * scale;
			pBody.Restitution = Restitution;
			pBody.IsStatic = Static;
			pBody.Friction = Friction;

			foreach ( ShapePrefab sp in Shapes )
			{
				sp.AttachToBody( pBody, scale );
			}

			return pBody;
		}
	}
}