using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework.Graphics;
using VertexArmy.Graphics;

namespace VertexArmy.Entities
{
	public class Robot
	{
		private SceneNode _node = new SceneNode();
		public Robot()
		{
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
			}
			SceneManager.Instance.RegisterSceneTree(_node);
		}
	}
}
