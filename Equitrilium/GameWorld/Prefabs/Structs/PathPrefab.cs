using System.Collections.Generic;
using FarseerPhysics.Collision.Shapes;
using FarseerPhysics.Common;
using FarseerPhysics.Dynamics;
using FarseerPhysics.Dynamics.Joints;
using FarseerPhysics.Factories;
using Microsoft.Xna.Framework;
using VertexArmy.Global;
using VertexArmy.Utilities;

namespace VertexArmy.GameWorld.Prefabs.Structs
{
	public class PathPrefab
	{
		public string Name { get; set; }

		public List<Vector2> Path;
		public JointType JointType;

		public bool ConnectFirstAndLast;
		public bool CollideConnected;
		public float MinLength, MaxLength;
		public Vector2 Anchor1, Anchor2;

		public BodyPrefab Body { get; set; }
		public int BodyCount;

		public PathEntity GetPathEntity( Vector2 scale )
		{
			float scaleD = ( scale.X + scale.Y ) / 2;
			PathEntity pathEntity = new PathEntity();
			List<Shape> shapes = new List<Shape>();
			Body b = Body.GetPhysicsBody( scale );

			foreach ( var fix in b.FixtureList )
			{
				shapes.Add( fix.Shape );
			}


			Path p = new Path();
			foreach ( Vector2 node in Path )
			{
				p.Add( UnitsConverter.ToSimUnits( node ) * scale );
			}

			pathEntity.Bodies = PathManager.EvenlyDistributeShapesAlongPath(
				Platform.Instance.PhysicsWorld,
				p,
				shapes,
				Body.Static ? BodyType.Static : BodyType.Dynamic,
				BodyCount + 1
				);

			foreach ( var link in pathEntity.Bodies )
			{
				link.CollisionGroup = b.FixtureList[0].CollisionGroup;
			}

			Platform.Instance.PhysicsWorld.RemoveBody( b );

			switch ( JointType )
			{
				case JointType.Revolute:

					pathEntity.Joints = new List<Joint>( PathManager.AttachBodiesWithRevoluteJoint(
						Platform.Instance.PhysicsWorld,
						pathEntity.Bodies,
						UnitsConverter.ToSimUnits( Anchor1 ) * scale,
						UnitsConverter.ToSimUnits( Anchor2 ) * scale,
						ConnectFirstAndLast,
						CollideConnected
						) );

					return pathEntity;

				case JointType.Slider:

					pathEntity.Joints = new List<Joint>( PathManager.AttachBodiesWithSliderJoint(
						Platform.Instance.PhysicsWorld,
						pathEntity.Bodies,
						UnitsConverter.ToSimUnits( Anchor1 ) * scale,
						UnitsConverter.ToSimUnits( Anchor2 ) * scale,
						ConnectFirstAndLast,
						CollideConnected,
						UnitsConverter.ToSimUnits( MaxLength ) * scaleD,
						UnitsConverter.ToSimUnits( MinLength ) * scaleD
						) );

					return pathEntity;
			}

			return new PathEntity();
		}
	}
}
