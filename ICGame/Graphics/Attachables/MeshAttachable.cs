using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace VertexArmy.Graphics
{
	public class MeshAttachable : Attachable
	{
		public Model Model { get; private set; }
		public Material Material { get; private set; }
		public BoundingSphere BoundingSphere { get; internal set; }

		public MeshAttachable( Model mod, Material mat )
		{
			BoundingSphere = new BoundingSphere();
			foreach ( ModelMesh mesh in mod.Meshes )
			{
				BoundingSphere = BoundingSphere.CreateMerged( BoundingSphere, mesh.BoundingSphere );
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

			Depth.Apply();
			foreach ( ModelMesh m in Model.Meshes )
			{
				foreach ( ModelMeshPart part in m.MeshParts )
				{
					part.Effect = Depth.Effect;
				}

				m.Draw();

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
