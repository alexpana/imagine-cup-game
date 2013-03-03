
using System.Collections.Generic;

namespace VertexArmy.Nodes
{
	class Node
	{
		private Node _parent;
		private List<Node> _children;

		public Node Parent
		{
			get { return _parent; }
			set
			{
				_parent = value;
			}
		}

		public int ChildrenCount
		{
			get { return _children.Count; }
		}

		public Node GetChild (int i)
		{
			return _children[i];
		}

		public void AddChild( Node child )
		{
			_children.Add( child );
		}

		public void RemoveChild( Node child )
		{
			_children.Remove( child );
		}
	}
}
