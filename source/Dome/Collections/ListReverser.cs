using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;

namespace Dome.Collections
{
	public class ListReverser<T> : IReadOnlyList<T>, IStack<T>
	{
		private readonly IList<T> list;

		internal ListReverser(IList<T> list)
		{
			Debug.Assert(list != null);
			this.list = list;
		}

		public int Count => list.Count;
		public void Push(T item) => list.Add(item);
		public void Clear() => list.Clear();
		public bool TryPeek(out T result) => list.TryPeekLast(out result);
		public bool TryPop(out T result) => list.TryPopLast(out result);

		/// <summary>
		/// 
		/// </summary>
		/// <returns></returns>
		/// <exception cref="InvalidOperationException" />
		public T Peek() => list.PeekLast();

		/// <summary>
		/// 
		/// </summary>
		/// <returns></returns>
		/// <exception cref="InvalidOperationException" />
		public T Pop() => list.PopLast();

		public bool Contains(T item, IEqualityComparer<T> comparer = null)
		{
			if (comparer == null)
				return list.Contains(item);

			for (int i = list.Count - 1; i >= 0; --i)
			{
				T value = list[i];
				if (comparer.Equals(item, value))
					return true;
			}

			return false;
		}

		/// <summary>
		/// 
		/// </summary>
		/// <param name="array"></param>
		/// <param name="arrayIndex"></param>
		/// <exception cref="ArgumentNullException" />
		/// <exception cref="ArgumentOutOfRangeException" />
		/// <exception cref="ArgumentException" />
		public void CopyTo(T[] array, int arrayIndex = 0)
		{
			int count = list.Count;
			CollectionUtils.CheckCopyToArguments(array, arrayIndex, count);

			// Copy in reverse order.
			for (int i = 0; i < count; ++i)
			{
				int listIndex = count - i - 1;
				array[i + arrayIndex] = list[listIndex];
			}
		}

		public IEnumerator<T> GetEnumerator()
		{
			// Enumerate in reverse order.
			for (int i = list.Count - 1; i >= 0; --i)
			{
				yield return list[i];
			}
		}

		/// <summary>
		/// 
		/// </summary>
		/// <param name="index"></param>
		/// <exception cref="ArgumentOutOfRangeException" />
		public T this[int index]
		{
			get
			{
				if (index < 0)
					throw new ArgumentOutOfRangeException(nameof(index), ExceptionMessages.ArgumentMayNotBeNegative);

				int count = list.Count;
				if (index >= count)
					throw new ArgumentOutOfRangeException(nameof(index), ExceptionMessages.ArgumentMustBeLessThanCount);

				int listIndex = count - index - 1;
				return list[listIndex];
			}
		}

		IEnumerator IEnumerable.GetEnumerator() => GetEnumerator();

		public override int GetHashCode() => list.GetHashCode();
		public override bool Equals(object obj) => obj is ListReverser<T> other && list == other.list;
	}
}