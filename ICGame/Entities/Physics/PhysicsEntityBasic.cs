
using System.Runtime.Serialization;
using FarseerPhysics.Dynamics;
using FarseerPhysics.Factories;
using Microsoft.Xna.Framework;
using VertexArmy.Global;

namespace VertexArmy.Entities.Physics
{
	[DataContract]
	public class PhysicsEntityBasic : IPhysicsEntity
	{
		[DataMember]
		public PhysicsEntityType Type { get; private set; }

		[DataMember]
		public float Width { get; set; }
		[DataMember]
		public float Height { get; set; }

		[DataMember]
		public Vector2 Position { get; set; }

		private Body _body;
		public Body Body
		{
			get
			{
				if ( _body == null ) CreateBody();
				return _body;
			}
		}

		public PhysicsEntityBasic( PhysicsEntityType type, float width, float height )
		{
			Type = type;
			Width = width;
			Height = height;

			CreateBody();
		}

		private void CreateBody()
		{
			switch ( Type )
			{
				case PhysicsEntityType.Rectangle:
					_body = BodyFactory.CreateRectangle( Platform.Instance.PhysicsWorld, Width, Height, 1.0f, Position );
					break;
			}
		}

		public void SetPosition( Vector2 position )
		{
			Position = position;
		}

		public Vector2 GetPosition()
		{
			return Position;
		}

		public float GetRotation()
		{
			return Body.Rotation;
		}

		public void SetFreeze( bool value )
		{
			Body.Enabled = value;
		}

		public bool IsFrozen()
		{
			return !Body.Enabled;
		}

		public void PostDeserializeInit()
		{
			CreateBody();
		}
	}
}
