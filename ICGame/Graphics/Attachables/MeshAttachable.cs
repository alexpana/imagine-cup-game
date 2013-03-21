using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework.Graphics;

namespace VertexArmy.Graphics
{
	class MeshAttachable : Attachable
	{
		private Model _myMod;
		private Material _myMat;

		public MeshAttachable(Model mod, Material mat)
		{
			foreach ( ModelMesh mesh in mod.Meshes ) {
				foreach ( ModelMeshPart part in mesh.MeshParts ) {
					part.Effect = mat.Effect;
				}
			}

			_myMod = mod;
			_myMat = mat;
		}

		public override void Render( float dt )
		{
			Renderer.Instance.SetGlobalMaterialParameters(_myMat);
			_myMat.Apply();
			foreach (ModelMesh m in _myMod.Meshes)
			{
				m.Draw();
			}
		}
	}
}
