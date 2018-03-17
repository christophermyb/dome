using System;
using System.Collections.Generic;

namespace Dome.Collections
{
	/// <summary>
	/// Handy methods for working with lists.
	/// </summary>
	public static class ListUtils
	{
		/// <summary>
		/// 
		/// </summary>
		/// <typeparam name="T"></typeparam>
		/// <param name="list"></param>
		/// <returns></returns>
		/// <exception cref="ArgumentNullException" />
		/// <exception cref="ArgumentException" />
		public static ListReverser<T> Reverse<T>(this List<T> list)
		{
			if (list == null)
				throw new ArgumentNullException(nameof(list));

			var reverser = new ListReverser<T>(list);
			return reverser;
		}

		private static int LowerBoundSearchImpl<T>(IReadOnlyList<T> list, int index, int length, ref T value, IComparer<T> comparer)
		{
			if (comparer == null)
				comparer = Comparer<T>.Default;

			int endIndexExcl = index + length;
			while (length > 0)
			{
				int i = index + length / 2;
				int comparison = comparer.Compare(value, list[i]);
				if (comparison > 0)
				{
					index = i + 1;
				}
				else
				{
					endIndexExcl = i;
				}

				length = endIndexExcl - index;
			}

			return index;
		}

		/// <summary>
		/// Uses a simplified binary search to find the first occurrence of a given value within a sorted list.
		/// </summary>
		/// <typeparam name="T">The type of the list's items.</typeparam>
		/// <param name="list">The sorted list to search.</param>
		/// <param name="value">The value to search for within the list.</param>
		/// <param name="index">The index within the list to begin the search.</param>
		/// <param name="length">The numbers of items in the list starting from <paramref name="index" /> to search.</param>
		/// <param name="comparer">The comparer to used to search the list.</param>
		/// <returns>The index of the first occurrence of the value within the specified range of the list or if the range does not contain any occurrences of the value, then the postion within the list where such a value should be inserted to keep the range sorted.</returns>
		/// <exception cref="ArgumentNullException"></exception>
		/// <exception cref="ArgumentOutOfRangeException"></exception>
		/// <exception cref="ArgumentException"></exception>
		public static int LowerBoundSearch<T>(this IReadOnlyList<T> list, int index, int length, T value, IComparer<T> comparer = null)
		{
			if (list == null)
				throw new ArgumentNullException(nameof(list));

			if (index < 0)
				throw new ArgumentOutOfRangeException(nameof(index), ExceptionMessages.ArgumentMayNotBeNegative);

			if (length < 0)
				throw new ArgumentOutOfRangeException(nameof(length), ExceptionMessages.ArgumentMayNotBeNegative);

			if (index + length > list.Count)
				throw new ArgumentException($"{nameof(index)} and {nameof(length)} define a range that exceeds beyond the size of {nameof(list)}.");

			return LowerBoundSearchImpl(list, index, length, ref value, comparer);
		}

		/// <summary>
		/// Uses a simplified binary search to find the first occurrence of a given value within a sorted list.
		/// </summary>
		/// <typeparam name="T">The type of the list's items.</typeparam>
		/// <param name="list">The sorted list to search.</param>
		/// <param name="value">The value to search for within the list.</param>
		/// <param name="comparer">The comparer to used to search the list.</param>
		/// <returns>The index of the first occurrence of the value or if the list does not contain any occurrences of the value, then the postion within the list where such a value should be inserted to keep the list sorted.</returns>
		/// <exception cref="ArgumentNullException"></exception>
		public static int LowerBoundSearch<T>(this IReadOnlyList<T> list, T value, IComparer<T> comparer = null)
		{
			if (list == null)
				throw new ArgumentNullException(nameof(list));

			return LowerBoundSearchImpl(list, 0, list.Count, ref value, comparer);
		}

		/// <summary>
		/// 
		/// </summary>
		/// <typeparam name="T"></typeparam>
		/// <param name="list"></param>
		/// <returns></returns>
		/// <exception cref="ArgumentNullException" />
		/// <exception cref="InvalidOperationException" />
		public static T PeekLast<T>(this IList<T> list)
		{
			if (list == null)
				throw new ArgumentNullException(nameof(list));

			int index = list.Count - 1;
			if (index < 0)
				throw new InvalidOperationException(ExceptionMessages.CollectionIsEmpty);
			else
				return list[index];
		}

		/// <summary>
		/// 
		/// </summary>
		/// <typeparam name="T"></typeparam>
		/// <param name="list"></param>
		/// <returns></returns>
		/// <exception cref="ArgumentNullException" />
		/// <exception cref="InvalidOperationException" />
		/// <exception cref="NotSupportedException" />
		public static T PopLast<T>(this IList<T> list)
		{
			if (list == null)
				throw new ArgumentNullException(nameof(list));

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

		/// <summary>
		/// 
		/// </summary>
		/// <typeparam name="T"></typeparam>
		/// <param name="list"></param>
		/// <param name="result"></param>
		/// <returns></returns>
		/// <exception cref="ArgumentNullException" />
		public static bool TryPeekLast<T>(this IList<T> list, out T result)
		{
			if (list == null)
				throw new ArgumentNullException(nameof(list));

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

		/// <summary>
		/// 
		/// </summary>
		/// <typeparam name="T"></typeparam>
		/// <param name="list"></param>
		/// <param name="result"></param>
		/// <returns></returns>
		/// <exception cref="ArgumentNullException" />
		/// <exception cref="NotSupportedException" />
		public static bool TryPopLast<T>(this IList<T> list, out T result)
		{
			if (list == null)
				throw new ArgumentNullException(nameof(list));

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
	}
}
