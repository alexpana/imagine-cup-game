using System;
using System.Collections.Generic;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Input;
using VertexArmy.GameWorld;
using VertexArmy.Global.Behaviours;
using VertexArmy.Global.Managers;
using VertexArmy.Graphics;
using VertexArmy.Graphics.Attachables;

namespace VertexArmy.Global.Controllers
{
	public class EditorToolsController : IController
	{
		private EditorState _state;
		public EditorState State { get { return _state; } }
		private double _time;

		private int _clicks;
		private const int _requiredClicks = 2;
		private const double _clickInterval = 500.0;
		private bool _leftClick;

		private GameEntity _selectedEntity;
		private GameEntity _tryEntity;

		public EditorToolsController()
		{
			_leftClick = false;
			_state = EditorState.None;
			_clicks = 0;
		}

		public void Update( GameTime dt )
		{
			if ( Mouse.GetState().LeftButton.Equals( ButtonState.Pressed ) && !_leftClick )
			{
				_leftClick = true;
				_clicks++;
			}
			else if ( Mouse.GetState().LeftButton.Equals( ButtonState.Released ) && _leftClick )
			{
				_leftClick = false;
				_time = dt.TotalGameTime.TotalMilliseconds;
			}
			else if ( Mouse.GetState().LeftButton.Equals( ButtonState.Released ) && !_leftClick )
			{
				if ( dt.TotalGameTime.TotalMilliseconds - _time > _clickInterval )
				{
					_clicks = 0;
				}
			}

			if ( _clicks == _requiredClicks )
			{
				_clicks = 0;
				GameEntity selected = TrySelectEntity();

				if ( selected != null )
				{
					HintManager.Instance.SpawnHint( selected.Name, new Vector2( 100, 100 ), 1000, 2 );
				}
			}
		}

		private GameEntity TrySelectEntity()
		{
			MouseState mouseState = Mouse.GetState();
			int mouseX = mouseState.X;
			int mouseY = mouseState.Y;

			List<SceneNode> lst = SceneManager.Instance.IntersectRayWithSceneNodes(mouseX, mouseY);


			lst.Sort( ( x, y ) => (int)(y.GetAbsolutePosition().Z - x.GetAbsolutePosition().Z) );


			if ( lst.Count > 0 )
				return GameWorldManager.Instance.GetEntityByMesh((MeshAttachable)lst[0].Attachable[0]);

			return null;
		}

		public List<object> Data { get; set; }

		public void Clean()
		{

		}
	}

	public enum EditorState
	{
		None,
		Selected
	}
}
