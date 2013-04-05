using System.Collections.Generic;
using FarseerPhysics.Common;
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
				Width = 26f,
				Height = 0.5f,
				Offset = new Vector2( 0f, 0.25f ),
				Type = ShapeType.Rectangle
			};

			ShapePrefab caseWall1 = new ShapePrefab { Type = ShapeType.Polygon, Density = 1f, Polygon = new List<Vertices> { new Vertices() } };
			caseWall1.Polygon[0].Add( new Vector2( 8.25f, 0.5f ) );
			caseWall1.Polygon[0].Add( new Vector2( 8.25f, 4f ) );
			caseWall1.Polygon[0].Add( new Vector2( 13f, 0.5f ) );

			ShapePrefab caseWall2 = new ShapePrefab { Type = ShapeType.Polygon, Density = 1f, Polygon = new List<Vertices> { new Vertices() } };
			caseWall2.Polygon[0].Add( new Vector2( -8.25f, 4f ) );
			caseWall2.Polygon[0].Add( new Vector2( -8.25f, 0.5f ) );
			caseWall2.Polygon[0].Add( new Vector2( -13f, 0.5f ) );

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
				Width = 16.5f,
				Height = 2.4f,
				Offset = Vector2.Zero,
				Type = ShapeType.Rectangle
			};

			BodyPrefab buttonBody = new BodyPrefab
			{
				Friction = 1f,
				LocalPosition = new Vector2( 0f, 4f ),
				Name = "Button",
				Shapes = new List<ShapePrefab> { buttonShape },
				Restitution = 0.1f,
				Static = false
			};

			button.RegisterBody( buttonBody );

			JointPrefab prismaticJoint = new JointPrefab
			{
				Name = "ButtonJoint1",
				Type = JointType.Prismatic,
				Body1 = "Button",
				Body2 = "ButtonCase",
				Anchor = new Vector2( 0f, -1.25f ),
				Anchor2 = new Vector2( 0f, 0f ),
				Axis = new Vector2( 0f, 1f ),
				CollideConnected = false,
				UpperLimit = 2.25f,
				LowerLimit = 0f,
				LimitEnabled = true,
				MaxMotorForce = 0.2f,
				MotorSpeed = 1f,
				MotorEnabled = true
			};

			button.RegisterJoint( prismaticJoint );

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
