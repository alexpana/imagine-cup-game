using System.Collections.Generic;
using FarseerPhysics.Dynamics;
using Microsoft.Xna.Framework;
using VertexArmy.Global;
using VertexArmy.Utilities;

namespace VertexArmy.GameWorld.Prefabs.Structs
{
	public class BodyPrefab
	{
		public string Name { get; set; }
		public bool Static { get; set; }
		public float Friction { get; set; }
		public float Restitution { get; set; }
		public short CollisionGroup { get; set; }
		public Vector2 LocalPosition;
		public List<string> CollisionSounds;

		public List<ShapePrefab> Shapes;

		public Body GetPhysicsBody( float scale )
		{
			Body pBody = new Body( Platform.Instance.PhysicsWorld );

			pBody.Position = UnitsConverter.ToSimUnits( LocalPosition ) * scale;
			pBody.IsStatic = Static;

			foreach ( ShapePrefab sp in Shapes )
			{
				sp.AttachToBody( pBody, scale );
			}

			pBody.Restitution = Restitution;
			pBody.Friction = Friction;

			pBody.CollisionGroup = CollisionGroup;

			if ( CollisionSounds != null )
			{
				Platform.Instance.SoundManager.RegisterCollisionSound( pBody, CollisionSounds );
			}

			return pBody;
		}
	}
}