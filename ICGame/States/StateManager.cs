using System;
using Microsoft.Xna.Framework.Content;
using VertexArmy.Global;

namespace VertexArmy.States
{
	public sealed class StateManager
	{
		private static readonly object _dummySync = new Object();

		private bool _requestedStateChange;
		private IGameState _requestedGameState;

		public static StateManager Instance
		{
			get { return StateManagerInstanceHolder.Instance; }
		}

		public IGameState CurrentGameState { get; private set; }

		private StateManager()
		{
			_requestedStateChange = false;
		}

		public void ChangeState( GameState newGameState )
		{
			lock ( _dummySync )
			{
				_requestedStateChange = true;
				ContentManager contentManager = new ContentManager( Platform.Instance.Game.Services, "Content" );
				_requestedGameState = CreateGameStateInstance( newGameState, contentManager );
			}
		}

		private static IGameState CreateGameStateInstance( GameState state, ContentManager contentManager )
		{
			switch ( state )
			{
				case GameState.Loading:
					return new GameStateLoading( contentManager );
				case GameState.Presentation:
					return new GameStatePresentation( contentManager );
				case GameState.Menu:
					return new GameStateMenu( contentManager );
				case GameState.Pause:
					return new GameStatePaused( contentManager );
				case GameState.RoomPreview:
					return new GameStateRoomPreview( contentManager );
				case GameState.ModelView:
					return new GameStateModelViewer( contentManager );
				case GameState.PhysicsPresentation:
					return new GameStatePhysics( contentManager );
				case GameState.LevelLoading:
					return new GameStateLevelLoading( contentManager );
			}
			return null;
		}

		public void OnFrameEndCommitStates()
		{
			if ( _requestedStateChange )
			{
				_requestedGameState.OnEnter();

				if ( CurrentGameState != null )
				{
					CurrentGameState.OnClose();
				}

				CurrentGameState = _requestedGameState;

				_requestedStateChange = false;
			}
		}

		private static class StateManagerInstanceHolder
		{
			// ReSharper disable MemberHidesStaticFromOuterClass
			public static readonly StateManager Instance = new StateManager();
			// ReSharper restore MemberHidesStaticFromOuterClass
		}
	}
}
