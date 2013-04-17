using System.Collections.Generic;

namespace VertexArmy.Graphics.ParticleSystem
{
	internal abstract class ParticleEmitter
	{
		private ParticleSystem _parent;
		private float _emissionRate;

		private readonly List<Particle> _aliveParticles;

		private ParticleEmitter( ParticleSystem parent, float emissionRate )
		{
			_parent = parent;
			_emissionRate = emissionRate;
			_aliveParticles = new List<Particle>();
		}

		protected abstract void InitializeParticle( Particle p );

		public void Render()
		{
			// Call the renderer
		}

		public void Update()
		{
			_cleanupDeadParticles();
			if ( _shouldEmitNewParticle() )
			{
				EmitNewParticle();
			}

			_updateParticles();
		}

		private void _cleanupDeadParticles()
		{
		}

		private bool _shouldEmitNewParticle()
		{
			return false;
		}

		private void EmitNewParticle()
		{
			Particle p = ParticleCache.Instance.NewParticle();
			InitializeParticle( p );
			_aliveParticles.Add( p );
		}

		private void _updateParticles()
		{
		}
	}
}
