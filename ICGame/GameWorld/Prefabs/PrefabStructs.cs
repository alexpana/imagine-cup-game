using System.Collections.Generic;
using FarseerPhysics.Collision.Shapes;
using FarseerPhysics.Common;
using FarseerPhysics.Dynamics;
using FarseerPhysics.Dynamics.Joints;
using FarseerPhysics.Factories;
using Microsoft.Xna.Framework;
using VertexArmy.Global;
using VertexArmy.Graphics;

namespace VertexArmy.GameWorld.Prefabs
{
	/* main prefab components */
	public struct PhysicsPrefab
	{
		public Dictionary<string, BodyPrefab> Bodies;
		public Dictionary<string, JointPrefab> Joints;
		public Dictionary<string, PathPrefab> Paths;
	}

	public struct SceneNodesPrefab
	{
		public string Name { get; set; }
		public string Body;
		public string Mesh;
		public Material Material;
	}

	/* BodyPrefab */
	public struct BodyPrefab
	{
		public string Name { get; set; }
		public bool Static { get; set; }
		public float Friction { get; set; }
		public float Restitution { get; set; }
		public Vector2 LocalPosition;

		public List<ShapePrefab> Shapes;

		public Body GetPhysicsBody()
		{
			Body pBody = new Body( Platform.Instance.PhysicsWorld );

			pBody.Position = LocalPosition;
			pBody.Restitution = Restitution;
			pBody.IsStatic = Static;
			pBody.Friction = Friction;

			foreach (ShapePrefab sp in Shapes)
			{
				sp.AttachToBody( pBody );
			}

			return pBody;
		}
	}

	public struct ShapePrefab
	{
		public ShapeType Type { get; set; }

		public float XRadius, YRadius;
		public float Density;

		public int Edges;

		public List<Vertices> Polygon;
		public Vector2 Start, End;
		public Vector2 Offset;

		public float Width, Height;

		public void AttachToBody( Body body)
		{
			switch (Type)
			{
				case ShapeType.Circle:
					FixtureFactory.AttachCircle( XRadius, Density, body );
					break;
				case ShapeType.Ellipse:
					FixtureFactory.AttachEllipse( XRadius, YRadius, Edges, Density, body );
					break;
				case ShapeType.Edge:
					FixtureFactory.AttachEdge( Start, End, body );
					break;
				case ShapeType.Rectangle:
					FixtureFactory.AttachRectangle( Width, Height, Density, Offset, body );
					break;
				case ShapeType.Polygon:
					FixtureFactory.AttachPolygon( Polygon[0], Density, body );
					break;
				case ShapeType.CompoundPolygon:
					FixtureFactory.AttachCompoundPolygon( Polygon, Density, body );
					break;
				default:
					return;
			}
		}
	}

	/*JointPrefab*/
	public struct JointPrefab
	{
		public string Name { get; set; }
		public JointType Type { get; set; }

		public string Body1, Body2;
		public Vector2 Anchor, Anchor2, Axis;

		public Joint GetPhysicsJoint( Body body1, Body body2)
		{
			switch ( Type )
			{
				case JointType.Line:
					return new LineJoint( body1, body2, Anchor, Axis );
				case JointType.Revolute:
					return new RevoluteJoint( body1, body2, Anchor, Anchor2 );
				case JointType.Weld:
					return new WeldJoint( body1, body2, Anchor, Anchor2 );
				default:
					return null;
			}
		}
	}

	/*PathPrefab*/
	public struct PathPrefab
	{
		public string Name { get; set; }

		public Path Path;
		public JointType JointType;

		public bool ConnectFirstAndLast;
		public bool CollideConnected;
		public float MinLength, MaxLength;
		public Vector2 Anchor1, Anchor2;

		public BodyPrefab Body { get; set; }
		public int BodyCount;

		public List<Body> GetPhysicsBodies()
		{
			List<Shape> shapes = new List<Shape>( );
			Body b = Body.GetPhysicsBody( );

			foreach ( var fix in b.FixtureList )
			{
				shapes.Add( fix.Shape );
			}
			Platform.Instance.PhysicsWorld.RemoveBody( b );

			List<Body> bodies = PathManager.EvenlyDistributeShapesAlongPath(
				Platform.Instance.PhysicsWorld,
				Path,
				shapes,
				Body.Static ? BodyType.Static : BodyType.Dynamic,
				BodyCount
			);

			switch ( JointType )
			{

				case JointType.Revolute:

					PathManager.AttachBodiesWithRevoluteJoint(
						Platform.Instance.PhysicsWorld,
						bodies,
						Anchor1,
						Anchor2,
						ConnectFirstAndLast,
						CollideConnected
						);

					return bodies;
					
				case JointType.Slider:

					PathManager.AttachBodiesWithSliderJoint(
						Platform.Instance.PhysicsWorld,
						bodies,
						Anchor1,
						Anchor2,
						ConnectFirstAndLast,
						CollideConnected,
						MaxLength,
						MinLength
						);

					return bodies;
			}

			return null;
		}
	}
}
