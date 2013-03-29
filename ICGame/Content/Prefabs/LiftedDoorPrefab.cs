using System.Collections.Generic;
using FarseerPhysics.Dynamics.Joints;
using Microsoft.Xna.Framework;
using VertexArmy.GameWorld.Prefabs;
using VertexArmy.GameWorld.Prefabs.Structs;

namespace VertexArmy.Content.Prefabs
{
	public class LiftedDoorPrefab
	{
		public static PrefabEntity CreatePrefab()
		{
			PrefabEntity liftedDoor = new PrefabEntity { Name = "LiftedDoor" };

			ShapePrefab caseTop = new ShapePrefab
			{
				Density = 1f,
				Width = 60f,
				Height = 5f,
				Offset = Vector2.Zero,
				Type = ShapeType.Rectangle
			};

			ShapePrefab caseWall1 = new ShapePrefab
			{
				Density = 1f,
				Width = 10f,
				Height = 150f,
				Offset = new Vector2( -25f, -77.5f ),
				Type = ShapeType.Rectangle
			};

			ShapePrefab caseWall2 = new ShapePrefab
			{
				Density = 1f,
				Width = 10f,
				Height = 150f,
				Offset = new Vector2( 25f, -77.5f ),
				Type = ShapeType.Rectangle
			};

			BodyPrefab doorCaseBody = new BodyPrefab
								   {
									   Friction = 1f,
									   LocalPosition = Vector2.Zero,
									   Name = "DoorCase",
									   Shapes = new List<ShapePrefab> { caseTop, caseWall1, caseWall2 },
									   Restitution = 0.1f,
									   Static = true
								   };

			liftedDoor.RegisterBody( doorCaseBody, true );

			ShapePrefab doorShape = new ShapePrefab
			{
				Density = 0.3f,
				Width = 38f,
				Height = 180f,
				Offset = Vector2.Zero,
				Type = ShapeType.Rectangle
			};

			BodyPrefab doorBody = new BodyPrefab
			{
				Friction = 1f,
				LocalPosition = new Vector2( 0f, -120f ),
				Name = "Door",
				Shapes = new List<ShapePrefab> { doorShape },
				Restitution = 0.1f,
				Static = false
			};

			liftedDoor.RegisterBody( doorBody );

			JointPrefab sliderJoint = new JointPrefab
			{
				Name = "DoorJoint",
				Type = JointType.Distance,
				Body1 = "Door",
				Body2 = "DoorCase",
				Length = 150f,
				Anchor = new Vector2( 0f, 0f ),
				Anchor2 = new Vector2( 0f, 0f ),
				CollideConnected = true,
				Frequency = 3f,
				DampingRatio = 0.3f
			};

			liftedDoor.RegisterJoint( sliderJoint );

			return liftedDoor;
		}
	}
}
