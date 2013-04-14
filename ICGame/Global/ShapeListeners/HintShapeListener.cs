using VertexArmy.Global.Behaviours;
using VertexArmy.Global.Hints;
using VertexArmy.Global.Managers;


namespace VertexArmy.Global.ShapeListeners
{
	class HintShapeListener : IShapeListener
	{
		private FadeHint _hint;
		public HintShapeListener ( FadeHint hint )
		{
			_hint = hint;
			HintManager.Instance.SpawnHint( hint );
		}

		public void OnEnterShape()
		{
			if(_hint != null)
				_hint.StartAsync();
		}

		public void OnExitShape()
		{
			_hint.StopHintAsync();
		}

		public void OnEachFrameInsideShape()
		{
			
		}
	}
}
