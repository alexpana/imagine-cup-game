using System.Collections.Generic;
using FarseerPhysics.Common;
using FarseerPhysics.Dynamics;
using FarseerPhysics.Factories;
using Microsoft.Xna.Framework;
using VertexArmy.Utilities;

namespace VertexArmy.GameWorld.Prefabs.Structs
{
	public class ShapePrefab
	{
		public ShapeType Type { get; set; }

		public float XRadius, YRadius;
		public float Density;

		public int Edges;

		public List<Vertices> Polygon;
		public Vector2 Start, End;
		public Vector2 Offset;

		public float Width, Height;

		public void AttachToBody( Body body, float scale )
		{
			switch ( Type )
			{
				case ShapeType.Circle:
					FixtureFactory.AttachCircle( UnitsConverter.ToSimUnits( XRadius ) * scale, Density, body );
					break;
				case ShapeType.Ellipse:
					FixtureFactory.AttachEllipse( UnitsConverter.ToSimUnits( XRadius ) * scale, UnitsConverter.ToSimUnits( YRadius ) * scale, Edges, Density, body );
					break;
				case ShapeType.Edge:
					FixtureFactory.AttachEdge( UnitsConverter.ToSimUnits( Start ) * scale, UnitsConverter.ToSimUnits( End ) * scale, body );
					break;
				case ShapeType.Rectangle:
					FixtureFactory.AttachRectangle( UnitsConverter.ToSimUnits( Width ) * scale, UnitsConverter.ToSimUnits( Height ) * scale, Density, UnitsConverter.ToSimUnits( Offset ) * scale, body );
					break;
				case ShapeType.Polygon:

					Vertices p = new Vertices();
					foreach ( Vector2 node in Polygon[0] )
					{
						p.Add( UnitsConverter.ToSimUnits( node ) * scale );
					}

					FixtureFactory.AttachPolygon( p, Density, body );
					break;
				case ShapeType.CompoundPolygon:

					List<Vertices> cp = new List<Vertices>();
					foreach ( Vertices v in Polygon )
					{
						Vertices polygon = new Vertices();
						foreach ( Vector2 node in v )
						{
							polygon.Add( UnitsConverter.ToSimUnits( node ) * scale );
						}
						cp.Add( polygon );
					}

					FixtureFactory.AttachCompoundPolygon( cp, Density, body );
					break;
				default:
					return;
			}
		}
	}
}