using System.Collections.Generic;

namespace VertexArmy.Graphics.ParticleSystem
{
	class ParticleCache
	{
		private const int InitialCapacity = 500;

		private readonly Dictionary<Particle, bool> _particles;

		private static ParticleCache _instance;

		public static ParticleCache Instance
		{
			get { return _instance ?? ( _instance = new ParticleCache( ) ); }
		}

		ParticleCache()
		{
			_particles = new Dictionary<Particle, bool>( InitialCapacity );

			for ( var i = 0; i < InitialCapacity; ++i )
			{
				_particles.Add( new Particle(), true );
			}
		}

		private Particle NextAvailableParticle()
		{
			foreach( KeyValuePair<Particle, bool> p in _particles )
			{
				if( p.Value )
				{
					return p.Key;
				}
			}
			return null;
		}

		private Particle CreateNewParticle()
		{
			Particle p = new Particle( );
			_particles.Add( p, true );
			return p;
		}

		public Particle NewParticle()
		{
			Particle p = NextAvailableParticle( ) ?? CreateNewParticle( );
			_particles[p] = false;
			return p;
		}

		public void Delete( Particle p )
		{
			_particles[p] = true;
		}
	}
}
