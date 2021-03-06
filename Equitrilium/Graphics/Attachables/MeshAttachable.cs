﻿using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using VertexArmy.Global;
using VertexArmy.Global.Managers;

namespace VertexArmy.Graphics.Attachables
{
	public class MeshAttachable : Attachable
	{
		private BoundingSphere _boundSphere;
		public Model Model { get; private set; }
		public Material Material { get; private set; }

		private readonly Material _highlightMaterial;
		public Vector3 HighColor;

		public BoundingSphere BoundingSphere
		{
			get { return _boundSphere.Transform( Parent.GetAbsoluteTransformation() ); }
			internal set { _boundSphere = value; }
		}

		public MeshAttachable( string name, Model mod, Material mat )
		{
			_boundSphere = new BoundingSphere();
			Highlighted = false;
			HighColor = Vector3.One;
			_highlightMaterial = MaterialRepository.Instance.GetMaterial( "HighlightMaterial" )( null );
			foreach ( ModelMesh mesh in mod.Meshes )
			{
				_boundSphere = BoundingSphere.CreateMerged( _boundSphere, mesh.BoundingSphere );

				foreach ( ModelMeshPart part in mesh.MeshParts )
				{
					part.Effect = mat.Effect;
				}
			}

			Model = mod;
			Model.Tag = name;

			Material = mat;
		}

		public override void RenderDepth( float dt )
		{
			Material depth = Renderer.Instance.GetDepthBufferMaterial();
			Renderer.Instance.SetGlobalMaterialParameters( depth );
			depth.Apply();

			foreach ( ModelMesh m in Model.Meshes )
			{
				foreach ( ModelMeshPart part in m.MeshParts )
				{
					part.Effect = depth.Effect;
				}
			}

			
			foreach ( ModelMesh m in Model.Meshes )
			{
				m.Draw();
			}

			foreach ( ModelMesh m in Model.Meshes )
			{
				foreach ( ModelMeshPart part in m.MeshParts )
				{
					part.Effect = null;
				}
			}
		}

		public override void PostRender( float dt )
		{
			if ( Highlighted )
			{
				_highlightMaterial.SetParameter( "f3Color", HighColor );
				Renderer.Instance.SetGlobalMaterialParameters( _highlightMaterial );
				_highlightMaterial.Apply();

				foreach ( ModelMesh m in Model.Meshes )
				{
					foreach ( ModelMeshPart part in m.MeshParts )
					{
						part.Effect = _highlightMaterial.Effect;
					}
				}


				BlendState _safBlend = new BlendState
				{
					AlphaSourceBlend = Blend.SourceAlpha,
					AlphaDestinationBlend = Blend.InverseSourceAlpha,
					ColorSourceBlend = Blend.SourceAlpha,
					ColorDestinationBlend = Blend.InverseSourceAlpha,
					AlphaBlendFunction = BlendFunction.Add,
				};

				BlendState defaultBlend = new BlendState();
				Platform.Instance.Device.BlendState = _safBlend;

				foreach ( ModelMesh m in Model.Meshes )
				{
					m.Draw();
				}

				foreach ( ModelMesh m in Model.Meshes )
				{
					foreach ( ModelMeshPart part in m.MeshParts )
					{
						part.Effect = null;
					}
				}

				Platform.Instance.Device.BlendState = defaultBlend;
				Highlighted = false;
			}
		}

		public override void Render( float dt )
		{
			Renderer.Instance.SetGlobalMaterialParameters( Material );
			Material.Apply();
			Platform.Instance.Device.BlendState = Material.State;

			foreach ( ModelMesh m in Model.Meshes )
			{
				foreach ( ModelMeshPart part in m.MeshParts )
				{
					part.Effect = Material.Effect;
				}
			}

			foreach ( ModelMesh m in Model.Meshes )
			{
				m.Draw();
			}

			foreach ( ModelMesh m in Model.Meshes )
			{
				foreach ( ModelMeshPart part in m.MeshParts )
				{
					part.Effect = null;
				}
			}
		}

		public override int GetLayer()
		{
			return Material.Layer;
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
			Vector3 min = Vector3.Transform( GetAABB().Min, Parent.GetAbsoluteTransformation() );
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
			return new BoundingBox( trueMin, trueMax );
		}

		private void ComputeAABB()
		{
			Vector3 min = new Vector3( float.MaxValue, float.MaxValue, float.MaxValue );
			Vector3 max = new Vector3( float.MinValue, float.MinValue, float.MinValue );


			foreach ( ModelMesh mesh in Model.Meshes )
			{
				foreach ( ModelMeshPart meshPart in mesh.MeshParts )
				{
					// Vertex buffer parameters
					int vertexStride = meshPart.VertexBuffer.VertexDeclaration.VertexStride;
					int vertexBufferSize = meshPart.NumVertices * vertexStride;

					float[] vertexData = new float[vertexBufferSize / sizeof( float )];
					meshPart.VertexBuffer.GetData( vertexData );

					// Iterate through vertexes (possibly) growing bounding box, all calculations are done in world space
					for ( int i = 0; i < vertexBufferSize / sizeof( float ); i += vertexStride / sizeof( float ) )
					{
						Vector3 position = new Vector3( vertexData[i], vertexData[i + 1], vertexData[i + 2] );

						min = Vector3.Min( min, position );
						max = Vector3.Max( max, position );
					}
				}
			}
			_localSpaceAABB = new BoundingBox( min, max );
		}
	}
}
