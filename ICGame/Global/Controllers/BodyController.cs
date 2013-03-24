using System;
using System.Collections.Generic;
using FarseerPhysics.Dynamics;
using Microsoft.Xna.Framework;
using VertexArmy.Global.Behaviours;
using VertexArmy.Utilities;

namespace VertexArmy.Global.Controllers
{
	public class BodyController : IController, IUpdatable
	{
		private Vector2 _lastBodyPosition;
		private float _lastBodyRotation;

		private const float RotationError = 0.00001f;
		private const float PositionError = 0.00001f;

		public BodyController( ITransformable transformable, Body body )
		{
			IParameter transParam = new ParameterTransformable
									{
										Alive = true,
										Input = false,
										Null = false,
										Output = true,
										Value = transformable
									};


			IParameter bodyParam = new ParameterBody
								   {
									   Alive = true,
									   Input = true,
									   Null = false,
									   Output = false,
									   Value = body
								   };

			Data = new List<IParameter> { transParam, bodyParam };
		}

		public Body Body
		{
			get { return ( ( ParameterBody ) Data[1] ).Value; }
		}

		public ITransformable Transformable
		{
			get { return ( ( ParameterTransformable ) Data[0] ).Value; }
		}

		public void Update( GameTime dt )
		{
			ParameterTransformable trans = Data[0] as ParameterTransformable;
			ParameterBody body = Data[1] as ParameterBody;

			bool ok = ( trans != null && body != null );

			if ( !ok ) return;


			float rotationDelta = Math.Abs( body.Value.Rotation - _lastBodyRotation );
			float positionDelta = ( body.Value.Position - _lastBodyPosition ).LengthSquared();

			if ( rotationDelta > RotationError || positionDelta > PositionError )
			{
				List<IParameter> parameters = Data;
				DirectCompute( ref parameters );

				_lastBodyPosition = body.Value.Position;
				_lastBodyRotation = body.Value.Rotation;
			}
		}

		public void DirectCompute( ref List<IParameter> data )
		{
			ParameterTransformable trans = data[0] as ParameterTransformable;
			ParameterBody body = data[1] as ParameterBody;

			bool apply = ( trans != null && body != null );

			if ( !apply ) return;

			apply = !trans.Null && !body.Null;
			apply = apply && ( trans.Output && body.Input );
			apply = apply && ( trans.Alive && body.Alive );

			if ( !apply ) return;

			trans.Value.SetPosition( new Vector3( UnitsConverter.ToDisplayUnits( body.Value.Position ), 0f ) );
			trans.Value.SetRotation( UnitsConverter.To3DRotation( body.Value.Rotation ) );
		}

		public List<IParameter> Data { get; set; }
	}
}
