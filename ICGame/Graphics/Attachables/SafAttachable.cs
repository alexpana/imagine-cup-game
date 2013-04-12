using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using VertexArmy.Global;

namespace VertexArmy.Graphics.Attachables
{
	class SafAttachable : Attachable
	{
		public Model Model { get; private set; }
		public Material Material { get; private set; }
		public BoundingSphere BoundingSphere { get; internal set; }

		private BlendState _safBlend, _defaultBlend;

		public SafAttachable( Model mod, Material mat )
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

			_safBlend = new BlendState()
			{
				AlphaSourceBlend = Blend.SourceAlpha,
				AlphaDestinationBlend = Blend.InverseSourceAlpha,
				ColorSourceBlend = Blend.SourceAlpha,
				ColorDestinationBlend = Blend.InverseSourceAlpha,
				AlphaBlendFunction = BlendFunction.Add,
			};

			_defaultBlend = new BlendState();
		}

		public override int GetLayer()
		{
			return 2;
		}
		
		public override void Render( float dt )
		{
			Renderer.Instance.SetGlobalMaterialParameters( Material );
			Material.Apply();

			Platform.Instance.Device.BlendState = _safBlend;

			foreach ( ModelMesh m in Model.Meshes )
			{
				m.Draw();
			}

			Platform.Instance.Device.BlendState = _defaultBlend;
		}
	}
}
