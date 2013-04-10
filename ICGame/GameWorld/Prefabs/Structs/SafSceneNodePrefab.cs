using System;
using System.Collections.Generic;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using VertexArmy.Global;
using VertexArmy.Global.Managers;
using VertexArmy.Graphics;
using VertexArmy.Graphics.Attachables;

namespace VertexArmy.GameWorld.Prefabs.Structs
{
	public class SafSceneNodePrefab
	{
		public string Name { get; set; }
		public string Mesh;
		public string Material;
		public Vector3 LocalPosition;
		public Quaternion LocalRotation;

		public Func<IDictionary<string, object>, Material> GetMaterialFunc()
		{
			return MaterialRepository.Instance.GetMaterial( Material );
		}

		public SceneNode GetSceneNode( IDictionary<string, object> parameters, GameEntity entity )
		{
			var materialFunc = GetMaterialFunc();
			SceneNode scn = new SceneNode();
			scn.AddAttachable( new SafAttachable( Platform.Instance.Content.Load<Model>( Mesh ), materialFunc( parameters ) ) );
			scn.Invisible = true;
			scn.SetPosition( LocalPosition );
			scn.SetRotation( LocalRotation );

			return scn;
		}
	}
}