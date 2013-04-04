using System.Collections.Generic;
using System.Runtime.Serialization;
using Microsoft.Xna.Framework;
using VertexArmy.Global.Behaviours;
using VertexArmy.Utilities;

namespace VertexArmy.Graphics
{
	[DataContract]
	public class SceneNode : ITransformable
	{
		[DataMember]
		private Matrix _absoluteTransformation;

		[DataMember]
		private Matrix _relativeTransformation;

		[DataMember]
		private Vector3 _position;

		[DataMember]
		private Quaternion _rotation;

		[DataMember]
		private Vector3 _scale;

		private SceneNode _parent;

		[DataMember]
		private List<SceneNode> _children;

		[DataMember]
		private List<Attachable> _attachables;

		private bool _recomputeAbsoluteTransformation;
		private bool _recomputeRelativeTransformation;

		public bool Invisible { get; set; }

		public List<SceneNode> Children
		{
			get { return _children; }
		}

		public List<Attachable> Attachable
		{
			get { return _attachables; }
		}

		public SceneNode()
		{
			Invisible = false;
			_scale = new Vector3( 1, 1, 1 );
			_position = new Vector3( 0, 0, 0 );
			_rotation = Quaternion.Identity;
			_recomputeAbsoluteTransformation =
				_recomputeRelativeTransformation = false;
			_parent = null;
			_children = new List<SceneNode>();
			_attachables = new List<Attachable>();
			_absoluteTransformation = Matrix.Identity;
			_relativeTransformation = Matrix.Identity;
		}


		public void AddAttachable( Attachable attach )
		{
			_attachables.Add( attach );
			attach.Parent = this;
		}



		private bool ShouldRecomputeTransformations()
		{
			return _recomputeRelativeTransformation ||
				   _recomputeAbsoluteTransformation;
		}


		public Matrix GetAbsoluteTransformation()
		{
			if ( ShouldRecomputeTransformations() )
			{
				_absoluteTransformation = Matrix.Identity;
				if ( _parent != null )
				{
					_absoluteTransformation = _parent.GetAbsoluteTransformation();
				}

				_absoluteTransformation = GetRelativeTransformation() * _absoluteTransformation;

				for ( int i = 0, l = _children.Count; i < l; ++i )
				{
					_children[i]._recomputeAbsoluteTransformation = true;
				}
				_recomputeAbsoluteTransformation = false;
			}
			return _absoluteTransformation;
		}

		public Matrix GetRelativeTransformation()
		{
			if ( _recomputeRelativeTransformation )
			{
				_relativeTransformation = Matrix.Identity;
				_relativeTransformation *= Matrix.CreateScale( _scale );
				_relativeTransformation = Matrix.Transform( _relativeTransformation, _rotation );
				_relativeTransformation *= Matrix.CreateTranslation( _position );

				_recomputeRelativeTransformation = false;
			}
			return _relativeTransformation;
		}


		public void SetPosition( Vector3 newPos )
		{
			_recomputeRelativeTransformation = true;
			_position = newPos;
		}

		public void SetRotation( Quaternion newRot )
		{
			_recomputeRelativeTransformation = true;
			_rotation = newRot;
		}

		public void SetScale( Vector3 newScale )
		{
			_recomputeRelativeTransformation = true;
			_scale = newScale;
		}

		public Vector3 GetPosition()
		{
			return _position;
		}

		public Quaternion GetRotation()
		{
			return _rotation;
		}

		public float GetRotationRadians()
		{
			return TransformUtility.GetAngleRollFromQuaternion( _rotation );
		}

		public Vector3 GetScale()
		{
			return _scale;
		}

		public void AddChild( SceneNode child )
		{
			if ( _children.Find( i => i.Equals( child ) ) == null )
			{
				_children.Add( child );
				child._parent = this;
				child._recomputeAbsoluteTransformation = true;
			}
		}

		public void RemoveChild( SceneNode child )
		{
			_children.RemoveAll( i => i.Equals( child ) );
			child._parent = null;
			child._recomputeAbsoluteTransformation = true;
		}

		public void RemoveAllChildren()
		{
			for ( int i = 0, l = _children.Count; i < l; ++i )
			{
				_children[i]._parent = null;
				_children[i]._recomputeAbsoluteTransformation = true;
			}
			_children.Clear();
		}

		public SceneNode GetChild( int index )
		{
			return _children[index];
		}
	}
}