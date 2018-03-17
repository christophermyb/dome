using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;

namespace Dome.Collections
{
	/// <summary>
	/// Stores items in a list
	/// </summary>
	/// <typeparam name="T"></typeparam>
	public class RingBuffer<T> : IReadOnlyList<T>, IList<T>, IQueue<T>, IStack<T>
	{
		private T[] items;

		private int startIndex = 0, count = 0, version = 0;

		/// <summary>
		/// 
		/// </summary>
		/// <param name="capacity"></param>
		/// <exception cref="ArgumentOutOfRangeException" />
		public RingBuffer(int capacity)
		{
			if (capacity < 0)
				throw new ArgumentOutOfRangeException(nameof(capacity));

			if (capacity > 0)
				items = new T[capacity];
			else
				items = ArrayUtils.GetEmpty<T>();
		}

		public RingBuffer() : this(0)
		{ }

		public int Capacity => items.Length;
		public int Count => count;
		public void AddLast(T item) => AddLastImpl(ref item);
		public bool Contains(T item, IEqualityComparer<T> comparer = null) => ContainsImpl(ref item, comparer);
		public int IndexOf(T item, IEqualityComparer<T> comparer = null) => IndexOfImpl(ref item, comparer);
		public bool Remove(T item, IEqualityComparer<T> comparer = null) => RemoveImpl(ref item, comparer);

		private void IncVersion()
		{
			version = unchecked(version + 1);
		}

		private int ToItemIndex(int listIndex)
		{
			Debug.Assert(listIndex >= 0);
			Debug.Assert(listIndex <= count);

			int itemIndex = (startIndex + listIndex) % items.Length;
			return itemIndex;
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
					throw new ArgumentOutOfRangeException(nameof(index), index, ExceptionMessages.ArgumentMayNotBeNegative);

				if (index >= count)
					throw new ArgumentOutOfRangeException(nameof(index), index, ExceptionMessages.ArgumentMustBeLessThanCount);

				int itemIndex = ToItemIndex(index);
				return items[itemIndex];
			}
			set
			{
				if (index < 0)
					throw new ArgumentOutOfRangeException(nameof(index), index, ExceptionMessages.ArgumentMayNotBeNegative);

				if (index >= count)
					throw new ArgumentOutOfRangeException(nameof(index), index, ExceptionMessages.ArgumentMustBeLessThanCount);

				int itemIndex = ToItemIndex(index);
				items[itemIndex] = value;
			}
		}

		/// <summary>
		/// 
		/// </summary>
		/// <returns></returns>
		/// <exception cref="InvalidOperationException" />
		public T PeekFirst()
		{
			PeekFirst(out T item);
			return item;
		}

		/// <summary>
		/// 
		/// </summary>
		/// <returns></returns>
		/// <exception cref="InvalidOperationException" />
		public T PopFirst()
		{
			PopFirst(out T item);
			return item;
		}

		/// <summary>
		/// 
		/// </summary>
		/// <returns></returns>
		/// <exception cref="InvalidOperationException" />
		public T PeekLast()
		{
			PeekLast(out T item);
			return item;
		}

		/// <summary>
		/// 
		/// </summary>
		/// <returns></returns>
		/// <exception cref="InvalidOperationException" />
		public T PopLast()
		{
			PopLast(out T item);
			return item;
		}

		private void IncrementCount()
		{
			++count;
			IncVersion();
		}

		private void SetItemBuffer(T[] buffer)
		{
			items = buffer;
			startIndex = 0;
		}

		private void AddFirstImpl(ref T item)
		{
			if (count == items.Length)
			{
				T[] items = CreateHigherCapacityItemBuffer();
				CopyToImpl(0, count, items, 1);
				items[0] = item;

				SetItemBuffer(items);
			}
			else
			{
				startIndex = MathUtils.Wrap(items.Length, startIndex - 1);
				items[startIndex] = item;
			}

			IncrementCount();
		}

		public void AddFirst(T item) => AddFirstImpl(ref item);

		private void AddLastImpl(ref T item)
		{
			if (count == items.Length)
			{
				T[] items = CreateHigherCapacityItemBuffer();
				CopyToImpl(0, count, items, 0);
				items[count] = item;

				SetItemBuffer(items);
			}
			else
			{
				int itemIndex = ToItemIndex(count);
				items[itemIndex] = item;
			}

			IncrementCount();
		}

		public void Clear()
		{
			for (int i = 0; i < count; ++i)
			{
				int itemIndex = ToItemIndex(i);
				items[itemIndex] = default(T);
			}

			count = 0;
			IncVersion();
		}

		private bool ContainsImpl(ref T item, IEqualityComparer<T> comparer)
		{
			int index = IndexOfImpl(ref item, comparer);
			bool found = index >= 0;
			return found;
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
			CollectionUtils.CheckCopyToArguments(array, arrayIndex, count);
			CopyToImpl(0, count, array, arrayIndex);
		}

		private int IndexOfImpl(ref T item, IEqualityComparer<T> comparer)
		{
			if (comparer == null)
				comparer = EqualityComparer<T>.Default;

			for (int i = 0; i < count; ++i)
			{
				int itemIndex = ToItemIndex(i);
				T value = items[itemIndex];
				if (comparer.Equals(item, value))
					return i;
			}

			return -1;
		}

		private void CopyToImpl(int index, int length, T[] array, int arrayIndex)
		{
			Debug.Assert(index >= 0);
			Debug.Assert(length >= 0);
			Debug.Assert(index + length <= count);

			for (int i = 0; i < length; ++i)
			{
				int itemIndex = ToItemIndex(i + index);
				array[arrayIndex + i] = items[itemIndex];
			}
		}

		private T[] CreateHigherCapacityItemBuffer()
		{
			int capacity = count == 0 ? 4 : count * 2;
			return new T[capacity];
		}

		/// <summary>
		/// 
		/// </summary>
		/// <param name="index"></param>
		/// <param name="item"></param>
		/// <exception cref="ArgumentOutOfRangeException" />
		public void Insert(int index, T item)
		{
			if (index < 0)
				throw new ArgumentOutOfRangeException(nameof(index), ExceptionMessages.ArgumentMayNotBeNegative);

			if (index > count)
				throw new ArgumentOutOfRangeException(nameof(index), ExceptionMessages.ArgumentMayNotBeLargerThanCount);

			if (count == items.Length)
			{
				T[] items = CreateHigherCapacityItemBuffer();
				CopyToImpl(0, index, items, 0);
				CopyToImpl(index, count - index, items, index + 1);
				items[index] = item;

				SetItemBuffer(items);
			}
			else
			{
				int itemIndexOfNext = ToItemIndex(count);
				for (int i = count - 1; i >= index; --i)
				{
					int itemIndex = ToItemIndex(i);
					items[itemIndexOfNext] = items[itemIndex];
					itemIndexOfNext = itemIndex;
				}

				items[itemIndexOfNext] = item;
			}

			IncrementCount();
		}

		private void DecrementCount()
		{
			--count;
			IncVersion();
		}

		private void RemoveAtImpl(int index)
		{
			Debug.Assert(index < count);

			int previousItemIndex = ToItemIndex(index);
			++index;
			while (index < count)
			{
				int itemIndex = ToItemIndex(index);
				items[previousItemIndex] = items[itemIndex];
				previousItemIndex = itemIndex;

				++index;
			}

			items[previousItemIndex] = default(T);

			DecrementCount();
		}

		private bool RemoveImpl(ref T item, IEqualityComparer<T> comparer)
		{
			if (comparer == null)
				comparer = EqualityComparer<T>.Default;

			for (int i = 0; i < count; ++i)
			{
				int itemIndex = ToItemIndex(i);
				T value = items[itemIndex];
				if (comparer.Equals(item, value))
				{
					RemoveAtImpl(i);
					return true;
				}
			}

			return false;
		}

		/// <summary>
		/// 
		/// </summary>
		/// <param name="index"></param>
		/// <exception cref="ArgumentOutOfRangeException" />
		public void RemoveAt(int index)
		{
			if (index < 0)
				throw new ArgumentOutOfRangeException(nameof(index), index, ExceptionMessages.ArgumentMayNotBeNegative);

			if (index >= count)
				throw new ArgumentOutOfRangeException(nameof(index), index, ExceptionMessages.ArgumentMustBeLessThanCount);

			RemoveAtImpl(index);
		}

		private void PeekFirst(out T result)
		{
			if (count == 0)
				throw new InvalidOperationException(ExceptionMessages.CollectionIsEmpty);

			result = items[startIndex];
		}

		public bool TryPeekFirst(out T result)
		{
			if (count > 0)
			{
				result = items[startIndex];
				return true;
			}
			else
			{
				result = default(T);
				return false;
			}
		}

		private void PopFirstImpl(out T result)
		{
			result = items[startIndex];
			items[startIndex] = default(T);
			startIndex = MathUtils.Wrap(items.Length, startIndex + 1);
			DecrementCount();
		}

		private void PopFirst(out T result)
		{
			if (count == 0)
				throw new InvalidOperationException(ExceptionMessages.CollectionIsEmpty);

			PopFirstImpl(out result);
		}

		public bool TryPopFirst(out T result)
		{
			if (count > 0)
			{
				PopFirstImpl(out result);
				return true;
			}
			else
			{
				result = default(T);
				return false;
			}
		}

		private void PeekLast(out T result)
		{
			if (count == 0)
				throw new InvalidOperationException(ExceptionMessages.CollectionIsEmpty);

			int itemIndex = ToItemIndex(count - 1);
			result = items[itemIndex];
		}

		public bool TryPeekLast(out T result)
		{
			if (count > 0)
			{
				int itemIndex = ToItemIndex(count - 1);
				result = items[itemIndex];
				return true;
			}
			else
			{
				result = default(T);
				return false;
			}
		}

		private void PopLastImpl(out T result)
		{
			int itemIndex = ToItemIndex(count - 1);
			result = items[itemIndex];
			items[itemIndex] = default(T);
			DecrementCount();
		}

		private void PopLast(out T result)
		{
			if (count == 0)
				throw new InvalidOperationException(ExceptionMessages.CollectionIsEmpty);

			PopLastImpl(out result);
		}

		public bool TryPopLast(out T result)
		{
			if (count > 0)
			{
				PopLastImpl(out result);
				return true;
			}
			else
			{
				result = default(T);
				return false;
			}
		}

		public Enumerator GetEnumerator() => new Enumerator(this);

		public struct Enumerator : IEnumerator<T>
		{
			private readonly RingBuffer<T> buffer;

			private int index, version;

			internal Enumerator(RingBuffer<T> buffer)
			{
				this.buffer = buffer;

				index = -1;
				version = buffer.version;
			}

			/// <summary>
			/// 
			/// </summary>
			/// <returns></returns>
			/// <exception cref="InvalidOperationException" />
			/// <exception cref="NullReferenceException" />
			public bool MoveNext()
			{
				if (version != buffer.version)
					throw new InvalidOperationException(ExceptionMessages.CollectionContentsHaveChanged);

				++index;
				bool valid = index < buffer.count;
				return valid;
			}

			/// <summary>
			/// 
			/// </summary>
			/// <exception cref="NullReferenceException" />
			public T Current
			{
				get
				{
					int itemIndex = buffer.ToItemIndex(index);
					return buffer.items[itemIndex];
				}
			}

			public void Dispose()
			{
				// Nothing to do.
			}

			/// <summary>
			/// 
			/// </summary>
			/// <exception cref="NullReferenceException" />
			public void Reset()
			{
				index = -1;
				version = buffer.version;
			}

			object IEnumerator.Current => Current;
		}

		void IQueue<T>.Enqueue(T item) => AddLastImpl(ref item);
		bool IQueue<T>.TryPeek(out T result) => TryPeekFirst(out result);
		bool IQueue<T>.TryDequeue(out T result) => TryPopFirst(out result);

		T IQueue<T>.Peek()
		{
			PeekFirst(out T item);
			return item;
		}

		T IQueue<T>.Dequeue()
		{
			PopFirst(out T item);
			return item;
		}

		void IStack<T>.Push(T item) => AddFirstImpl(ref item);
		bool IStack<T>.TryPeek(out T result) => TryPeekFirst(out result);
		bool IStack<T>.TryPop(out T result) => TryPopFirst(out result);

		T IStack<T>.Peek()
		{
			PeekFirst(out T item);
			return item;
		}

		T IStack<T>.Pop()
		{
			PopFirst(out T item);
			return item;
		}

		int IList<T>.IndexOf(T item) => IndexOfImpl(ref item, null);

		void ICollection<T>.Add(T item) => AddLastImpl(ref item);
		bool ICollection<T>.Remove(T item) => RemoveImpl(ref item, null);
		bool ICollection<T>.Contains(T item) => ContainsImpl(ref item, null);
		bool ICollection<T>.IsReadOnly => false;

		IEnumerator<T> IEnumerable<T>.GetEnumerator() => GetEnumerator();

		IEnumerator IEnumerable.GetEnumerator() => GetEnumerator();
	}
}