using System.Collections.Generic;

namespace VertexArmy.Graphics.ParticleSystem
{
	abstract  class ParticleEmitter
	{
		private ParticleSystem _parent;
		private float _emissionRate;

		private List<Particle> _aliveParticles;

		ParticleEmitter( ParticleSystem parent, float emissionRate )
		{
			_parent = parent;
			_emissionRate = emissionRate;
			_aliveParticles = new List<Particle>();
		}

		protected abstract void InitializeParticle( Particle p );

		public void Render()
		{
			
		}

		public void Update()
		{
			_cleanupDeadParticles();
			if( _shouldEmitNewParticle() )
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
