using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.Xna.Framework.Content;
using VertexArmy.Global;
using VertexArmy.States.Menu;

namespace VertexArmy.States
{
	public sealed class StateManager
	{
		private readonly object _lock = new object();

		private bool _statesChangeRequested;

		private readonly Stack<IGameState> _requestedStates;
		private Stack<IGameState> _states;

		public IGameState CurrentGameState
		{
			get { return _states.Count > 0 ? _states.Peek() : null; }
		}

		private StateManager()
		{
			_statesChangeRequested = true;
			_states = new Stack<IGameState>();
			_requestedStates = new Stack<IGameState>();
		}

		public void ChangeState( GameState newGameState )
		{
			PopState();
			PushState( newGameState );
		}

		public void PopState()
		{
			if ( _requestedStates.Count == 0 )
			{
				return;
			}

			lock ( _lock )
			{
				_statesChangeRequested = true;

				_requestedStates.Pop();
			}
		}

		public void PushState( GameState newGameState )
		{
			lock ( _lock )
			{
				_statesChangeRequested = true;

				ContentManager contentManager = new ContentManager( Platform.Instance.Game.Services, "Content" );
				_requestedStates.Push( CreateGameStateInstance( newGameState, contentManager ) );
			}
		}

		private static IGameState CreateGameStateInstance( GameState state, ContentManager contentManager )
		{
			switch ( state )
			{
				case GameState.Menu:
					return new GameStateMenu( contentManager );
				case GameState.Pause:
					return new GameStatePaused( contentManager );
				case GameState.ModelView:
					return new GameStateModelViewer( contentManager );
				case GameState.PhysicsPresentationRobot:
					return new GameStatePhysicsRobot( contentManager );
				case GameState.TutorialLevel:
					return new GameStateTutorial( contentManager );
				case GameState.SelectLevelMenu:
					return new GameStateSelectLevelMenu( contentManager );
				case GameState.Demo2:
					return new GameStateDemo2( contentManager );
#if WINDOWS
				case GameState.Editor:
					return new GameStateEditor( contentManager );
#endif
				default:
					return new GameStateMenu( contentManager );
			}
		}

		public void OnFrameEndCommitStates()
		{
			if ( _statesChangeRequested )
			{
				foreach ( var gameState in _states.Except( _requestedStates ) )
				{
					gameState.OnClose();
				}

				foreach ( var gameState in _requestedStates.Except( _states ) )
				{
					gameState.OnEnter();
				}

				IGameState[] newStates = _requestedStates.ToArray();
				Array.Reverse( newStates );

				_states = new Stack<IGameState>( newStates );
				_statesChangeRequested = false;
			}
		}

		private static readonly StateManager _instance = new StateManager();

		public static StateManager Instance
		{
			get { return _instance; }
		}
	}
}
