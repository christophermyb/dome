using System;
using System.Collections;
using System.Collections.Generic;

namespace Dome.Collections
{
	public class QueueWrapper<T> : IQueue<T>
	{
		private readonly Queue<T> queue;

		/// <summary>
		/// 
		/// </summary>
		/// <param name="queue"></param>
		/// <exception cref="ArgumentNullException" />
		public QueueWrapper(Queue<T> queue)
		{
			this.queue = queue ?? throw new ArgumentNullException(nameof(queue));
		}

		public int Count => queue.Count;
		public void Clear() => queue.Clear();
		public void Enqueue(T item) => queue.Enqueue(item);
		public T[] ToArray() => queue.ToArray();
		public void TrimExcess() => queue.TrimExcess();
		public Queue<T>.Enumerator GetEnumerator() => queue.GetEnumerator();

		public bool Contains(T item, IEqualityComparer<T> comparer = null)
		{
			if (comparer == null)
			{
				return queue.Contains(item);
			}
			else
			{
				foreach (T value in queue)
				{
					if (comparer.Equals(item, value))
						return true;
				}

				return false;
			}
		}

		/// <summary>
		/// 
		/// </summary>
		/// <param name="array"></param>
		/// <param name="arrayIndex"></param>
		/// <exception cref="ArgumentNullException" />
		/// <exception cref="ArgumentOutOfRangeException" />
		/// <exception cref="ArgumentException" />
		public void CopyTo(T[] array, int arrayIndex = 0) => queue.CopyTo(array, arrayIndex);

		/// <summary>
		/// 
		/// </summary>
		/// <returns></returns>
		/// <exception cref="InvalidOperationException" />
		public T Peek() => queue.Peek();

		/// <summary>
		/// 
		/// </summary>
		/// <returns></returns>
		/// <exception cref="InvalidOperationException" />
		public T Dequeue() => queue.Dequeue();

		public override int GetHashCode() => queue.GetHashCode();
		public override bool Equals(object obj) => obj is QueueWrapper<T> other && queue == other.queue;

		public static bool operator ==(QueueWrapper<T> a, QueueWrapper<T> b)
		{
			if (a is null)
				return b is null;
			else if (b is null)
				return false;
			else
				return a.queue == b.queue;
		}

		public static bool operator !=(QueueWrapper<T> a, QueueWrapper<T> b) => !(a == b);

		IEnumerator<T> IEnumerable<T>.GetEnumerator() => queue.GetEnumerator();
		IEnumerator IEnumerable.GetEnumerator() => queue.GetEnumerator();

		public bool TryPeek(out T result)
		{
#if WINDOWS_UWP
			return queue.TryPeek(out result);
#else
			if (queue.Count > 0)
			{
				result = queue.Peek();
				return true;
			}
			else
			{
				result = default(T);
				return false;
			}
#endif
		}

		public bool TryDequeue(out T result)
		{
#if WINDOWS_UWP
			return queue.TryDequeue(out result);
#else
			if (queue.Count > 0)
			{
				result = queue.Dequeue();
				return true;
			}
			else
			{
				result = default(T);
				return false;
			}
#endif
		}
	}
}