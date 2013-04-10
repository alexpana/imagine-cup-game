using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace VertexArmy.Graphics.Attachables
{
	public class MeshAttachable : Attachable
	{
		private BoundingSphere _boundSphere;
		public Model Model { get; private set; }
		public Material Material { get; private set; }
		public BoundingSphere BoundingSphere
		{
			get
			{
				return _boundSphere.Transform( Parent.GetAbsoluteTransformation() );
			}
			internal set { _boundSphere = value; }
		}

		public MeshAttachable( Model mod, Material mat )
		{
			_boundSphere = new BoundingSphere();
			foreach ( ModelMesh mesh in mod.Meshes )
			{
				_boundSphere = BoundingSphere.CreateMerged( _boundSphere, mesh.BoundingSphere );
				foreach ( ModelMeshPart part in mesh.MeshParts )
				{
					part.Effect = mat.Effect;
				}
			}

			Model = mod;
			Material = mat;
		}

		public override void RenderDepth( float dt )
		{
			Material Depth = Renderer.Instance.GetDepthBufferMaterial();
			Renderer.Instance.SetGlobalMaterialParameters( Depth );


			foreach ( ModelMesh m in Model.Meshes )
			{
				foreach ( ModelMeshPart part in m.MeshParts )
				{
					part.Effect = Depth.Effect;
				}
			}

			Depth.Apply();
			foreach ( ModelMesh m in Model.Meshes )
			{
				m.Draw();
			}

			foreach ( ModelMesh m in Model.Meshes )
			{
				foreach ( ModelMeshPart part in m.MeshParts )
				{
					part.Effect = Material.Effect;
				}
			}

		}

		public override void Render( float dt )
		{
			Renderer.Instance.SetGlobalMaterialParameters( Material );
			Material.Apply();
			foreach ( ModelMesh m in Model.Meshes )
			{
				m.Draw();
			}
		}
	}
}
