using Microsoft.Xna.Framework.Graphics;
using VertexArmy.Entities.Physics;
using VertexArmy.Global;
using VertexArmy.Graphics;
using VertexArmy.Physics;

namespace VertexArmy.Entities
{
	public class Robot
	{
		private PhysicsEntityRobot _robotPhysics;
		private SceneNode _node = new SceneNode();
		public Robot()
		{
			_robotPhysics = new PhysicsEntityRobot( 0.7f );
			Effect robofx = Global.Platform.Instance.Content.Load<Effect>("effects/" + "robo");


			for ( int i = 0; i < 29; ++i )
			{
				SceneNode scn = new SceneNode();
				Material robotmat = new Material( );
				robotmat.Effect = robofx;

				robotmat.AddParameter("ColorMap", Global.Platform.Instance.Content.Load<Texture2D>( "images/" + "color" ));
				robotmat.AddParameter( "NormalMap", Global.Platform.Instance.Content.Load<Texture2D>( "images/" + "normal" ) );
				robotmat.AddParameter( "SpecularMap", Global.Platform.Instance.Content.Load<Texture2D>( "images/" + "specular" ) );
				robotmat.AddParameter( "AOMap", Global.Platform.Instance.Content.Load<Texture2D>( "images/" + "ao" ) );

				scn.AddAttachable( new SimpleMeshEntity( Global.Platform.Instance.Content.Load<Model>( "models/" + "robo_link" ), robotmat ) );

				_node.AddChild( scn );

				Updateables.Instance.RegisterUpdatable( new BodyTransformableController( _robotPhysics.GetLinkBody( i ), scn ) );
			}

			for( int i = 0; i < 3; ++i )
			{
				SceneNode scn = new SceneNode( );
				Material robotmat = new Material( );
				robotmat.Effect = robofx;

				robotmat.AddParameter( "ColorMap", Global.Platform.Instance.Content.Load<Texture2D>( "images/" + "color" ) );
				robotmat.AddParameter( "NormalMap", Global.Platform.Instance.Content.Load<Texture2D>( "images/" + "normal" ) );
				robotmat.AddParameter( "SpecularMap", Global.Platform.Instance.Content.Load<Texture2D>( "images/" + "specular" ) );
				robotmat.AddParameter( "AOMap", Global.Platform.Instance.Content.Load<Texture2D>( "images/" + "ao" ) );

				scn.AddAttachable( new SimpleMeshEntity( Global.Platform.Instance.Content.Load<Model>( "models/" + "robo_wheel" ), robotmat ) );

				_node.AddChild(scn);

				Updateables.Instance.RegisterUpdatable( new BodyTransformableController( _robotPhysics.GetGearBody( i ), scn ) );
			}
			SceneManager.Instance.RegisterSceneTree(_node);

		}
	}
}
