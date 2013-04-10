using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using VertexArmy.Content.Materials;
using VertexArmy.Global;
using VertexArmy.Global.Managers;

namespace VertexArmy.Graphics.Attachables
{
	public class MeshAttachable : Attachable
	{
		private BoundingSphere _boundSphere;
		public Model Model { get; private set; }
		public Material Material { get; private set; }

		private Material _highlightMaterial;

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
			Highlighted = false;
			_highlightMaterial = MaterialRepository.Instance.GetMaterial("HighlightMaterial")(null);
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


			if(Highlighted)
			{
				Renderer.Instance.SetGlobalMaterialParameters( _highlightMaterial );
				_highlightMaterial.Apply();

				foreach ( ModelMesh m in Model.Meshes )
				{
					foreach ( ModelMeshPart part in m.MeshParts )
					{
						part.Effect = _highlightMaterial.Effect;
					}
				}

				BlendState _safBlend = new BlendState()
				{
					AlphaSourceBlend = Blend.SourceAlpha,
					AlphaDestinationBlend = Blend.InverseSourceAlpha,
					ColorSourceBlend = Blend.SourceAlpha,
					ColorDestinationBlend = Blend.InverseSourceAlpha,
					AlphaBlendFunction = BlendFunction.Add,
				};

				BlendState _defaultBlend = new BlendState();
				Platform.Instance.Device.BlendState = _safBlend;

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

				Platform.Instance.Device.BlendState = _defaultBlend;
				Highlighted = false;
			}

		}


		private BoundingBox _localSpaceAABB;
		private bool _isMeshAABBcomputed;


		public BoundingBox GetAABB()
		{
			if ( !_isMeshAABBcomputed )
			{
				ComputeAABB();
				_isMeshAABBcomputed = true;
			}
			return _localSpaceAABB;
		}

		public bool Highlighted { get; set; }

		public BoundingBox GetTransformedAABB()
		{
			
			Vector3 min = Vector3.Transform( GetAABB().Min, Parent.GetAbsoluteTransformation());
			Vector3 max = Vector3.Transform( GetAABB().Max, Parent.GetAbsoluteTransformation() );

			Vector3 trueMin = min;
			Vector3 trueMax = max;

			if ( min.X > max.X )
			{
				trueMin.X = max.X;
				trueMax.X = min.X;
			}

			if ( min.Y > max.Y )
			{
				trueMin.Y = max.Y;
				trueMax.Y = min.Y;
			}

			if ( min.Z > max.Z )
			{
				trueMin.Z = max.Z;
				trueMax.Z = min.Z;
			}
			return new BoundingBox(trueMin, trueMax);
		}

		private void ComputeAABB()
		{
			Vector3 min = new Vector3(float.MaxValue, float.MaxValue, float.MaxValue);
			Vector3 max = new Vector3(float.MinValue, float.MinValue, float.MinValue);


			foreach (ModelMesh mesh in Model.Meshes)
			{
				foreach (ModelMeshPart meshPart in mesh.MeshParts)
				{
					// Vertex buffer parameters
					int vertexStride = meshPart.VertexBuffer.VertexDeclaration.VertexStride;
					int vertexBufferSize = meshPart.NumVertices * vertexStride;

					float[] vertexData = new float[vertexBufferSize / sizeof( float )];
					meshPart.VertexBuffer.GetData( vertexData );

					// Iterate through vertexes (possibly) growing bounding box, all calculations are done in world space
					for ( int i = 0; i < vertexBufferSize / sizeof( float ); i += vertexStride / sizeof( float ) )
					{
						Vector3 position = new Vector3(vertexData[i], vertexData[i + 1], vertexData[i + 2]);

						min = Vector3.Min( min, position );
						max = Vector3.Max( max, position );
					}
				}
			}
			_localSpaceAABB = new BoundingBox(min, max);
		}
	}
}
