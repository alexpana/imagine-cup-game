using System.Collections.Generic;
using Microsoft.Xna.Framework;
using VertexArmy.Global.Behaviours;
using VertexArmy.Graphics;

using RankBoundingBox = System.Collections.Generic.KeyValuePair<int, Microsoft.Xna.Framework.BoundingBox>;
using RankBoundingSphere = System.Collections.Generic.KeyValuePair<int, Microsoft.Xna.Framework.BoundingSphere>;


namespace VertexArmy.Global.Controllers
{
	public class WorldShapeCollisionController : IController
	{
		
		
		private RankBoundingBox ToRankedBoundingBox( BoundingBox box )
		{
			if ( Data[2] == null )
				Data[2] = new List<RankBoundingBox>();

			List<RankBoundingBox> blist = Data[2] as List<RankBoundingBox>;
			return blist == null ? new RankBoundingBox(-1, new BoundingBox()) : new RankBoundingBox(blist.Count, box);
		}

		private RankBoundingSphere ToRankedBoundingSphere( BoundingSphere sphere )
		{
			if ( Data[3] == null )
				Data[3] = new List<RankBoundingSphere>();

			List<RankBoundingSphere> blist = Data[3] as List<RankBoundingSphere>;
			return blist == null ? new RankBoundingSphere( -1, new BoundingSphere() ) : new RankBoundingSphere( blist.Count, sphere );
		}

		WorldShapeCollisionController()
		{
			Data = new List<object>( 4 );
		}
		/// <param name="node">

		/// </param>
		public void SetSubject( SceneNode node )
		{
			Data[0] = node;
		}

		private readonly Dictionary<int, IShapeListener> _boxListeners = new Dictionary<int, IShapeListener>();
		private readonly Dictionary<int, IShapeListener> _sphereListeners = new Dictionary<int, IShapeListener>();

		/// <param name="shapeListener">

		/// </param>
		public void Register( IShapeListener shapeListener, BoundingBox box )
		{
			if ( Data[1] == null )
				Data[1] = new List<IShapeListener>();

			RankBoundingBox cbox;
			if ( Data[2] == null )
				Data[2] = new List<RankBoundingBox>();
			( ( List<RankBoundingBox> ) Data[2] ).Add( cbox = ToRankedBoundingBox( box ) );
			_boxListeners[cbox.Key] = shapeListener;

			( ( List<IShapeListener> ) Data[1] ).Add( shapeListener );
		}

		public void Register( IShapeListener shapeListener, BoundingSphere sphere )
		{
			if ( Data[1] == null )
				Data[1] = new List<IShapeListener>();

			RankBoundingSphere csphere;
			if ( Data[3] == null )
				Data[3] = new List<RankBoundingSphere>();
			( ( List<RankBoundingSphere> ) Data[3] ).Add( csphere = ToRankedBoundingSphere( sphere ) );
			_sphereListeners[csphere.Key] = shapeListener;

			( ( List<IShapeListener> ) Data[1] ).Add( shapeListener );
		}

		private readonly List<RankBoundingBox> _insideBBox = new List<RankBoundingBox>();
		private readonly List<RankBoundingSphere> _insideBSphere = new List<RankBoundingSphere>();


		private void UpdateBoxIntersections()
		{
			SceneNode node = Data[0] as SceneNode;

			if ( node == null )
				return;

			if ( Data[2] != null )
			{
				List<RankBoundingBox> boxlist = Data[2] as List<RankBoundingBox>;

				if ( boxlist == null )
					return;


				List<RankBoundingBox> currentIntersection = new List<RankBoundingBox>();


				foreach ( RankBoundingBox boundingBox in boxlist )
				{
					if ( boundingBox.Value.Contains( node.GetAbsolutePosition() ) != ContainmentType.Disjoint )
						currentIntersection.Add( boundingBox );
				}

				//compute the set difference between the two lists
				int i = 0, j = 0;

				List<RankBoundingBox> enterBoxes = new List<RankBoundingBox>();
				List<RankBoundingBox> exitBoxes = new List<RankBoundingBox>();
				List<RankBoundingBox> insideBoxes = new List<RankBoundingBox>();

				while ( i < currentIntersection.Count && j < _insideBBox.Count )
				{
					if ( currentIntersection[i].Key == _insideBBox[j].Key )
					{
						insideBoxes.Add( currentIntersection[i] );
						i++;
						j++;
					}

					else if ( currentIntersection[i].Key < _insideBBox[j].Key )
					{
						enterBoxes.Add( currentIntersection[i] );
						i++;
					}

					else if ( currentIntersection[i].Key > _insideBBox[j].Key )
					{
						exitBoxes.Add( _insideBBox[j] );
						j++;
					}
				}

				while ( i < currentIntersection.Count )
					enterBoxes.Add( currentIntersection[i++] );

				while ( j < _insideBBox.Count )
					exitBoxes.Add( _insideBBox[j++] );

				foreach ( RankBoundingBox keyValuePair in enterBoxes )
				{
					_boxListeners[keyValuePair.Key].OnEnterShape();
				}

				foreach ( RankBoundingBox keyValuePair in exitBoxes )
				{
					_boxListeners[keyValuePair.Key].OnExitShape();
				}

				foreach ( RankBoundingBox keyValuePair in insideBoxes )
				{
					_boxListeners[keyValuePair.Key].OnEachFrameInsideShape();
				}
			}
		}

		private void UpdateSphereIntersections()
		{
			SceneNode node = Data[0] as SceneNode;

			if ( node == null )
				return;


			if ( Data[3] != null )
			{
				List<RankBoundingSphere> spherelist = Data[3] as List<RankBoundingSphere>;

				if ( spherelist == null )
					return;


				List<RankBoundingSphere> currentIntersection = new List<RankBoundingSphere>();


				foreach ( RankBoundingSphere boundingSphere in spherelist )
				{
					if ( boundingSphere.Value.Contains( node.GetAbsolutePosition() ) != ContainmentType.Disjoint )
						currentIntersection.Add( boundingSphere );
				}

				//compute the set difference between the two lists
				int i = 0, j = 0;

				List<RankBoundingSphere> enterSpheres = new List<RankBoundingSphere>();
				List<RankBoundingSphere> exitSpheres = new List<RankBoundingSphere>();
				List<RankBoundingSphere> insideSpheres = new List<RankBoundingSphere>();

				while ( i < currentIntersection.Count && j < _insideBSphere.Count )
				{
					if ( currentIntersection[i].Key == _insideBSphere[j].Key )
					{
						insideSpheres.Add( currentIntersection[i] );
						i++;
						j++;
					}

					else if ( currentIntersection[i].Key < _insideBSphere[j].Key )
					{
						enterSpheres.Add( currentIntersection[i] );
						i++;
					}

					else if ( currentIntersection[i].Key > _insideBSphere[j].Key )
					{
						exitSpheres.Add( _insideBSphere[j] );
						j++;
					}
				}

				while ( i < currentIntersection.Count )
					enterSpheres.Add( currentIntersection[i++] );

				while ( j < _insideBBox.Count )
					exitSpheres.Add( _insideBSphere[j++] );

				foreach ( RankBoundingSphere keyValuePair in enterSpheres )
				{
					_sphereListeners[keyValuePair.Key].OnEnterShape();
				}

				foreach ( RankBoundingSphere keyValuePair in exitSpheres )
				{
					_sphereListeners[keyValuePair.Key].OnExitShape();
				}

				foreach ( RankBoundingSphere keyValuePair in insideSpheres )
				{
					_sphereListeners[keyValuePair.Key].OnEachFrameInsideShape();
				}
			}
		}

		public void Update( GameTime dt )
		{
			if ( Data == null )
				return;

			if(Data[0] == null)
				return;

			if ( Data[1] == null )
				return;
			
			UpdateBoxIntersections();
			UpdateSphereIntersections();
		}

		public List<object> Data { get; set; }
		public void Clean()
		{
			throw new System.NotImplementedException();
		}
	}
}