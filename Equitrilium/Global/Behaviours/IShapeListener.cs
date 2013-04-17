namespace VertexArmy.Global.Behaviours
{
	public interface IShapeListener
	{
		void OnEnterShape();
		void OnExitShape();
		void OnEachFrameInsideShape();
	}
}
