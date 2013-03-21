using System.Collections.Generic;
using FarseerPhysics.Collision.Shapes;
using FarseerPhysics.Common;
using FarseerPhysics.Dynamics;
using FarseerPhysics.Dynamics.Joints;
using FarseerPhysics.Factories;
using Microsoft.Xna.Framework;
using VertexArmy.Global;
using VertexArmy.Global.Managers;
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

	public struct CameraSceneNodePrefab
	{
		public Vector3 LookingDirection;
		public Vector3 UpVector;
		public float Near, Far, AspectRatio, Fov;
	}

	public struct MeshSceneNodePrefab
	{
		public string Name { get; set; }
		public string Body;
		public string Mesh;
		public string Material;

		public Material GetMaterial()
		{
			return MaterialRepository.Instance.GetMaterial( Material );
		}
	}

	public struct PathMeshSceneNodePrefab
	{
		public string Name { get; set; }
		public string Path;
		public string Mesh;
		public string Material;

		public int StartIndex, EndIndex;

		public Material GetMaterial()
		{
			return MaterialRepository.Instance.GetMaterial( Material );
		}
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

			foreach ( ShapePrefab sp in Shapes )
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

		public void AttachToBody( Body body )
		{
			switch ( Type )
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

		public float MaxMotorTorque;
		public bool MotorEnabled;
		public float Frequency;
		public float DampingRatio;


		public Joint GetPhysicsJoint( Body body1, Body body2 )
		{
			switch ( Type )
			{
				case JointType.Line:
					LineJoint joint = new LineJoint( body1, body2, Anchor, Axis );

					joint.MaxMotorTorque = MaxMotorTorque;
					joint.MotorEnabled = MotorEnabled;
					joint.Frequency = Frequency;
					joint.DampingRatio = DampingRatio;

					Platform.Instance.PhysicsWorld.AddJoint( joint );
					return joint;
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

		public PathEntity GetPathEntity()
		{
			PathEntity pathEntity = new PathEntity( );
			List<Shape> shapes = new List<Shape>( );
			Body b = Body.GetPhysicsBody( );

			foreach ( var fix in b.FixtureList )
			{
				shapes.Add( fix.Shape );
			}
			Platform.Instance.PhysicsWorld.RemoveBody( b );

			pathEntity.Bodies = PathManager.EvenlyDistributeShapesAlongPath(
				Platform.Instance.PhysicsWorld,
				Path,
				shapes,
				Body.Static ? BodyType.Static : BodyType.Dynamic,
				BodyCount + 1
			);

			switch ( JointType )
			{

				case JointType.Revolute:

					pathEntity.Joints = new List<Joint>( PathManager.AttachBodiesWithRevoluteJoint(
						Platform.Instance.PhysicsWorld,
						pathEntity.Bodies,
						Anchor1,
						Anchor2,
						ConnectFirstAndLast,
						CollideConnected
						) );

					return pathEntity;

				case JointType.Slider:

					pathEntity.Joints = new List<Joint>( PathManager.AttachBodiesWithSliderJoint(
						Platform.Instance.PhysicsWorld,
						pathEntity.Bodies,
						Anchor1,
						Anchor2,
						ConnectFirstAndLast,
						CollideConnected,
						MaxLength,
						MinLength
						) );

					return pathEntity;
			}

			return new PathEntity( );
		}
	}
}
