using System.Collections.Generic;
using System.Linq;
using UnifiedInputSystem.Events;

namespace UnifiedInputSystem
{
	/// <summary>
	/// Aggregates input from multiple <see cref="IInputProcessor"/>s 
	/// to provide a unified input system
	/// </summary>
	public class InputAggregator
	{
		private readonly List<IInputProcessor> _processors;

		public InputAggregator()
		{
			_processors = new List<IInputProcessor>();
		}

		/// <summary>
		/// Adds a new input source
		/// </summary>
		/// <param name="processor">The input processor</param>
		public void Add( IInputProcessor processor )
		{
			_processors.Add( processor );
		}

		/// <summary>
		/// Updates the input system
		/// </summary>
		/// <param name="time"></param>
		public void Update( Time time )
		{
			foreach ( var inputProcessor in _processors )
			{
				inputProcessor.Update( time );
			}
		}

		/// <summary>
		/// Gets the first event of the specified type, or null if none exists
		/// </summary>
		/// <typeparam name="T">The type of the input event to filter by</typeparam>
		/// <returns>An <see cref="IInputEvent"/> or null if none exists</returns>
		public T GetEvent<T>() where T : IInputEvent
		{
			return GetEvents<T>().FirstOrDefault();
		}

		/// <summary>
		/// Gets all events of the specified type
		/// </summary>
		/// <typeparam name="T">The type of the input event to filter by</typeparam>
		/// <returns>A list of <see cref="IInputEvent"/>s</returns>
		public IEnumerable<T> GetEvents<T>() where T : IInputEvent
		{
			return _processors.SelectMany( p => p.GetEvents().OfType<T>() );
		}

		/// <summary>
		/// Gets all aggregated events
		/// </summary>
		/// <returns>A list of <see cref="IInputEvent"/>s</returns>
		public IEnumerable<IInputEvent> GetAllEvents()
		{
			return _processors.SelectMany( p => p.GetEvents() );
		}
	}
}
