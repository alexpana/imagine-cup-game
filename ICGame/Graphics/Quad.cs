using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using VertexArmy.Global;

namespace VertexArmy.Graphics
{
	public class Quad
	{
		public Vector3 Origin;
		public Vector3 UpperLeft;
		public Vector3 LowerLeft;
		public Vector3 UpperRight;
		public Vector3 LowerRight;
		public Vector3 Normal;
		public Vector3 Up;
		public Vector3 Left;

		public VertexPositionNormalTexture[] Vertices;
		public short [] Indices;

		private VertexBuffer _vertexBuffer;
		private IndexBuffer _indexBuffer;

		public Quad()
		{
			Init(Vector3.Zero, Vector3.Backward, Vector3.Up, 2f, 2f);
		}

		public void Init(Vector3 origin, Vector3 normal, Vector3 up, float width, float height)
		{
			Vertices = new VertexPositionNormalTexture[4];
			Indices = new short[6];
			Origin = origin;
			Normal = normal;
			Up = up;

			// Calculate the quad corners
			Left = Vector3.Cross(normal, Up);
			Vector3 uppercenter = (Up * height / 2) + origin;
			UpperLeft = uppercenter + (Left * width / 2);
			UpperRight = uppercenter - (Left * width / 2);
			LowerLeft = UpperLeft - (Up * height);
			LowerRight = UpperRight - (Up * height);

			FillVertices();

			CreateDeviceData();
		}

		private void FillVertices()
		{
			// Fill in texture coordinates to display full texture
			// on quad
			Vector2 textureUpperLeft = new Vector2(0.0f, 0.0f);
			Vector2 textureUpperRight = new Vector2(1.0f, 0.0f);
			Vector2 textureLowerLeft = new Vector2(0.0f, 1.0f);
			Vector2 textureLowerRight = new Vector2(1.0f, 1.0f);

			// Provide a normal for each vertex
			for (int i = 0; i < Vertices.Length; i++)
			{
				Vertices[i].Normal = Normal;
			}

			// Set the position and texture coordinate for each
			// vertex
			Vertices[0].Position = LowerLeft;
			Vertices[0].TextureCoordinate = textureLowerLeft;
			Vertices[1].Position = UpperLeft;
			Vertices[1].TextureCoordinate = textureUpperLeft;
			Vertices[2].Position = LowerRight;
			Vertices[2].TextureCoordinate = textureLowerRight;
			Vertices[3].Position = UpperRight;
			Vertices[3].TextureCoordinate = textureUpperRight;

			// Set the index buffer for each vertex, using
			// clockwise winding
			Indices[0] = 0;
			Indices[1] = 1;
			Indices[2] = 2;
			Indices[3] = 2;
			Indices[4] = 1;
			Indices[5] = 3;
		}

		public void CreateDeviceData()
		{
			_vertexBuffer = new VertexBuffer( Platform.Instance.Device, typeof( VertexPositionNormalTexture ), 4, BufferUsage.None );
			_vertexBuffer.SetData(Vertices);

			_indexBuffer = new IndexBuffer(Platform.Instance.Device, IndexElementSize.SixteenBits, 6, BufferUsage.None);
			_indexBuffer.SetData(Indices);
		}

		public void Draw(Material mat)
		{
			mat.Apply();
			Platform.Instance.Device.Indices = _indexBuffer;
			Platform.Instance.Device.SetVertexBuffer( _vertexBuffer );
			
			mat.Effect.CurrentTechnique.Passes[0].Apply();
			Platform.Instance.Device.DrawIndexedPrimitives( PrimitiveType.TriangleList, 0, 0, Vertices.Length, 0, Indices.Length / 3 );     
		}
	}
}
