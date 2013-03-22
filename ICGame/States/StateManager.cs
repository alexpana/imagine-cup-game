using System.Collections.Generic;
using Microsoft.Xna.Framework.Content;
using VertexArmy.Global;
using VertexArmy.States.Menu;

namespace VertexArmy.States
{
	public sealed class StateManager
	{
		private readonly object _lock = new object();

		private IGameState _requestedGameState;
		private bool _pushRequested;
		private bool _popRequested;

		private readonly Stack<IGameState> _states;
		public IGameState CurrentGameState { get { return _states.Count > 0 ? _states.Peek() : null; } }

		private StateManager()
		{
			_pushRequested = false;
			_popRequested = false;
			_states = new Stack<IGameState>();
		}

		public void ChangeState( GameState newGameState )
		{
			PopState();
			PushState( newGameState );
		}

		public void PopState()
		{
			_popRequested = true;
		}

		public void PushState( GameState newGameState )
		{
			lock ( _lock )
			{
				_pushRequested = true;

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
				case GameState.LevelLoading:
					return new GameStateLevelLoading( contentManager );
				case GameState.PhysicsPresentationRobot:
					return new GameStatePhysicsRobot( contentManager );
			}
			return null;
		}

		public void OnFrameEndCommitStates()
		{
			if ( _popRequested && _states.Count > 0 )
			{
				if ( CurrentGameState != null )
				{
					CurrentGameState.OnClose();
				}

				_states.Pop();
			}

			if ( _pushRequested )
			{
				_requestedGameState.OnEnter();

				_states.Push( _requestedGameState );
				_requestedGameState = null;
			}

			_popRequested = false;
			_pushRequested = false;
		}

		private static StateManager _instance = new StateManager();
		public static StateManager Instance
		{
			get { return _instance; }
		}
	}
}
