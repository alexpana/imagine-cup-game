using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;



namespace VertexArmy.Graphics
{
	class RobotSceneNode : SceneNode
	{
		//private ContentManager _cmanager = null;
		private Effect _effect;
		private Model _robom;
		public readonly int PassCount = 2;
		
		public void LoadNode(ContentManager manager, string path)
		{
			//_cmanager = manager;
			_robom = manager.Load<Model>( "models" + path );
			_effect = manager.Load<Effect>( "effects" + "robo.fx" );
		}

		public void OnRender(float dt, int pass)
		{
			_effect.Parameters["matWorldViewProj"].SetValue(GlobalMatrix.Instance.MatWorldViewProjection);
			_effect.Parameters["matWorldViewInverseTranspose"].SetValue(GlobalMatrix.Instance.MatWorldInverseTranspose);
			_effect.Parameters["matWorld"].SetValue(GlobalMatrix.Instance.MatWorld);


			_effect.CurrentTechnique.Passes[pass].Apply( );

			foreach (ModelMesh m in _robom.Meshes) {
				m.Draw();
			}

		}

		public void OnUpdate(float dt)
		{
			

		}

	}
}
