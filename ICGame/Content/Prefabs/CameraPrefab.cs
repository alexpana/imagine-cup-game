using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using VertexArmy.GameWorld.Prefabs;
using VertexArmy.GameWorld.Prefabs.Structs;

namespace VertexArmy.Content.Prefabs
{
	class CameraPrefab
	{
		public static PrefabEntity CreatePrefab( )
		{
			PrefabEntity camera = new PrefabEntity();
			camera.RegisterCamera("camera", new CameraSceneNodePrefab
			                                {
				                                Near = 1,
				                                Far = 10000,
				                                Fov = MathHelper.PiOver4,
				                                AspectRatio = Global.Platform.Instance.Device.Viewport.AspectRatio,
				                                LookingDirection = new Vector3(0, 0, 1),
				                                UpVector = new Vector3(0, 1, 0)
			                                });
			return camera;
		}
	}
}
