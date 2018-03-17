using System;
using System.Collections;
using System.Collections.Generic;

namespace Dome.Collections
{
	public class StackWrapper<T> : IStack<T>
	{
		private readonly Stack<T> stack;

		/// <summary>
		/// 
		/// </summary>
		/// <param name="stack"></param>
		/// <exception cref="ArgumentNullException" />
		public StackWrapper(Stack<T> stack)
		{
			this.stack = stack ?? throw new ArgumentNullException(nameof(stack));
		}

		public int Count => stack.Count;
		public void Clear() => stack.Clear();
		public void Push(T item) => stack.Push(item);
		public T[] ToArray() => stack.ToArray();
		public void TrimExcess() => stack.TrimExcess();
		public Stack<T>.Enumerator GetEnumerator() => stack.GetEnumerator();

		public bool Contains(T item, IEqualityComparer<T> comparer = null)
		{
			if (comparer == null)
			{
				return stack.Contains(item);
			}
			else
			{
				foreach (T value in stack)
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
		public void CopyTo(T[] array, int arrayIndex = 0) => stack.CopyTo(array, arrayIndex);

		/// <summary>
		/// 
		/// </summary>
		/// <returns></returns>
		/// <exception cref="InvalidOperationException" />
		public T Peek() => stack.Peek();

		/// <summary>
		/// 
		/// </summary>
		/// <returns></returns>
		/// <exception cref="InvalidOperationException" />
		public T Pop() => stack.Pop();

		public override int GetHashCode() => stack.GetHashCode();
		public override bool Equals(object obj) => obj is StackWrapper<T> other && stack == other.stack;

		public static bool operator ==(StackWrapper<T> a, StackWrapper<T> b)
		{
			if (a is null)
				return b is null;
			else if (b is null)
				return false;
			else
				return a.stack == b.stack;
		}

		public static bool operator !=(StackWrapper<T> a, StackWrapper<T> b) => !(a == b);

		IEnumerator<T> IEnumerable<T>.GetEnumerator() => GetEnumerator();
		IEnumerator IEnumerable.GetEnumerator() => GetEnumerator();

		public bool TryPeek(out T result)
		{
#if WINDOWS_UWP
			return stack.TryPeek(out result);
#else
			if (stack.Count > 0)
			{
				result = stack.Peek();
				return true;
			}
			else
			{
				result = default(T);
				return false;
			}
#endif
		}

		public bool TryPop(out T result)
		{
#if WINDOWS_UWP
			return stack.TryPop(out result);
#else
			if (stack.Count > 0)
			{
				result = stack.Pop();
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