using System.Collections.Generic;
using FarseerPhysics.Dynamics.Joints;
using Microsoft.Xna.Framework;
using VertexArmy.GameWorld.Prefabs;
using VertexArmy.GameWorld.Prefabs.Structs;

namespace VertexArmy.Content.Prefabs
{
	public class ButtonPrefab
	{
		public static PrefabEntity CreatePrefab()
		{
			PrefabEntity button = new PrefabEntity { Name = "Button" };

			ShapePrefab caseBottom = new ShapePrefab
			{
				Density = 1f,
				Width = 80f,
				Height = 5f,
				Offset = Vector2.Zero,
				Type = ShapeType.Rectangle
			};

			ShapePrefab caseWall1 = new ShapePrefab
			{
				Density = 1f,
				Width = 10f,
				Height = 15f,
				Offset = new Vector2( -35f, 10f ),
				Type = ShapeType.Rectangle
			};

			ShapePrefab caseWall2 = new ShapePrefab
			{
				Density = 1f,
				Width = 10f,
				Height = 15f,
				Offset = new Vector2( 35f, 10f ),
				Type = ShapeType.Rectangle
			};

			BodyPrefab buttonCaseBody = new BodyPrefab
								   {
									   Friction = 1f,
									   LocalPosition = Vector2.Zero,
									   Name = "ButtonCase",
									   Shapes = new List<ShapePrefab> { caseBottom, caseWall1, caseWall2 },
									   Restitution = 0.1f,
									   Static = true
								   };

			button.RegisterBody( buttonCaseBody, true );

			ShapePrefab buttonShape = new ShapePrefab
			{
				Density = 0.3f,
				Width = 58f,
				Height = 10f,
				Offset = Vector2.Zero,
				Type = ShapeType.Rectangle
			};

			BodyPrefab buttonBody = new BodyPrefab
			{
				Friction = 1f,
				LocalPosition = new Vector2( 0f, 17.5f ),
				Name = "Button",
				Shapes = new List<ShapePrefab> { buttonShape },
				Restitution = 0.1f,
				Static = false
			};

			button.RegisterBody( buttonBody );

			JointPrefab sliderJoint1 = new JointPrefab
			{
				Name = "ButtonJoint1",
				Type = JointType.Distance,
				Body1 = "Button",
				Body2 = "ButtonCase",
				Length = 17.5f,
				Anchor = new Vector2( -20f, 0f ),
				Anchor2 = new Vector2( -20f, 0f ),
				CollideConnected = true,
				Frequency = 3f,
				DampingRatio = 0.3f
			};

			JointPrefab sliderJoint2 = new JointPrefab
			{
				Name = "ButtonJoint2",
				Type = JointType.Distance,
				Body1 = "Button",
				Body2 = "ButtonCase",
				Length = 17.5f,
				Anchor = new Vector2( 20f, 0f ),
				Anchor2 = new Vector2( 20f, 0f ),
				CollideConnected = true,
				Frequency = 3f,
				DampingRatio = 0.3f
			};

			button.RegisterJoint( sliderJoint1 );
			button.RegisterJoint( sliderJoint2 );

			MeshSceneNodePrefab caseNode = new MeshSceneNodePrefab
			{
				Name = "CaseNode",
				Mesh = "models/button_body",
				Material = "CelShadingMaterial",
			};

			MeshSceneNodePrefab buttonNode = new MeshSceneNodePrefab
			{
				Name = "ButtonNode",
				Mesh = "models/button_head",
				Material = "CelShadingMaterial",
			};

			button.RegisterMeshSceneNode( caseNode );
			button.RegisterMeshSceneNode( buttonNode );

			ControllerPrefab buttonController = new ControllerPrefab
			{
				Name = "ButtonController",
				Type = ControllerType.BodyController,
				Body = "Button",
				Transformable = "ButtonNode"
			};

			ControllerPrefab caseController = new ControllerPrefab
			{
				Name = "CaseController",
				Type = ControllerType.BodyController,
				Body = "ButtonCase",
				Transformable = "CaseNode"
			};

			button.RegisterController( buttonController );
			button.RegisterController( caseController );

			return button;
		}
	}
}
