﻿using Microsoft.Xna.Framework;
using VertexArmy.GameWorld.Prefabs;
using VertexArmy.GameWorld.Prefabs.Structs;
using VertexArmy.Global;

namespace VertexArmy.Content.Prefabs
{
	internal class CameraPrefab
	{
		public const string PrefabName = "Camera";

		public static PrefabEntity CreatePrefab()
		{
			PrefabEntity camera = new PrefabEntity();
			camera.RegisterCamera( "camera", new CameraSceneNodePrefab
			{
				Near = 1,
				Far = 10000,
				Fov = MathHelper.PiOver4,
				AspectRatio = Platform.Instance.Device.Viewport.AspectRatio,
				LookingDirection = new Vector3( 0, 0, -1 ),
				UpVector = new Vector3( 0, 1, 0 )
			} );
			return camera;
		}
	}
}
