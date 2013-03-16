
using System.Collections.Generic;
using FarseerPhysics.Common;
using FarseerPhysics.Dynamics.Joints;
using Microsoft.Xna.Framework;

namespace VertexArmy.GameWorld.Prefabs
{
	public class RobotPrefab
	{
		public static PrefabEntity CreatePrefab()
		{
			PrefabEntity robot = new PrefabEntity { Name = "Robot", PhysicsScale = 1f };

			float scale = 1f;

			/* gear bodies */
			ShapePrefab gearShape = new ShapePrefab { Type = ShapeType.Circle, XRadius = 1.215f * scale, Density = 1f };

			BodyPrefab gear1 = new BodyPrefab
			{
				Name = "Gear1",
				Friction = 1f,
				Restitution = 0.1f,
				LocalPosition = new Vector2( -2.301f * scale, 1.243f * scale ),
				Static = false,
				Shapes = new List<ShapePrefab> { gearShape }
			};

			BodyPrefab gear2 = new BodyPrefab
			{
				Name = "Gear2",
				Friction = 1f,
				Restitution = 0.1f,
				LocalPosition = new Vector2( 2.301f * scale, 1.243f * scale ),
				Static = false,
				Shapes = new List<ShapePrefab> { gearShape }
			};

			BodyPrefab gear3 = new BodyPrefab
			{
				Name = "Gear3",
				Friction = 1f,
				Restitution = 0.1f,
				LocalPosition = new Vector2( 0f * scale, -2.615f * scale ),
				Static = false,
				Shapes = new List<ShapePrefab> { gearShape }
			};

			robot.RegisterBody( gear1 );
			robot.RegisterBody( gear2 );
			robot.RegisterBody( gear3 );

			/* chassis body */
			ShapePrefab chassisShape = new ShapePrefab { Type = ShapeType.Polygon, Density = 1f, Polygon = new List<Vertices> { new Vertices( ) } };
			chassisShape.Polygon[0].Add( new Vector2( -1.215f * scale, 0.7f * scale ) );
			chassisShape.Polygon[0].Add( new Vector2( 0f * scale, -1.4f * scale ) );
			chassisShape.Polygon[0].Add( new Vector2( 1.215f * scale, 0.7f * scale ) );

			BodyPrefab chassis = new BodyPrefab
			{
				Name = "Chassis",
				Restitution = 0.1f,
				Friction = 1f,
				LocalPosition = new Vector2( 0f * scale, 0f * scale ),
				Static = false,
				Shapes = new List<ShapePrefab> { chassisShape }
			};

			robot.RegisterBody( chassis, true ); // is main component

			/* gear joints */
			const float jointLengthModifier = 1f;

			JointPrefab gearJoint1 = new JointPrefab
			{
				Type = JointType.Line,
				Name = "GearJoint1",
				Body1 = "Chassis",
				Body2 = "Gear1",
				Anchor = gear1.LocalPosition,
				Axis = new Vector2( 0.66f, -0.33f ) * jointLengthModifier
			};

			JointPrefab gearJoint2 = new JointPrefab
			{
				Type = JointType.Line,
				Name = "GearJoint2",
				Body1 = "Chassis",
				Body2 = "Gear2",
				Anchor = gear2.LocalPosition,
				Axis = new Vector2( -0.66f, -0.33f ) * jointLengthModifier
			};

			JointPrefab gearJoint3 = new JointPrefab
			{
				Type = JointType.Line,
				Name = "GearJoint3",
				Body1 = "Chassis",
				Body2 = "Gear3",
				Anchor = gear1.LocalPosition,
				Axis = new Vector2( 0f, 0.76f ) * jointLengthModifier
			};

			robot.RegisterJoint( gearJoint1 );
			robot.RegisterJoint( gearJoint2 );
			robot.RegisterJoint( gearJoint3 );

			/* scene nodes */
			SceneNodesPrefab gearNode1 = new SceneNodesPrefab
			{
				Name = "Gear1",
				Mesh = "models/robo_wheel",
				Material = "RobotMaterial",
				Body = "Gear1"
			};

			SceneNodesPrefab gearNode2 = new SceneNodesPrefab
			{
				Name = "Gear2",
				Mesh = "models/robo_wheel",
				Material = "RobotMaterial",
				Body = "Gear2"
			};

			SceneNodesPrefab gearNode3 = new SceneNodesPrefab
			{
				Name = "Gear3",
				Mesh = "models/robo_wheel",
				Material = "RobotMaterial",
				Body = "Gear3"
			};

			robot.RegisterSceneNode( gearNode1 );
			robot.RegisterSceneNode( gearNode2 );
			robot.RegisterSceneNode( gearNode3 );

			SceneNodesPrefab chassisNode = new SceneNodesPrefab
			{
				Name = "Chassis",
				Mesh = "models/robo_link",
				Material = "RobotMaterial",
				Body = "Chassis"
			};

			robot.RegisterSceneNode( chassisNode );

			return robot;
		}
	}
}
