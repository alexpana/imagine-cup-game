using Microsoft.Xna.Framework;
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

		private float ttime = 0;
		
		public void LoadNode(ContentManager manager, string path)
		{
			//_cmanager = manager;
			_robom = manager.Load<Model>( "models/" + path );
			_effect = manager.Load<Effect>( "effects/" + "robo" );

			RemapModel(_robom, _effect);
		}

		public void OnRender(float dt, int pass)
		{
			_effect.CurrentTechnique.Passes[pass].Apply( );
			foreach (ModelMesh m in _robom.Meshes) {

				_effect.Parameters["matWorldViewProj"].SetValue( m.ParentBone.Transform * GlobalMatrix.Instance.MatWorldViewProjection );
				_effect.Parameters["matWorldViewInverseTranspose"].SetValue( m.ParentBone.Transform * GlobalMatrix.Instance.MatWorldInverseTranspose );
				_effect.Parameters["matWorld"].SetValue( m.ParentBone.Transform * GlobalMatrix.Instance.MatWorld );

				m.Draw();
			}
		}

		public void DrawRobo()
		{
			/*
			Matrix[] modelTransforms = new Matrix[_robom.Bones.Count];
			_robom.CopyAbsoluteBoneTransformsTo( modelTransforms );
			int i = 0;
			foreach ( ModelMesh mesh in _robom.Meshes ) {
				foreach ( Effect currentEffect in mesh.Effects ) {
					Matrix worldMatrix = modelTransforms[mesh.ParentBone.Index] * GlobalMatrix.Instance.MatWorld;
					currentEffect.CurrentTechnique = currentEffect.Techniques[0];
					currentEffect.Parameters["matWorldViewProj"].SetValue( worldMatrix * viewMatrix * projectionMatrix );
					currentEffect.Parameters["xTexture"].SetValue( textures[i++] );
				}
				mesh.Draw( );
			}
			 */
		}

		public void RemapModel( Model model, Effect effect )
		{
			foreach ( ModelMesh mesh in model.Meshes ) {
				foreach ( ModelMeshPart part in mesh.MeshParts ) {
					part.Effect = effect;
				}
			}
		}

		public void OnUpdate(float dt)
		{
			ttime += dt;
			SetRotation(Quaternion.CreateFromAxisAngle(new Vector3(0,1,0), ttime/1000 ));
		}
	}
}
