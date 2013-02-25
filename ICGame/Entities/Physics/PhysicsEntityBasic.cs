
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
		public Vector2 Position
		{
			get { return Body.Position; }
			set { Body.Position = value; }
		}

		[DataMember]
		public float Rotation
		{
			get { return Body.Rotation; }
			set { Body.Rotation = value; }
		}

		[DataMember]
		public bool Enabled
		{
			get { return Body.Enabled; }
			set { Body.Enabled = value; }
		}

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

		public void PreSerialize()
		{
		}

		public void PostDeserialize()
		{
			CreateBody();
		}
	}
}
