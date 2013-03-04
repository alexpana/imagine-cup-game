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
		private Model _robolink;
		private Model _robowheel;
		public readonly int PassCount = 2;


		private const int _LINK_NUMBER = 29;
		private const int _WHEEL_NUMBER = 3;

		private Matrix [] _linkmatrix =  new Matrix[_LINK_NUMBER];
		private Matrix [] _wheelmatrix =  new Matrix[_WHEEL_NUMBER];


		private float _ttime = 0;
		
		public void LoadNode(ContentManager manager)
		{
			//_cmanager = manager;
			_robolink = manager.Load<Model>( "models/" + "link" );
			_robowheel = manager.Load<Model>( "models/" + "wheel" );
			_effect = manager.Load<Effect>( "effects/" + "robo" );
			_color = manager.Load<Texture2D>( "images/" + "color" );
			_normal = manager.Load<Texture2D>( "images/" + "normal" );
			_specular = manager.Load<Texture2D>( "images/" + "specular" );
			_ao = manager.Load<Texture2D>( "images/" + "ao" );

			RemapModel( _robolink, _effect );
			RemapModel( _robowheel, _effect );


			for ( int i = 0; i < _LINK_NUMBER; ++i )
				_linkmatrix[i] = Matrix.Identity;

			for ( int i = 0; i < _WHEEL_NUMBER; ++i )
				_wheelmatrix[i] = Matrix.Identity;
		}

		public void SetWheelParameters(int index, Vector3 position, Quaternion rotation)
		{
			_wheelmatrix[index] = Matrix.Identity;
			_wheelmatrix[index] = Matrix.Transform( _wheelmatrix[index], rotation );
			_wheelmatrix[index] *= Matrix.CreateTranslation( position );
		}

		public void SetLinkParameteres(int index, Vector3 position, Quaternion rotation)
		{
			_linkmatrix[index] = Matrix.Identity;
			_linkmatrix[index] = Matrix.Transform( _linkmatrix[index], rotation );
			_linkmatrix[index] *= Matrix.CreateTranslation( position );
		}

		public void OnRender(float dt, SceneManager scn, int pass)
		{
			_effect.CurrentTechnique.Passes[pass].Apply( );
			//_effect.Parameters["eyePosition"].SetValue( scn.Eye );
			//_effect.Parameters["lightPosition"].SetValue( scn.Light );

			foreach ( Matrix matrix in _linkmatrix ) 
			{
				foreach ( ModelMesh m in _robolink.Meshes ) 
				{
					_effect.Parameters["matWorldViewProj"].SetValue( matrix * GlobalMatrix.Instance.MatWorldViewProjection );
					_effect.Parameters["matWorldInverseTranspose"].SetValue( Matrix.Invert( Matrix.Transpose( matrix ) ) * GlobalMatrix.Instance.MatWorldInverseTranspose );
					_effect.Parameters["matWorld"].SetValue( matrix * GlobalMatrix.Instance.MatWorld );
					_effect.Parameters["ColorMap"].SetValue( _color );
					_effect.Parameters["NormalMap"].SetValue( _normal );
					_effect.Parameters["SpecularMap"].SetValue( _specular );
					_effect.Parameters["AOMap"].SetValue( _ao );
					m.Draw( );
				}
			}


			foreach ( Matrix matrix in _wheelmatrix )
			{
				foreach (ModelMesh m in _robowheel.Meshes)
				{
					_effect.Parameters["matWorldViewProj"].SetValue( matrix * GlobalMatrix.Instance.MatWorldViewProjection );
					_effect.Parameters["matWorldInverseTranspose"].SetValue( Matrix.Invert( Matrix.Transpose( matrix ) ) * GlobalMatrix.Instance.MatWorldInverseTranspose );
					_effect.Parameters["matWorld"].SetValue( matrix * GlobalMatrix.Instance.MatWorld );
					_effect.Parameters["ColorMap"].SetValue(_color);
					_effect.Parameters["NormalMap"].SetValue(_normal);
					_effect.Parameters["SpecularMap"].SetValue(_specular);
					_effect.Parameters["AOMap"].SetValue(_ao);
					m.Draw();
				}
			}
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
			/*
			_ttime += dt;
			SetRotation(Quaternion.CreateFromAxisAngle(new Vector3(0,1,0), _ttime/1000 ));
			

			float sintt = (float)Math.Sin((double)_ttime / 5000);
			float sinttf = ( float ) Math.Sin( ( double ) _ttime / 1000 );

			sintt *= sintt;

			sintt += 0.5f;

			sintt /= 4;

			//SetScale( new Vector3( 0.5f, 0.5f, 0.5f ) );
			SetPosition( new Vector3( 0, 30*sinttf, 0 ) );
			 */
		}
	}
}
