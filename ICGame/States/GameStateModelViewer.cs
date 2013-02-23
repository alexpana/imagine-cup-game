using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using VertexArmy.Graphics;

namespace VertexArmy.States
{
	class GameStateModelViewer : IGameState
	{
		private ContentManager _cm;
		private RobotSceneNode _node;
		private Scene _scene;
		public GameStateModelViewer( ContentManager content )
		{
			_cm = content;
			
		}

		public void OnUpdate( GameTime dt )
		{
			_node.OnUpdate( dt.ElapsedGameTime.Milliseconds );
		}

		public void OnRender( GameTime dt )
		{
			GlobalMatrix.Instance.LoadMatrix( EMatrix.World, _node.GetAbsoluteTransformation( ) );
			
			_node.OnRender(dt.ElapsedGameTime.Milliseconds, 0);
		}

		public void OnEnter()
		{
			//Global.Platform.Instance.Device.RasterizerState = new RasterizerState { CullMode = CullMode.None, FillMode = FillMode.WireFrame};
		
			GlobalMatrix.Instance.LoadMatrix( EMatrix.Projection, Matrix.CreatePerspectiveFieldOfView( MathHelper.PiOver4, Global.Platform.Instance.Device.Viewport.AspectRatio, 1, 10000 ) );
			GlobalMatrix.Instance.LoadMatrix( EMatrix.View, Matrix.CreateLookAt( new Vector3( 0, 0, -500 ), new Vector3( 0, 0, 0 ), new Vector3( 0, 1, 0 ) ) );


			_scene = new Scene { Eye = new Vector3(0, 0, 0), Light = new Vector3(0, 100, 100) };
			_node = new RobotSceneNode( );
			_node.LoadNode( _cm, "tracks" );
			//_node.SetScale(new Vector3(0.05f, 0.05f, 0.05f));
		}

		public void OnClose()
		{
			
		}
	}
}
