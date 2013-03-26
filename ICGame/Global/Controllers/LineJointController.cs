using System;
using System.Collections.Generic;
using FarseerPhysics.Dynamics;
using Microsoft.Xna.Framework;
using VertexArmy.Global.Behaviours;
using VertexArmy.Utilities;

namespace VertexArmy.Global.Controllers
{
	public class LineJointController : IController, IUpdatable
	{
		private Vector2 _lastBodyPosition;
		private float _lastBodyRotation;

		private const float RotationError = 0.00001f;
		private const float PositionError = 0.00001f;

		public LineJointController( ITransformable transformable, Body body1, Body body2 )
		{
			IParameter transParam = new ParameterTransformable
									{
										Alive = true,
										Input = false,
										Null = false,
										Output = true,
										Value = transformable
									};


			IParameter bodyParam1 = new ParameterBody
								   {
									   Alive = true,
									   Input = true,
									   Null = false,
									   Output = false,
									   Value = body1
								   };

			IParameter bodyParam2 = new ParameterBody
			{
				Alive = true,
				Input = true,
				Null = false,
				Output = false,
				Value = body2
			};

			IParameter positionParam = new ParameterVector2
				{
					Alive = true,
					Input = true,
					Null = false,
					Output = false,
					Value = Vector2.Zero
				};

			IParameter rotationParam = new ParameterFloat
			{
				Alive = true,
				Input = true,
				Null = false,
				Output = false,
				Value = 0f
			};

			Data = new List<IParameter> { transParam, bodyParam1, bodyParam2, positionParam, rotationParam };
		}

		public Body Body1
		{
			get { return ( ( ParameterBody ) Data[1] ).Value; }
		}

		public Body Body2
		{
			get { return ( ( ParameterBody ) Data[2] ).Value; }
		}

		public ITransformable Transformable
		{
			get { return ( ( ParameterTransformable ) Data[0] ).Value; }
		}

		public void Update( GameTime dt )
		{
			ParameterTransformable trans = Data[0] as ParameterTransformable;
			ParameterBody body1 = Data[1] as ParameterBody;
			ParameterBody body2 = Data[2] as ParameterBody;

			bool ok = ( trans != null && body1 != null && body2 != null );

			if ( !ok ) return;

			Vector2 line = body2.Value.Position - body1.Value.Position;
			Vector2 newPosition = line / 2;

			line.Normalize();

			float newRotation = ( float ) Math.Acos( line.X ) * Math.Sign( Math.Asin( line.Y ) );

			float rotationDelta = Math.Abs( newRotation - _lastBodyRotation );
			float positionDelta = ( newPosition - _lastBodyPosition ).LengthSquared();

			if ( rotationDelta > RotationError || positionDelta > PositionError )
			{
				List<IParameter> parameters = Data;
				( ( ParameterVector2 ) parameters[3] ).Value = newPosition;
				( ( ParameterFloat ) parameters[4] ).Value = newRotation;
				DirectCompute( ref parameters );

				_lastBodyPosition = newPosition;
				_lastBodyRotation = newRotation;
			}
		}

		public void DirectCompute( ref List<IParameter> data )
		{
			ParameterTransformable trans = data[0] as ParameterTransformable;
			ParameterBody body1 = Data[1] as ParameterBody;
			ParameterBody body2 = Data[2] as ParameterBody;
			ParameterVector2 position = data[3] as ParameterVector2;
			ParameterFloat rotation = data[4] as ParameterFloat;

			bool apply = ( trans != null && body1 != null && body2 != null );

			if ( !apply ) return;

			apply = !trans.Null && !body1.Null && !body2.Null;
			apply = apply && ( trans.Output && body1.Input && body2.Input );
			apply = apply && ( trans.Alive && body1.Alive && body2.Alive );

			if ( !apply ) return;

			trans.Value.SetPosition( new Vector3( UnitsConverter.ToDisplayUnits( position.Value ), 0f ) );
			trans.Value.SetRotation( UnitsConverter.To3DRotation( rotation.Value ) );
		}

		public List<IParameter> Data { get; set; }

		public void Clean()
		{

		}
	}
}
