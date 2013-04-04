using System.Collections.Generic;
using Microsoft.Xna.Framework;
using VertexArmy.GameWorld.Prefabs;
using VertexArmy.GameWorld.Prefabs.Structs;

namespace VertexArmy.Content.Prefabs
{
	public class FloorPrefab
	{
		public static PrefabEntity CreatePrefab()
		{
			PrefabEntity floor = new PrefabEntity { Name = "Floor" };

			ShapePrefab floorShape = new ShapePrefab
			{
				Density = 1f,
				Width = 60f,
				Height = 10f,
				Offset = Vector2.Zero,
				Type = ShapeType.Rectangle
			};

			BodyPrefab floorBody = new BodyPrefab
			{
				Friction = 1f,
				LocalPosition = Vector2.Zero,
				Name = "FloorBody",
				Shapes = new List<ShapePrefab> { floorShape },
				Restitution = 0.1f,
				Static = true,
			};

			floor.RegisterBody( floorBody, true );

			return floor;
		}
	}
}
