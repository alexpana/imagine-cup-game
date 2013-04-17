using System;
using System.Collections.Generic;
using Microsoft.Xna.Framework.Graphics;
using VertexArmy.Global;
using VertexArmy.Global.Managers;
using VertexArmy.Graphics;
using VertexArmy.Graphics.Attachables;

namespace VertexArmy.GameWorld.Prefabs.Structs
{
	public class ArrayMeshSceneNodePrefab
	{
		public string Name { get; set; }
		public string Path;
		public string Mesh;
		public string Material;

		public int StartIndex, EndIndex;

		public Func<IDictionary<string, object>, Material> GetMaterialFunc()
		{
			return MaterialRepository.Instance.GetMaterial( Material );
		}

		public SceneNode GetSceneNode( IDictionary<string, object> parameters, GameEntity entity )
		{
			var materialFunc = GetMaterialFunc();
			SceneNode scn = new SceneNode();
			MeshAttachable mesh = new MeshAttachable( Platform.Instance.Content.Load<Model>( Mesh ), materialFunc( parameters ) );
			GameWorldManager.Instance.RegisterMesh( mesh, entity );
			scn.AddAttachable( mesh );

			return scn;
		}
	}
}
