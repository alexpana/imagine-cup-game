using System;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;



namespace VertexArmy.Graphics
{
	class RobotSceneNode : SceneNode
	{
		//private ContentManager _cmanager = null;
		private Effect _effect;
		private Texture2D _color;
		private Texture2D _normal;
		private Texture2D _specular;
		private Texture2D _ao;
		private Model _robom;
		public readonly int PassCount = 2;

		private float _ttime = 0;
		
		public void LoadNode(ContentManager manager, string path)
		{
			//_cmanager = manager;
			_robom = manager.Load<Model>( "models/" + path );
			_effect = manager.Load<Effect>( "effects/" + "robo" );
			_color = manager.Load<Texture2D>( "images/" + "color" );
			_normal = manager.Load<Texture2D>( "images/" + "normal" );
			_specular = manager.Load<Texture2D>( "images/" + "specular" );
			_ao = manager.Load<Texture2D>( "images/" + "ao" );

			RemapModel(_robom, _effect);
		}

		public void OnRender(float dt, Scene scn, int pass)
		{
			_effect.CurrentTechnique.Passes[pass].Apply( );
			_effect.Parameters["eyePosition"].SetValue( scn.Eye );
			_effect.Parameters["lightPosition"].SetValue( scn.Light );
			foreach (ModelMesh m in _robom.Meshes) {
				_effect.Parameters["matWorldViewProj"].SetValue( m.ParentBone.Transform * GlobalMatrix.Instance.MatWorldViewProjection );
				_effect.Parameters["matWorldViewInverseTranspose"].SetValue( Matrix.Invert(Matrix.Transpose(m.ParentBone.Transform)) * GlobalMatrix.Instance.MatWorldInverseTranspose );
				_effect.Parameters["matWorld"].SetValue( m.ParentBone.Transform * GlobalMatrix.Instance.MatWorld );
				_effect.Parameters["ColorMap"].SetValue(_color);
				_effect.Parameters["NormalMap"].SetValue(_normal);
				_effect.Parameters["SpecularMap"].SetValue( _specular );
				_effect.Parameters["AOMap"].SetValue( _ao );
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
			_ttime += dt;
			SetRotation(Quaternion.CreateFromAxisAngle(new Vector3(0,1,0), _ttime/1000 ));

			float sintt = (float)Math.Sin((double)_ttime / 5000);
			float sinttf = ( float ) Math.Sin( ( double ) _ttime / 1000 );

			sintt *= sintt + 0.5f;

			//SetScale(new Vector3(sintt, sintt, sintt));
			//SetPosition( new Vector3( 0, 30*sinttf, 0 ) );
		}
	}
}
