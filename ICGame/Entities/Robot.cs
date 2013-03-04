using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using VertexArmy.Entities.Physics;
using VertexArmy.Global;
using VertexArmy.Graphics;
using VertexArmy.Physics;

namespace VertexArmy.Entities
{
	public class Robot
	{
		public PhysicsEntityRobot RobotPhysics = new PhysicsEntityRobot( 0.7f );
		private readonly SceneNode _node = new SceneNode();
		public Robot()
		{
			Effect robofx = Platform.Instance.Content.Load<Effect>("effects/" + "robo");


			for ( int i = 0; i < 28; ++i )
			{
				SceneNode scn = new SceneNode();
				Material robotmat = new Material( );
				robotmat.Effect = robofx;

				robotmat.AddParameter("ColorMap", Platform.Instance.Content.Load<Texture2D>( "images/" + "color" ));
				robotmat.AddParameter( "NormalMap", Platform.Instance.Content.Load<Texture2D>( "images/" + "normal" ) );
				robotmat.AddParameter( "SpecularMap", Platform.Instance.Content.Load<Texture2D>( "images/" + "specular" ) );
				robotmat.AddParameter( "AOMap", Platform.Instance.Content.Load<Texture2D>( "images/" + "ao" ) );

				robotmat.AddParameter( "matWorldViewProj", Matrix.Identity );
				robotmat.AddParameter( "matWorldInverseTranspose", Matrix.Identity );
				robotmat.AddParameter( "matWorld", Matrix.Identity );
				robotmat.AddParameter( "eyePosition", Vector3.Zero );
				robotmat.AddParameter( "lightPosition", Vector3.Zero );

				scn.AddAttachable( new SimpleMeshEntity( Platform.Instance.Content.Load<Model>( "models/" + "robo_link" ), robotmat ) );

				_node.AddChild( scn );

				Updateables.Instance.RegisterUpdatable( new BodyTransformableController( RobotPhysics.GetLinkBody( i ), scn ) );
			}

			for( int i = 0; i < 3; ++i )
			{
				SceneNode scn = new SceneNode( );
				Material robotmat = new Material( );
				robotmat.Effect = robofx;

				robotmat.AddParameter( "ColorMap", Platform.Instance.Content.Load<Texture2D>( "images/" + "color" ) );
				robotmat.AddParameter( "NormalMap", Platform.Instance.Content.Load<Texture2D>( "images/" + "normal" ) );
				robotmat.AddParameter( "SpecularMap", Platform.Instance.Content.Load<Texture2D>( "images/" + "specular" ) );
				robotmat.AddParameter( "AOMap", Platform.Instance.Content.Load<Texture2D>( "images/" + "ao" ) );

				robotmat.AddParameter( "matWorldViewProj", Matrix.Identity );
				robotmat.AddParameter( "matWorldInverseTranspose", Matrix.Identity );
				robotmat.AddParameter( "matWorld", Matrix.Identity );
				robotmat.AddParameter( "eyePosition", Vector3.Zero );
				robotmat.AddParameter( "lightPosition", Vector3.Zero );

				scn.AddAttachable( new SimpleMeshEntity( Platform.Instance.Content.Load<Model>( "models/" + "robo_wheel" ), robotmat ) );

				_node.AddChild(scn);

				Updateables.Instance.RegisterUpdatable( new BodyTransformableController( RobotPhysics.GetGearBody( i ), scn ) );
			}
			SceneManager.Instance.RegisterSceneTree(_node);

		}
	}
}
