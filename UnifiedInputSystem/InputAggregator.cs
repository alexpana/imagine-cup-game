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
		/// <param name="processor">The input processor to add</param>
		public void Add( IInputProcessor processor )
		{
			_processors.Add( processor );
		}

		/// <summary>
		/// Adds a new input source to the front of the processors list
		/// </summary>
		/// <param name="processor">The input processor to add</param>
		public void AddToFront( IInputProcessor processor )
		{
			_processors.Insert( 0, processor );
		}

		/// <summary>
		/// Removes the specified processor
		/// </summary>
		/// <param name="processor">The input processor to remove</param>
		public void Remove( IInputProcessor processor )
		{
			_processors.Remove( processor );
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
		/// <typeparam name="TEventType">The type of the input event to filter by</typeparam>
		/// <returns>An <see cref="IInputEvent"/> or null if none exists</returns>
		public TEventType GetEvent<TEventType>() where TEventType : IInputEvent
		{
			return GetEvents<TEventType>().FirstOrDefault();
		}

		/// <summary>
		/// Gets all events of the specified type
		/// </summary>
		/// <typeparam name="TEventType">The type of the input event to filter by</typeparam>
		/// <returns>A list of <see cref="IInputEvent"/>s</returns>
		public IEnumerable<TEventType> GetEvents<TEventType>() where TEventType : IInputEvent
		{
			return _processors.SelectMany( p => p.GetEvents().OfType<TEventType>() );
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
