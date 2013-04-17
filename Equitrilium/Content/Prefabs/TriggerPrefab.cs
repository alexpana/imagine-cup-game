using VertexArmy.GameWorld.Prefabs;

namespace VertexArmy.Content.Prefabs
{
	public class TriggerPrefab
	{
		public static PrefabEntity CreatePrefab()
		{
			PrefabEntity trigger = new PrefabEntity { Name = "Trigger" };
			return trigger;
		}
	}
}
