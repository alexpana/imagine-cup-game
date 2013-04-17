using System;
using System.Collections.Generic;
using VertexArmy.Graphics;

namespace VertexArmy.Global.Managers
{
	public class MaterialRepository
	{
		private readonly Dictionary<string, Func<IDictionary<string, object>, Material>> _materials;

		public void RegisterMaterial( string name, Func<IDictionary<string, object>, Material> materialFunc )
		{
			_materials.Add( name, materialFunc );
		}

		public void UnregisterMaterial( string name )
		{
			if ( _materials.ContainsKey( name ) )
			{
				_materials.Remove( name );
			}
		}

		public Func<IDictionary<string, object>, Material> GetMaterial( string name )
		{
			Func<IDictionary<string, object>, Material> materialFunc;
			_materials.TryGetValue( name, out materialFunc );

			return materialFunc;
		}

		public static MaterialRepository Instance
		{
			get { return MaterialRepositoryInstanceHolder.Instance; }
		}

		public MaterialRepository()
		{
			_materials = new Dictionary<string, Func<IDictionary<string, object>, Material>>();
		}

		private static class MaterialRepositoryInstanceHolder
		{
			// ReSharper disable MemberHidesStaticFromOuterClass
			public static readonly MaterialRepository Instance = new MaterialRepository();
			// ReSharper restore MemberHidesStaticFromOuterClass
		}
	}
}
