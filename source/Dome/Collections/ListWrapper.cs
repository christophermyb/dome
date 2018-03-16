using System;
using System.Collections;
using System.Collections.Generic;

namespace Dome.Collections
{
	public class ListWrapper<T> : IReadOnlyList<T>, IList<T>, IStack<T>
	{
		private readonly List<T> list;

		/// <summary>
		/// 
		/// </summary>
		/// <param name="list"></param>
		/// <exception cref="ArgumentNullException" />
		public ListWrapper(List<T> list)
		{
			this.list = list ?? throw new ArgumentNullException(nameof(list));
		}

		public int Count => list.Count;
		public void Clear() => list.Clear();
		public bool Contains(T item) => list.Contains(item);
		public T[] ToArray() => list.ToArray();
		public void TrimExcess() => list.TrimExcess();
		public void Add(T item) => list.Add(item);
		public bool Remove(T item) => list.Remove(item);
		public int IndexOf(T item) => list.IndexOf(item);
		public List<T>.Enumerator GetEnumerator() => list.GetEnumerator();

		/// <summary>
		/// 
		/// </summary>
		/// <param name="array"></param>
		/// <param name="arrayIndex"></param>
		/// <exception cref="ArgumentNullException" />
		/// <exception cref="ArgumentOutOfRangeException" />
		/// <exception cref="ArgumentException" />
		public void CopyTo(T[] array, int arrayIndex = 0) => list.CopyTo(array, arrayIndex);

		/// <summary>
		/// 
		/// </summary>
		/// <param name="index"></param>
		/// <returns></returns>
		/// <exception cref="ArgumentOutOfRangeException" />
		public T this[int index]
		{
			get => list[index];
			set => list[index] = value;
		}

		/// <summary>
		/// 
		/// </summary>
		/// <param name="index"></param>
		/// <param name="item"></param>
		/// <exception cref="ArgumentOutOfRangeException" />
		public void Insert(int index, T item) => list.Insert(index, item);

		/// <summary>
		/// 
		/// </summary>
		/// <param name="index"></param>
		/// <exception cref="ArgumentOutOfRangeException" />
		public void RemoveAt(int index) => list.RemoveAt(index);

		/// <summary>
		/// 
		/// </summary>
		/// <returns></returns>
		/// <exception cref="InvalidOperationException" />
		public T PeekLast()
		{
			int index = list.Count - 1;
			if (index < 0)
				throw new InvalidOperationException(ExceptionMessages.CollectionIsEmpty);
			else
				return list[index];
		}

		/// <summary>
		/// 
		/// </summary>
		/// <returns></returns>
		/// <exception cref="InvalidOperationException" />
		public T PopLast()
		{
			int index = list.Count - 1;
			if (index < 0)
			{
				throw new InvalidOperationException(ExceptionMessages.CollectionIsEmpty);
			}
			else
			{
				T item = list[index];
				list.RemoveAt(index);
				return item;
			}
		}

		public bool TryPeekLast(out T result)
		{
			int index = list.Count - 1;
			if (index < 0)
			{
				result = default(T);
				return false;
			}
			else
			{
				result = list[index];
				return true;
			}
		}

		public bool TryPopLast(out T result)
		{
			int index = list.Count - 1;
			if (index < 0)
			{
				result = default(T);
				return false;
			}
			else
			{
				result = list[index];
				list.RemoveAt(index);
				return true;
			}
		}

		public override int GetHashCode() => list.GetHashCode();
		public override bool Equals(object obj) => obj is ListWrapper<T> other && list == other.list;

		public static bool operator ==(ListWrapper<T> a, ListWrapper<T> b)
		{
			if (a is null)
				return b is null;
			else if (b is null)
				return false;
			else
				return a.list == b.list;
		}

		public static bool operator !=(ListWrapper<T> a, ListWrapper<T> b) => !(a == b);

		IEnumerator<T> IEnumerable<T>.GetEnumerator() => list.GetEnumerator();
		IEnumerator IEnumerable.GetEnumerator() => list.GetEnumerator();
		bool ICollection<T>.IsReadOnly => false;
		void IStack<T>.Push(T item) => list.Add(item);
		T IStack<T>.Peek() => PeekLast();
		T IStack<T>.Pop() => PopLast();
		bool IStack<T>.TryPeek(out T result) => TryPeekLast(out result);
		bool IStack<T>.TryPop(out T result) => TryPopLast(out result);
	}
}