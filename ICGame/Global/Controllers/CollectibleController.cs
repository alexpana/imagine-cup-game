using System.Collections.Generic;
using Microsoft.Xna.Framework;
using VertexArmy.Global.Behaviours;
using VertexArmy.Graphics;
using VertexArmy.Graphics.Attachables;

namespace VertexArmy.Global.Controllers
{
	class CollectibleController : IController
	{
		private float _rotationSpeed;
		public CollectibleController( SceneNode tree, float rotationSpeed = 0.0005f )
		{
			_rotationSpeed = rotationSpeed;
			Data = new List<object>
			       {
				       tree,
				       rotationSpeed
			       };
		}

		public void Update( GameTime dt )
		{
			if(Data == null || Data.Count == 0)
				return;

			SceneNode pnode = Data[0] as SceneNode;

			if(pnode != null)
				pnode.SetRotation(Quaternion.CreateFromAxisAngle(Vector3.UnitZ, (float)dt.TotalGameTime.TotalMilliseconds * _rotationSpeed));

			foreach (object obj in Data)
			{
				SceneNode node = obj as SceneNode;

				if(node != null)
				{
					MeshAttachable attachable = node.Attachable.Count > 0 ? node.Attachable[0] as MeshAttachable : null;

					//  if(attachable != null)
					//	attachable.Material.SetParameter("");
				}

			}
		}

		public List<object> Data { get; set; }
		public void Clean()
		{

		}
	}
}
