namespace VertexArmy.Global.Behaviours
{
	public interface IParameter
	{
		bool Null { get; set; }
		bool Input { get; set; }
		bool Output { get; set; }
		bool Alive { get; set; }
	}
}
