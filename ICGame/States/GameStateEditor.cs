﻿using System;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.GamerServices;
using Microsoft.Xna.Framework.Input;
using VertexArmy.GameWorld;
using VertexArmy.Global;
using VertexArmy.Global.Controllers;
using VertexArmy.Global.Managers;
using VertexArmy.Physics.DebugView;
using VertexArmy.Utilities;

namespace VertexArmy.States
{
	internal class GameStateEditor : PlayableGameState
	{
		private ContentManager _contentManager;

		private DebugViewXNA _debugView;
		private Matrix _projection;
		private Matrix _view;

		public GameEntity Robot;
		public GameEntity Camera;

		private bool _actionToggleDebugView;
		private bool _debugViewState;

		private bool _levelLoaded;

		public GameStateEditor( ContentManager content )
		{
			_contentManager = content;
		}

		public override void OnUpdate( GameTime gameTime )
		{
			if ( !_levelLoaded )
			{
				return;
			}

			base.OnUpdate( gameTime );

			_pausePhysics = true;

			if ( Keyboard.GetState( PlayerIndex.One ).IsKeyDown( Keys.D ) )
			{
				if ( !_actionToggleDebugView )
				{
					_debugViewState = !_debugViewState;
					_actionToggleDebugView = true;
				}
			}

			if ( Keyboard.GetState( PlayerIndex.One ).IsKeyUp( Keys.D ) )
			{
				_actionToggleDebugView = false;
			}
		}

		public override void OnRender( GameTime gameTime )
		{
			if ( !_levelLoaded )
			{
				return;
			}

			base.OnRender( gameTime );

			if ( _debugViewState )
			{
				float scale = ( SceneManager.Instance.GetCurrentCamera().Parent.GetPosition().Z / 800f );
				_projection = Matrix.CreateOrthographicOffCenter(
					UnitsConverter.ToSimUnits( SceneManager.Instance.GetCurrentCamera().Parent.GetPosition().X - Platform.Instance.Device.Viewport.Width / 2f * scale ),
					UnitsConverter.ToSimUnits( SceneManager.Instance.GetCurrentCamera().Parent.GetPosition().X + Platform.Instance.Device.Viewport.Width / 2f * scale ),
					UnitsConverter.ToSimUnits( -SceneManager.Instance.GetCurrentCamera().Parent.GetPosition().Y + Platform.Instance.Device.Viewport.Height / 2f * scale ),
					UnitsConverter.ToSimUnits( -SceneManager.Instance.GetCurrentCamera().Parent.GetPosition().Y - Platform.Instance.Device.Viewport.Height / 2f * scale ),
					0f,
					1f
					);

				_debugView.RenderDebugData( ref _projection, ref _view );
			}
		}

		public void LoadLevel()
		{
			//GameWorldManager.Instance.SpawnEntity( "Camera", "camera1", new Vector3( 0, -200, 800 ) );
			FreeCameraController camControl = new FreeCameraController( SceneManager.Instance.GetCurrentCamera() );
			ControllerRepository.Instance.RegisterController( "camcontrol", camControl );
			FrameUpdateManager.Instance.Register( camControl );
			FrameUpdateManager.Instance.Register( new EditorToolsController() );
		}

		public override void OnEnter()
		{
			Guide.BeginShowKeyboardInput( PlayerIndex.One, "Select level", "Specify the name of the level", "level1", LevelNameInputCallback, null );
		}

		private void LevelNameInputCallback( IAsyncResult ar )
		{
			string levelName = Guide.EndShowKeyboardInput( ar );

			// nothing to do yet
			if ( !ar.IsCompleted )
			{
				return;
			}

			GameWorldManager.Instance.SetState( PrefabRepository.Instance.GetLevelPrefab( @"Content\Levels\" + levelName + ".eql" )._savedState );
			GameWorldManager.Instance.LoadLastState();

			LoadLevel();
			Platform.Instance.PhysicsWorld.Gravity = Vector2.UnitY * Platform.Instance.PhysicsWorld.Gravity.Length();

			_debugViewState = false;
			_actionToggleDebugView = false;

			_debugView = new DebugViewXNA( Platform.Instance.PhysicsWorld );

			_debugView.LoadContent( Platform.Instance.Device, Platform.Instance.Content );
			//_debugView.RemoveFlags( DebugViewFlags.Joint );
			//_debugView.AppendFlags( DebugViewFlags.PerformanceGraph );

			_debugView.TextColor = Color.Black;

			_view = Matrix.Identity;

			//Song song = _contentManager.Load<Song>( "music/Beluga_-_Lost_In_Outer_Space" );
			//Platform.Instance.SoundManager.PlayMusic( song );

			FrameUpdateManager.Instance.Register( HintManager.Instance );
			FrameUpdateManager.Instance.Register( SceneManager.Instance );

			_levelLoaded = true;
		}

		public override void OnClose()
		{
			GameWorldManager.Instance.Clear();
			ControllerRepository.Instance.Clear();
			PhysicsContactManager.Instance.Clear();
			FrameUpdateManager.Instance.Clear();
			Platform.Instance.PhysicsWorld.Clear();
			SceneManager.Instance.Clear();
			Platform.Instance.SoundManager.StopMusic();

			_contentManager.Unload();
		}

		public void LoadLastSateCallback()
		{
			GameWorldManager.Instance.LoadLastState();
		}
	}
}
