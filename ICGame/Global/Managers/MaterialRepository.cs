using System.Collections.Generic;
using VertexArmy.Graphics;

namespace VertexArmy.Global.Managers
{
	public class MaterialRepository
	{
		private Dictionary<string, Material> _materials;

		public void RegisterMaterial( string name, Material mat )
		{
			_materials.Add( name, mat );
		}

		public void UnregisterMaterial( string name )
		{
			if ( _materials.ContainsKey( name ) )
			{
				_materials.Remove( name );
			}
		}

		public Material GetMaterial( string name )
		{
			if ( _materials.ContainsKey( name ) )
			{
				return _materials[name];
			}
			return null;
		}

		public static MaterialRepository Instance
		{
			get { return MaterialRepositoryInstanceHolder.Instance; }
		}

		public MaterialRepository()
		{
			_materials = new Dictionary<string, Material>( );
		}

		private static class MaterialRepositoryInstanceHolder
		{
			// ReSharper disable MemberHidesStaticFromOuterClass
			public static readonly MaterialRepository Instance = new MaterialRepository( );
			// ReSharper restore MemberHidesStaticFromOuterClass
		}
	}
}