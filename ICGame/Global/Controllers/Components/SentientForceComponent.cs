using System;
using System.Collections.Generic;
using FarseerPhysics.Common;
using FarseerPhysics.Dynamics;
using FarseerPhysics.Dynamics.Contacts;
using FarseerPhysics.Factories;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Input;
using VertexArmy.GameWorld;
using VertexArmy.Global.Managers;
using VertexArmy.Graphics.Attachables;
using VertexArmy.Utilities;

namespace VertexArmy.Global.Controllers.Components
{
	public class SentientForceComponent : BaseComponent
	{
		public const float Distance = 350f;
		public const float Angle = 0.8f;
		public const float AttractionForce = 10f;
		public const float RepulsiveForce = 8f;

		private Vector2 _oldPosition;
		private float _distanceSim;
		private Category _collisionCategory;

		public Body Cone;
		public GameEntity ConeEntity;

		public SentientForceComponent()
		{
			Data = new List<object>();

			_type = ComponentType.SentientForce;

			float distance = UnitsConverter.ToSimUnits( Distance );
			Vertices coneShape = new Vertices();

			coneShape.Add( Vector2.Zero );

			coneShape.Add( new Vector2( ( float ) Math.Cos( -Angle / 2 ), ( float ) Math.Sin( -Angle / 2 ) ) * distance );
			coneShape.Add( new Vector2( ( float ) Math.Cos( -Angle / 4 ), ( float ) Math.Sin( -Angle / 4 ) ) * distance );
			coneShape.Add( new Vector2( 1f, 0f ) * distance );
			coneShape.Add( new Vector2( ( float ) Math.Cos( Angle / 4 ), ( float ) Math.Sin( Angle / 4 ) ) * distance );
			coneShape.Add( new Vector2( ( float ) Math.Cos( Angle / 2 ), ( float ) Math.Sin( Angle / 2 ) ) * distance );

			Cone = new Body( Platform.Instance.PhysicsWorld );
			Cone.IsSensor = true;
			Cone.BodyType = BodyType.Dynamic;

			Cone.Position = Vector2.Zero;
			FixtureFactory.AttachPolygon( coneShape, 0f, Cone );

			_oldPosition = Vector2.Zero;
			_distanceSim = UnitsConverter.ToSimUnits( Distance );

			if ( GameWorldManager.Instance.GetEntity( "saf1" ) == null )
			{
				GameWorldManager.Instance.SpawnEntity( "Saf", "saf1", new Vector3( 0, 0, 0 ), 15 );
			}
			ConeEntity = GameWorldManager.Instance.GetEntity( "saf1" );
		}

		public override void InitEntity()
		{
			Entity.PhysicsEntity.IgnoreCollisionWith( Cone );
			PhysicsContactManager.Instance.RegisterBeginCallback( ContactCallbackType.FixtureBBegin, Cone, BeginContactB );
			PhysicsContactManager.Instance.RegisterBeginCallback( ContactCallbackType.FixtureABegin, Cone, BeginContactA );
			Cone.CollidesWith = Entity.PhysicsEntity.GetCollisionLayer();
			Cone.CollisionCategories = Entity.PhysicsEntity.GetCollisionLayer();
			_collisionCategory = Entity.PhysicsEntity.GetCollisionLayer();
		}


		public override void Update( GameTime dt )
		{
			if ( Entity != null )
			{
				if ( !_collisionCategory.Equals( Entity.PhysicsEntity.GetCollisionLayer() ) )
				{
					Cone.CollidesWith = Entity.PhysicsEntity.GetCollisionLayer();
					Cone.CollisionCategories = Entity.PhysicsEntity.GetCollisionLayer();
					_collisionCategory = Entity.PhysicsEntity.GetCollisionLayer();
				}
				_oldPosition = Cone.Position;
				Vector3 newPosition3D = UnitsConverter.ToSimUnits( Entity.GetPosition() );
				Cone.Position = new Vector2( newPosition3D.X, newPosition3D.Y );

				if ( true )
				{
					//double dTime = ( data[1] as ParameterGameTime ).Value.ElapsedGameTime.TotalSeconds;

					Vector3 followPosition3D = UnitsConverter.ToSimUnits( SceneManager.Instance.IntersectScreenRayWithPlane( Entity.GetPosition().Z ) );
					Vector2 followPosition = new Vector2( followPosition3D.X, followPosition3D.Y );

					Vector2 direction = followPosition - Cone.Position;
					direction.Normalize();

					Cone.Rotation = ( float ) Math.Acos( direction.X ) * Math.Sign( ( float ) Math.Asin( direction.Y ) );
					if ( Mouse.GetState().LeftButton.Equals( ButtonState.Pressed ) )
					{
						ConeEntity.SetPosition( new Vector3( UnitsConverter.ToDisplayUnits( Cone.Position ), Entity.GetPosition().Z ) );
						ConeEntity.SetRotation( UnitsConverter.To3DRotation( Cone.Rotation ) );
						( ( SafAttachable ) ConeEntity.SceneNodes["Mesh"].Attachable[0] ).Material.SetParameter( "fVel", new Vector2( 0.0000f, 0.00025f ) );
						ConeEntity.SceneNodes["Mesh"].Invisible = false;
					}
					else if ( Mouse.GetState().RightButton.Equals( ButtonState.Pressed ) )
					{
						ConeEntity.SetPosition( new Vector3( UnitsConverter.ToDisplayUnits( Cone.Position ), Entity.GetPosition().Z ) );
						ConeEntity.SetRotation( UnitsConverter.To3DRotation( Cone.Rotation ) );
						( ( SafAttachable ) ConeEntity.SceneNodes["Mesh"].Attachable[0] ).Material.SetParameter( "fVel", new Vector2( 0.0000f, -0.00025f ) );
						ConeEntity.SceneNodes["Mesh"].Invisible = false;
					}
					else
					{
						ConeEntity.SceneNodes["Mesh"].Invisible = true;
					}
				}
				else
				{
					Cone.Rotation = Entity.GetRotationRadians();
				}
			}
		}

		public bool BeginContactB( Contact c )
		{
			if ( c.FixtureA.Body.IsStatic )
			{
				return false;
			}
			if ( Mouse.GetState().LeftButton.Equals( ButtonState.Pressed ) )
			{
				Vector2 forceDirection = _oldPosition - c.FixtureA.Body.Position;
				//float ratio = 1 - ( forceDirection.Length() / _distanceSim );
				float ratio = 1f;
				forceDirection.Normalize();
				Vector2 force = forceDirection * ratio * AttractionForce;
				c.FixtureA.Body.ApplyForce( force );
				Entity.MainBody.ApplyForce( -force * 2 );
			}
			else if ( Mouse.GetState().RightButton.Equals( ButtonState.Pressed ) )
			{
				Vector2 forceDirection = c.FixtureA.Body.Position - _oldPosition;
				//float ratio = 1 - ( forceDirection.Length() / _distanceSim );
				float ratio = 1f;
				forceDirection.Normalize();
				Vector2 force = forceDirection * ratio * RepulsiveForce;
				c.FixtureA.Body.ApplyForce( force );
				Entity.MainBody.ApplyForce( -force * 2 );
			}

			return false;
		}

		public bool BeginContactA( Contact c )
		{
			if ( c.FixtureB.Body.IsStatic )
			{
				return false;
			}
			if ( Mouse.GetState().LeftButton.Equals( ButtonState.Pressed ) )
			{
				Vector2 forceDirection = _oldPosition - c.FixtureB.Body.Position;
				//float ratio = 1 - ( forceDirection.Length() / _distanceSim );
				float ratio = 1f;
				forceDirection.Normalize();
				c.FixtureB.Body.ApplyForce( forceDirection * ratio * AttractionForce );
			}
			else if ( Mouse.GetState().RightButton.Equals( ButtonState.Pressed ) )
			{
				Vector2 forceDirection = c.FixtureB.Body.Position - _oldPosition;
				//float ratio = 1 - ( forceDirection.Length() / _distanceSim );
				float ratio = 1f;
				forceDirection.Normalize();
				c.FixtureB.Body.ApplyForce( forceDirection * ratio * RepulsiveForce );
			}

			return false;
		}

		public override void Clean()
		{
			PhysicsContactManager.Instance.UnregisterCallback( ContactCallbackType.FixtureBBegin, Cone );
			PhysicsContactManager.Instance.UnregisterCallback( ContactCallbackType.FixtureABegin, Cone );
			Platform.Instance.PhysicsWorld.RemoveBody( Cone );
			GameWorldManager.Instance.RemoveEntity( "saf1" );
		}
	}
}
