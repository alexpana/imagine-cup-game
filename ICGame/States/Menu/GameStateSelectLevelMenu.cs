using System;
using System.Collections.Generic;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Input;
using VertexArmy.Content.Prefabs;
using VertexArmy.Global;
using VertexArmy.Global.Managers;
using VertexArmy.Graphics;
using VertexArmy.Graphics.Attachables;

namespace VertexArmy.States.Menu
{
	public class GameStateSelectLevelMenu : BaseMenuGameState
	{
		private readonly List<MenuCube> _levelCubes;
		private int _selectedCubeIndex;
		private MenuCube _activeCube;
		Vector3 _lightPos = new Vector3( 0, 40000, 20000 );
		private SceneNode _lastNodeUnderPointer = null;

		private readonly float _cubeRotationDelta = MathHelper.ToRadians( 0.5f );

		public GameStateSelectLevelMenu( ContentManager contentManager )
			: base( contentManager )
		{
			_levelCubes = new List<MenuCube>();
		}

		public override void OnUpdate( GameTime gameTime )
		{
			base.OnUpdate( gameTime );

			_lightPos.Z = ( float ) ( 50000f + 20000.0 * Math.Sin( gameTime.TotalGameTime.TotalMilliseconds / 1000.0 ) );
			SceneManager.Instance.SetLightPosition( _lightPos );

			SceneNode nodeUnderPointer = null;

			if ( Platform.Instance.Input.PointerDelta.Length() > 0 )
			{
				List<SceneNode> nodes = SceneManager.Instance.IntersectScreenRayWithSceneNodes( Platform.Instance.Input.PointerPosition );
				if ( nodes.Count > 0 )
				{
					nodeUnderPointer = nodes[0];
					_lastNodeUnderPointer = nodeUnderPointer;
				}
				else
				{
					_lastNodeUnderPointer = null;
				}
			}

			for ( int i = 0; i < _levelCubes.Count; ++i )
			{

				var entity = GameWorldManager.Instance.GetEntity( _levelCubes[i].Id );
				if ( entity.SceneNodes["MenuCubeNode"] == nodeUnderPointer )
				{
					SelectCube( i );
				}

				entity.SetExternalRotation(
					Quaternion.Concatenate( entity.GetExternalRotation(),
						Quaternion.CreateFromAxisAngle( Vector3.UnitY, _cubeRotationDelta ) ) );
			}

			if ( Platform.Instance.Input.IsKeyPressed( Keys.Right, false ) || Platform.Instance.Input.ScrollDelta < 0 )
			{
				if ( _selectedCubeIndex < _levelCubes.Count - 1 )
				{
					_selectedCubeIndex++;
					SelectCube( _selectedCubeIndex );
				}
			}

			if ( Platform.Instance.Input.IsKeyPressed( Keys.Left, false ) || Platform.Instance.Input.ScrollDelta > 0 )
			{
				if ( _selectedCubeIndex > 0 )
				{
					_selectedCubeIndex--;
					SelectCube( _selectedCubeIndex );
				}
			}

			if ( Platform.Instance.Input.IsKeyPressed( Keys.Enter, false ) )
			{
				ActivateSelectedItem();
			}

			if ( Platform.Instance.Input.IsLeftPointerFirstTimePressed && _lastNodeUnderPointer != null )
			{
				ActivateSelectedItem();
			}

			if ( Platform.Instance.Input.IsKeyPressed( Keys.Back, false ) ||
				Platform.Instance.Input.IsKeyPressed( Keys.Escape, false ) )
			{
				StateManager.Instance.ChangeState( GameState.Menu );
			}
		}

		private void ActivateSelectedItem()
		{
			if ( _activeCube.Items != null &&
				 _activeCube.Items.Count > 0 )
			{
				_activeCube.Items[0].Activate();
			}
		}

		private void SelectCube( int index )
		{
			Vector3 selectedCubeScale = new Vector3( 2, 2, 2 );
			if ( _activeCube != null )
			{
				GameWorldManager.Instance.GetEntity( _activeCube.Id ).SetScale( Vector3.One );
			}

			_activeCube = _levelCubes[index];
			GameWorldManager.Instance.GetEntity( _activeCube.Id ).SetScale( selectedCubeScale );
			_selectedCubeIndex = index;
		}

		public override void OnEnter()
		{
			base.OnEnter();

			CreateLevelsCubes();

			GameWorldManager.Instance.SpawnEntity( CameraPrefab.PrefabName, "levelmenu_camera", new Vector3( 0, 10, 200 ) );

			SelectCube( 0 );
		}

		private void CreateLevelsCubes()
		{
			var tutorialCube = CreateLevelMenuCube( "level_cube_tutorial_text" );
			tutorialCube.Items = new List<MenuItem>
			{
				new MenuItem
				{
					Activated = obj => StateManager.Instance.ChangeState(GameState.TutorialLevel)
				}
			};
			tutorialCube.Spawn( -50f );

			var lockedLevel1Cube = CreateLevelMenuCube( "level_cube_locked_text" );
			lockedLevel1Cube.Items = new List<MenuItem>
			{
				new MenuItem
				{
					Activated = obj => StateManager.Instance.ChangeState( GameState.Demo2 )
				}
			};
			lockedLevel1Cube.Spawn( 0f );

			var lockedLevel2Cube = CreateLevelMenuCube( "level_cube_locked_text" );
			lockedLevel2Cube.Spawn( 50f );

			_levelCubes.Add( tutorialCube );
			_levelCubes.Add( lockedLevel1Cube );
			_levelCubes.Add( lockedLevel2Cube );

			_activeCube = tutorialCube;
		}

		private MenuCube CreateLevelMenuCube( string textImage )
		{
			MenuCube menuCube = new MenuCube( ContentManager );

			menuCube.SetTextImage( textImage );

			return menuCube;
		}

		public override void OnClose()
		{
			base.OnClose();

			foreach ( var levelCube in _levelCubes )
			{
				levelCube.Destroy();
			}
		}
	}
}
