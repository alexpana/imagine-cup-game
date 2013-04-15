using VertexArmy.Global.Behaviours;
using VertexArmy.Global.Hints;
using VertexArmy.Global.Managers;


namespace VertexArmy.Global.ShapeListeners
{
	class HintShapeListener : IShapeListener
	{
		private readonly FadeHint _hint;
		private readonly bool _onlyOnce;
		private readonly bool _stopOnExit;

		private int _timesPlayed;

		public HintShapeListener ( FadeHint hint ) : this(hint, false, true)
		{
		
		}

		public HintShapeListener ( FadeHint hint, bool onlyOnce ) : this(hint, onlyOnce, true)
		{
			
		}

		public HintShapeListener( FadeHint hint, bool onlyOnce, bool stopOnExitArea )
		{
			_hint = hint;
			_onlyOnce = onlyOnce;
			_stopOnExit = stopOnExitArea;
			_timesPlayed = 0;
		}

		public void OnEnterShape()
		{
			HintManager.Instance.SpawnHint( _hint );
			if( _onlyOnce && _timesPlayed == 0 )
				_hint.StartAsync();
			else if (!_onlyOnce)
				_hint.StartAsync();

			_timesPlayed++;
		}

		public void OnExitShape()
		{
			if(_stopOnExit)
				_hint.StopHintAsync();
		}

		public void OnEachFrameInsideShape()
		{
			
		}
	}
}
