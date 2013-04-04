using System.Collections.Generic;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Input;
using VertexArmy.Content.Prefabs;
using VertexArmy.Global;
using VertexArmy.Global.Managers;

namespace VertexArmy.States.Menu
{
	public class GameStateSelectLevelMenu : BaseMenuGameState
	{
		private readonly List<MenuCube> _levelCubes;
		private int _selectedCubeIndex;
		private MenuCube _activeCube;

		private readonly float _cubeRotationDelta = MathHelper.ToRadians( 0.5f );

		public GameStateSelectLevelMenu( ContentManager contentManager )
			: base( contentManager )
		{
			_levelCubes = new List<MenuCube>();
		}

		public override void OnUpdate( GameTime gameTime )
		{
			base.OnUpdate( gameTime );

			foreach ( var levelCube in _levelCubes )
			{
				var entity = GameWorldManager.Instance.GetEntity( levelCube.Id );

				if ( levelCube == _activeCube )
				{
					entity.SetScale( new Vector3( 2, 2, 2 ) );
				}

				entity.SetExternalRotation(
					Quaternion.Concatenate( entity.GetExternalRotation(),
						Quaternion.CreateFromAxisAngle( Vector3.UnitY, _cubeRotationDelta ) ) );
			}

			if ( Platform.Instance.Input.IsKeyPressed( Keys.Right, false ) )
			{
				if ( _selectedCubeIndex < _levelCubes.Count - 1 )
				{
					_selectedCubeIndex++;
					SelectCube( _selectedCubeIndex );
				}
			}

			if ( Platform.Instance.Input.IsKeyPressed( Keys.Left, false ) )
			{
				if ( _selectedCubeIndex > 0 )
				{
					_selectedCubeIndex--;
					SelectCube( _selectedCubeIndex );
				}
			}

			if ( Platform.Instance.Input.IsKeyPressed( Keys.Enter, false ) )
			{
				if ( _activeCube.Items != null &&
					 _activeCube.Items.Count > 0 )
				{
					_activeCube.Items[0].Activate();
				}
			}

			if ( Platform.Instance.Input.IsKeyPressed( Keys.Back, false ) ||
				Platform.Instance.Input.IsKeyPressed( Keys.Escape, false ) )
			{
				StateManager.Instance.ChangeState( GameState.Menu );
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
		}

		public override void OnEnter()
		{
			base.OnEnter();

			CreateLevelsCubes();

			GameWorldManager.Instance.SpawnEntity( CameraPrefab.PrefabName, "levelmenu_camera", new Vector3( 0, 0, 200 ) );
		}

		private void CreateLevelsCubes()
		{
			var tutorialCube = CreateLevelMenuCube( "level_tutorial" );
			tutorialCube.Items = new List<MenuItem>
			{
				new MenuItem
				{
					Activated = obj => StateManager.Instance.ChangeState(GameState.TutorialLevel)
				}
			};
			tutorialCube.Spawn( -50f );

			var lockedLevel1Cube = CreateLevelMenuCube( "level_blocked" );
			lockedLevel1Cube.Spawn( 0f );

			var lockedLevel2Cube = CreateLevelMenuCube( "level_blocked" );
			lockedLevel2Cube.Spawn( 50f );

			_levelCubes.Add( tutorialCube );
			_levelCubes.Add( lockedLevel1Cube );
			_levelCubes.Add( lockedLevel2Cube );

			_activeCube = tutorialCube;
		}

		private MenuCube CreateLevelMenuCube( string backgroundImage )
		{
			MenuCube menuCube = new MenuCube( ContentManager );

			menuCube.SetBackgroundImage( backgroundImage );

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
