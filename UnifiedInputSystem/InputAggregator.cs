using System.Collections.Generic;

namespace UnifiedInputSystem
{
	public class InputAggregator
	{
		private readonly List<IInputProcessor> _processors;

		public InputAggregator()
		{
			_processors = new List<IInputProcessor>();
		}

		public void Add( IInputProcessor processor )
		{
			_processors.Add( processor );
		}

		public void Update( Time time )
		{
			foreach ( var inputProcessor in _processors )
			{
				inputProcessor.Update( time );
			}
		}
	}
}
