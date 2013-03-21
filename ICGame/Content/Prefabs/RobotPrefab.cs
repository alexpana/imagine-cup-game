using System.Collections.Generic;
using FarseerPhysics.Common;
using FarseerPhysics.Dynamics.Joints;
using Microsoft.Xna.Framework;
using VertexArmy.GameWorld.Prefabs;

namespace VertexArmy.Content.Prefabs
{
	public class RobotPrefab
	{
		public static PrefabEntity CreatePrefab()
		{
			PrefabEntity robot = new PrefabEntity { Name = "Robot", PhysicsScale = 1f };

			float PhysicsInternalScale = 0.2f;

			/* gear bodies */
			ShapePrefab gearShape = new ShapePrefab { Type = ShapeType.Circle, XRadius = 1.215f * PhysicsInternalScale, Density = 1f };

			BodyPrefab gear1 = new BodyPrefab
			{
				Name = "Gear1",
				Friction = 10f,
				Restitution = 0.1f,
				LocalPosition = new Vector2( -2.301f * PhysicsInternalScale, 1.243f * PhysicsInternalScale ),
				Static = false,
				Shapes = new List<ShapePrefab> { gearShape }
			};

			BodyPrefab gear2 = new BodyPrefab
			{
				Name = "Gear2",
				Friction = 10f,
				Restitution = 0.1f,
				LocalPosition = new Vector2( 2.301f * PhysicsInternalScale, 1.243f * PhysicsInternalScale ),
				Static = false,
				Shapes = new List<ShapePrefab> { gearShape }
			};

			BodyPrefab gear3 = new BodyPrefab
			{
				Name = "Gear3",
				Friction = 10f,
				Restitution = 0.1f,
				LocalPosition = new Vector2( 0f * PhysicsInternalScale, -2.615f * PhysicsInternalScale ),
				Static = false,
				Shapes = new List<ShapePrefab> { gearShape }
			};

			robot.RegisterBody( gear1 );
			robot.RegisterBody( gear2 );
			robot.RegisterBody( gear3 );

			/* chassis body */
			ShapePrefab chassisShape = new ShapePrefab { Type = ShapeType.Polygon, Density = 1f, Polygon = new List<Vertices> { new Vertices( ) } };
			chassisShape.Polygon[0].Add( new Vector2( -1.215f * PhysicsInternalScale, 0.7f * PhysicsInternalScale ) );
			chassisShape.Polygon[0].Add( new Vector2( 0f * PhysicsInternalScale, -1.4f * PhysicsInternalScale ) );
			chassisShape.Polygon[0].Add( new Vector2( 1.215f * PhysicsInternalScale, 0.7f * PhysicsInternalScale ) );

			BodyPrefab chassis = new BodyPrefab
			{
				Name = "Chassis",
				Restitution = 0.1f,
				Friction = 1f,
				LocalPosition = new Vector2( 0f * PhysicsInternalScale, 0f * PhysicsInternalScale ),
				Static = false,
				Shapes = new List<ShapePrefab> { chassisShape }
			};

			robot.RegisterBody( chassis, true ); // is main body

			/* gear joints */
			const float jointLengthModifier = 1f;

			JointPrefab gearJoint1 = new JointPrefab
			{
				Type = JointType.Line,
				Name = "GearJoint1",
				Body1 = "Chassis",
				Body2 = "Gear1",
				Anchor = gear1.LocalPosition,
				Axis = new Vector2( 0.66f, -0.33f ) * jointLengthModifier,
				MaxMotorTorque = 60f * PhysicsInternalScale,
				Frequency = 10f,
				DampingRatio = 0.85f,
				MotorEnabled = true,
			};

			JointPrefab gearJoint2 = new JointPrefab
			{
				Type = JointType.Line,
				Name = "GearJoint2",
				Body1 = "Chassis",
				Body2 = "Gear2",
				Anchor = gear2.LocalPosition,
				Axis = new Vector2( -0.66f, -0.33f ) * jointLengthModifier,
				MaxMotorTorque = 60f * PhysicsInternalScale,
				Frequency = 10f,
				DampingRatio = 0.85f,
				MotorEnabled = true
			};

			JointPrefab gearJoint3 = new JointPrefab
			{
				Type = JointType.Line,
				Name = "GearJoint3",
				Body1 = "Chassis",
				Body2 = "Gear3",
				Anchor = gear3.LocalPosition,
				Axis = new Vector2( 0f, 0.76f ) * jointLengthModifier,
				MaxMotorTorque = 60f * PhysicsInternalScale,
				Frequency = 10f,
				DampingRatio = 0.85f,
				MotorEnabled = true
			};

			robot.RegisterJoint( gearJoint1 );
			robot.RegisterJoint( gearJoint2 );
			robot.RegisterJoint( gearJoint3 );

			ShapePrefab linkShape = new ShapePrefab
			{
				Type = ShapeType.Rectangle,
				Width = 0.201f * PhysicsInternalScale,
				Height = 0.62f * PhysicsInternalScale,
				Density = 2f,
				Offset = Vector2.Zero
			};

			ShapePrefab linkFeetShape = new ShapePrefab
			{
				Type = ShapeType.Rectangle,
				Width = 0.2f * PhysicsInternalScale,
				Height = 0.36f * PhysicsInternalScale,
				Density = 2f,
				Offset = new Vector2( 0.12f * PhysicsInternalScale, 0f * PhysicsInternalScale )
			};

			BodyPrefab link = new BodyPrefab
			{

				Name = "Link",
				Shapes = new List<ShapePrefab> { linkShape, linkFeetShape },
				Friction = 0.1f,
				Restitution = 0f
			};

			PathPrefab track = new PathPrefab
			{
				Name = "Track",
				Anchor1 = new Vector2( -0.101f, 0.355f ) * PhysicsInternalScale,
				Anchor2 = new Vector2( -0.15f, -0.355f ) * PhysicsInternalScale,
				BodyCount = 29,
				CollideConnected = false,
				ConnectFirstAndLast = true,
				JointType = JointType.Revolute,
				Path = new Path( new List<Vector2>
					{
						new Vector2(4.8f * PhysicsInternalScale, 3f * PhysicsInternalScale ),
						new Vector2(0f * PhysicsInternalScale, -4.6f * PhysicsInternalScale),
						new Vector2(-4.8f * PhysicsInternalScale, 3f * PhysicsInternalScale),
						new Vector2(5f * PhysicsInternalScale, 3f * PhysicsInternalScale )
					}
				),
				Body = link
			};

			robot.RegisterPath( track );

			/* scene nodes */
			MeshSceneNodePrefab gearNode1 = new MeshSceneNodePrefab
			{
				Name = "Gear1",
				Mesh = "models/robo_wheel",
				Material = "RobotMaterial",
				Body = "Gear1"
			};

			MeshSceneNodePrefab gearNode2 = new MeshSceneNodePrefab
			{
				Name = "Gear2",
				Mesh = "models/robo_wheel",
				Material = "RobotMaterial",
				Body = "Gear2"
			};

			MeshSceneNodePrefab gearNode3 = new MeshSceneNodePrefab
			{
				Name = "Gear3",
				Mesh = "models/robo_wheel",
				Material = "RobotMaterial",
				Body = "Gear3"
			};

			robot.RegisterSceneNode( gearNode1 );
			robot.RegisterSceneNode( gearNode2 );
			robot.RegisterSceneNode( gearNode3 );

			
			/*
			MeshSceneNodePrefab chassisNode = new MeshSceneNodePrefab
			{
				Name = "Chassis",
				Mesh = "models/robo_link",
				Material = "RobotMaterial",
				Body = "Chassis"
			};

			robot.RegisterSceneNode( chassisNode );
			 */

			PathMeshSceneNodePrefab linkNode = new PathMeshSceneNodePrefab
			{
				Name = "Links",
				Mesh = "models/robo_link",
				Material = "RobotMaterial",

				Path = "Track",
				StartIndex = 0,
				EndIndex = 28
			};

			robot.RegisterPathSceneNode( linkNode );

			return robot;
		}
	}
}
