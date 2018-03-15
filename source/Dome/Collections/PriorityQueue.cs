using System;
using System.Collections;
using System.Collections.Generic;

namespace Dome.Collections
{
	/// <summary>
	/// Queues items in an order determined by a priority associated with each item.
	/// </summary>
	/// <typeparam name="TItem">The type of the items in the queue.</typeparam>
	/// <typeparam name="TPriority"></typeparam>
	public class PriorityQueue<TItem, TPriority> : IQueue<TItem>
	{
		private readonly IComparer<TPriority> priorityComparer;

		private TPriority[] priorities;
		private TItem[] items;

		private int count = 0;
		private int version = 0;

		/// <param name="capacity">The initial capacity of the queue.</param>
		/// <param name="priorityComparer">The comparer to use to order items in the queue by their respective priorities.</param>
		/// <exception cref="ArgumentOutOfRangeException"></exception>
		public PriorityQueue(int capacity, IComparer<TPriority> priorityComparer = null)
		{
			if (capacity < 0)
				throw new ArgumentOutOfRangeException(nameof(capacity), capacity, ExceptionMessages.ArgumentMustNotBeNegative);

			this.priorityComparer = priorityComparer ?? Comparer<TPriority>.Default;

			if (capacity > 0)
			{
				priorities = new TPriority[capacity];
				items = new TItem[capacity];
			}
			else
			{
				priorities = ArrayUtils.GetEmpty<TPriority>();
				items = ArrayUtils.GetEmpty<TItem>();
			}
		}

		/// <param name="priorityComparer">The comparer to use to order items in the queue by their respective priorities.</param>
		public PriorityQueue(IComparer<TPriority> priorityComparer) : this(0, priorityComparer)
		{ }

		public PriorityQueue() : this(null)
		{ }

		/// <summary>
		/// The comparer being used to order items in the queue by their respective priorities.
		/// </summary>
		public IComparer<TPriority> PriorityComparer => priorityComparer;

		/// <summary>
		/// The current capacity of the queue. The capacity will increase automatically if an item is added to the queue when it is full.
		/// </summary>
		public int Capacity => items.Length;

		/// <summary>
		/// The number of items in the queue.
		/// </summary>
		public int Count => count;

		/// <summary>
		/// Places an item in the queue.
		/// </summary>
		/// <param name="item">The item to place in the queue.</param>
		/// <param name="priority">The priority to use to determine where in the queue to place the item.</param>
		public void Enqueue(TItem item, TPriority priority)
		{
			int index = priorities.LowerBoundSearch(0, count, priority, priorityComparer);
			if (items.Length == count)
			{
				int capacity = count == 0 ? 4 : count * 2;

				ArrayUtils.ResizeAndInsert(ref priorities, capacity, index, priority);
				ArrayUtils.ResizeAndInsert(ref items, capacity, index, item);
			}
			else
			{
				priorities.Insert(count, index, priority);
				items.Insert(count, index, item);
			}

			++count;
			IncVersion();
		}

		void IQueue<TItem>.Enqueue(TItem item)
		{
			Enqueue(item, default(TPriority));
		}

		private void DequeueImpl(out TItem item)
		{
			int index = count - 1;
			item = items[index];

			// Clear these array elements to avoid potentially obstructing garbage collection of otherwise unreferenced objects.
			items[index] = default(TItem);
			priorities[index] = default(TPriority);

			--count;
			IncVersion();
		}

		/// <summary>
		/// Removes the item at the front of the queue.
		/// </summary>
		/// <returns>The item removed from the front of the queue.</returns>
		/// <exception cref="InvalidOperationException"></exception>
		public TItem Dequeue()
		{
			if (count == 0)
				throw new InvalidOperationException(ExceptionMessages.CollectionIsEmpty);

			DequeueImpl(out TItem item);
			return item;
		}

		/// <summary>
		/// Removes the item at the front of the queue if the queue is not empty.
		/// </summary>
		/// <param name="item">The item removed from the front of the queue, or default(<typeparamref name="TItem"/>) if the queue is empty.</param>
		/// <returns>True if the queue was not empty, otherwise false.</returns>
		public bool TryDequeue(out TItem item)
		{
			if (count == 0)
			{
				item = default(TItem);
				return false;
			}
			else
			{
				DequeueImpl(out item);
				return true;
			}
		}

		/// <summary>
		/// Retrieves the item at the front of the queue without removing it.
		/// </summary>
		/// <returns>The item at the front of the queue.</returns>
		/// <exception cref="InvalidOperationException"></exception>
		public TItem Peek()
		{
			if (count == 0)
				throw new InvalidOperationException(ExceptionMessages.CollectionIsEmpty);

			int index = count - 1;
			TItem item = items[index];
			return item;
		}

		/// <summary>
		/// Retrieves the item at the front of the queue if the queue is not empty.
		/// </summary>
		/// <param name="item">The item at the front of the queue, or default(<typeparamref name="TItem"/>) if the queue is empty.</param>
		/// <returns></returns>
		public bool TryPeek(out TItem item)
		{
			if (count == 0)
			{
				item = default(TItem);
				return false;
			}
			else
			{
				int index = count - 1;
				item = items[index];
				return true;
			}
		}

		/// <summary>
		/// Removes all items from the queue.
		/// </summary>
		public void Clear()
		{
			Array.Clear(priorities, 0, count);
			Array.Clear(items, 0, count);

			count = 0;
			IncVersion();
		}

		private void IncVersion()
		{
			version = unchecked(version + 1);
		}

		public Enumerator GetEnumerator() => new Enumerator(this);

		// Since the queue begins toward the end of the internal buffer, the enumerator starts there and works backwards to yield the items in the order that they are queued.
		public struct Enumerator : IEnumerator<TItem>
		{
			private readonly PriorityQueue<TItem, TPriority> queue;

			private int index, version;

			internal Enumerator(PriorityQueue<TItem, TPriority> queue)
			{
				this.queue = queue;

				index = queue.count;
				version = queue.version;
			}

			/// <summary>
			/// Moves the enumerator to the next item in the queue. The behaviour of this method is undefined if it is called after it has returned false.
			/// </summary>
			/// <returns>False if all of the items in the queue have been enumerated, otherwise true.</returns>
			/// <exception cref="InvalidOperationException"></exception>
			public bool MoveNext()
			{
				if (version != queue.version)
					throw new InvalidOperationException(ExceptionMessages.CollectionContentsHaveChanged);

				--index;
				bool isValid = index >= 0;
				return isValid;
			}

			/// <summary>
			/// The item in the queue that the enumerator is currently on.
			/// The behaviour of this property is undefined if it is invoked before calling <see cref="MoveNext"/> for first the time or after <see cref="MoveNext"/> has returned false.
			/// </summary>
			public TItem Current => queue.items[index];

			/// <summary>
			/// Frees the resources in use by the enumerator.
			/// The behaviour of the enumerator is undefined if it used after this method has been called.
			/// </summary>
			public void Dispose()
			{
				// Nothing to do.
			}

			/// <summary>
			/// Resets the enumerator to the position before the first item in the queue.
			/// </summary>
			public void Reset()
			{
				index = queue.count;
				version = queue.version;
			}

			object IEnumerator.Current => Current;
		}

		IEnumerator<TItem> IEnumerable<TItem>.GetEnumerator() => GetEnumerator();

		IEnumerator IEnumerable.GetEnumerator() => GetEnumerator();
	}
}