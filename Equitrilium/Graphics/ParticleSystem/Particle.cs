using Microsoft.Xna.Framework;

namespace VertexArmy.Graphics.ParticleSystem
{
	internal class Particle
	{
		//public ParticleEmitter Parent;
		public Vector3 Position { get; set; }
		public float Lifetime { get; set; }
		public float Age { get; set; }
		public Vector3 Direction { get; set; }
		public float Speed { get; set; }
		public float Rotation { get; set; }
		public float Scale { get; set; }
		public float Alpha { get; set; }
		public Material RenderMaterial { get; set; }
	}
}
