using FarseerPhysics.Collision.Shapes;
using FarseerPhysics.Dynamics;
using FarseerPhysics.Controllers;
using FarseerPhysics.Factories;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Audio;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using VertexArmy.Global;

namespace VertexArmy.States
{
	internal class GameStatePhysics : IGameState
	{

		private ContentManager _contentManager;


		private World _physicsWorld;

		private Body _testBody;
		private Body _floor;
		private CircleShape _circleShape;
		private Fixture _circleFixture;

		public GameStatePhysics( ContentManager content )
		{
			_contentManager = content;
		}

		public void OnUpdate( GameTime dt )
		{
			
		}

		public void RenderScene()
		{
		
		}

		public void OnRender( GameTime dt )
		{
			Platform.Instance.Device.Clear( Color.SkyBlue );
			RenderScene( );
		}

		public void LoadContent()
		{
			_physicsWorld = new World(new Vector2(0f, 9.82f));

			_testBody = BodyFactory.CreateBody(_physicsWorld);
			_circleShape = new CircleShape(0.5f, 3f);
			_circleFixture = _testBody.CreateFixture(_circleShape);

			_floor = BodyFactory.CreateRectangle(
				_physicsWorld,
				480f,
				50f,
				10f);
			_floor.Position = new Vector2(240, 775);
			_floor.IsStatic = true;
			_floor.Restitution = 0.2f;
			_floor.Friction = 0.2f;
			

		}

		public void OnEnter()
		{
			
		}

		public void OnClose()
		{
			_contentManager.Unload( );
		}
	}
}
