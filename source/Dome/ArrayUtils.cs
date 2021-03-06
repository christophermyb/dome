﻿using System;
using System.Collections.Generic;
using System.Threading;

namespace Dome
{
	/// <summary>
	/// Handy methods for working with arrays.
	/// </summary>
	public static class ArrayUtils
	{
		private static class EmptyArray<T>
		{
			private static volatile T[] value = null;

			public static T[] Value
			{
				get
				{
					Thread.MemoryBarrier();
					if (value == null)
					{
						var array = new T[0];
						Interlocked.CompareExchange(ref value, array, null);
					}

					return value;
				}
			}
		}

		/// <summary>
		/// Returns a lazily initialized cached instance of an empty array of type <typeparamref name="T"/>.
		/// </summary>
		/// <typeparam name="T">The element type of the array.</typeparam>
		/// <returns>The empty array.</returns>
		public static T[] GetEmpty<T>() => EmptyArray<T>.Value;

		/// <summary>
		/// Efficiently inserts a value into an array while resizing it.
		/// </summary>
		/// <typeparam name="T">The type of the array element.</typeparam>
		/// <param name="array">The array.</param>
		/// <param name="size">The new length to resize the array too.</param>
		/// <param name="index">The index in the array at which to insert the value.</param>
		/// <param name="value">The value to insert into the array.</param>
		/// <exception cref="ArgumentNullException"></exception>
		/// <exception cref="ArgumentOutOfRangeException"></exception>
		/// <exception cref="ArgumentException"></exception>
		public static void ResizeAndInsert<T>(ref T[] array, int size, int index, T value)
		{
			if (array == null)
				throw new ArgumentNullException(nameof(array));

			if (size <= array.Length)
				throw new ArgumentException($"{nameof(size)} must be larger than the length of {nameof(array)} by at least 1.");

			if (index < 0)
				throw new ArgumentOutOfRangeException(nameof(index), index, ExceptionMessages.ArgumentMayNotBeNegative);

			if (index > array.Length)
				throw new ArgumentException($"{nameof(index)} may not be larger than the length of {nameof(array)}.");

			var newArray = new T[size];
			Array.Copy(array, newArray, index);
			Array.Copy(array, index, newArray, index + 1, array.Length - index);
			newArray[index] = value;

			array = newArray;
		}

		/// <summary>
		/// Inserts a value into an array.
		/// </summary>
		/// <typeparam name="T">The type of the array element and the value to insert.</typeparam>
		/// <param name="array">The array.</param>
		/// <param name="count">The number of elements in the array that are in use.</param>
		/// <param name="index">The index in the array at which to insert the value.</param>
		/// <param name="value">The value to insert into the array.</param>
		/// <exception cref="ArgumentNullException"></exception>
		/// <exception cref="ArgumentOutOfRangeException"></exception>
		/// <exception cref="ArgumentException"></exception>
		public static void Insert<T>(this T[] array, int count, int index, T value)
		{
			if (array == null)
				throw new ArgumentNullException(nameof(array));

			if (count < 0)
				throw new ArgumentOutOfRangeException(nameof(count), count, ExceptionMessages.ArgumentMayNotBeNegative);

			if (count >= array.Length)
				throw new ArgumentException($"{nameof(count)} must be smaller than the length of {nameof(array)}.");

			if (index < 0)
				throw new ArgumentOutOfRangeException(nameof(index), index, ExceptionMessages.ArgumentMayNotBeNegative);

			if (index > count)
				throw new ArgumentException($"{nameof(index)} may not be larger than {nameof(count)}.");

			Array.Copy(array, index, array, index + 1, count - index);
			array[index] = value;
		}

		private static int LastIndexOfImpl<T>(T[] array, ref T value, int index, int length, IEqualityComparer<T> comparer)
		{
			if (comparer == null)
				comparer = EqualityComparer<T>.Default;

			for (int i = index + length - 1; i >= index; --i)
			{
				T item = array[i];
				if (comparer.Equals(value, item))
					return i;
			}

			return -1;
		}

		/// <summary>
		/// 
		/// </summary>
		/// <typeparam name="T"></typeparam>
		/// <param name="array"></param>
		/// <param name="value"></param>
		/// <param name="index"></param>
		/// <param name="length"></param>
		/// <param name="comparer"></param>
		/// <returns></returns>
		/// <exception cref="ArgumentNullException" />
		/// <exception cref="ArgumentOutOfRangeException" />
		/// <exception cref="ArgumentException" />
		public static int LastIndexOf<T>(this T[] array, T value, int index, int length, IEqualityComparer<T> comparer = null)
		{
			if (array == null)
				throw new ArgumentNullException(nameof(array));

			if (index < 0)
				throw new ArgumentOutOfRangeException(nameof(index), index, ExceptionMessages.ArgumentMayNotBeNegative);

			if (length < 0)
				throw new ArgumentOutOfRangeException(nameof(length), length, ExceptionMessages.ArgumentMayNotBeNegative);

			if (index + length > array.Length)
				throw new ArgumentException($"{nameof(index)} and {nameof(length)} do not define a valid region of the array.");

			return LastIndexOfImpl(array, ref value, index, length, comparer);
		}

		/// <summary>
		/// 
		/// </summary>
		/// <typeparam name="T"></typeparam>
		/// <param name="array"></param>
		/// <param name="value"></param>
		/// <param name="index"></param>
		/// <param name="comparer"></param>
		/// <returns></returns>
		/// <exception cref="ArgumentNullException" />
		/// <exception cref="ArgumentOutOfRangeException" />
		public static int LastIndexOf<T>(this T[] array, T value, int index = 0, IEqualityComparer<T> comparer = null)
		{
			if (array == null)
				throw new ArgumentNullException(nameof(array));

			if (index < 0)
				throw new ArgumentOutOfRangeException(nameof(index), index, ExceptionMessages.ArgumentMayNotBeNegative);

			int length = array.Length - index;
			return LastIndexOfImpl(array, ref value, index, length, comparer);
		}

		/// <summary>
		/// 
		/// </summary>
		/// <typeparam name="T"></typeparam>
		/// <param name="array"></param>
		/// <param name="count"></param>
		/// <param name="newSize"></param>
		/// <exception cref="ArgumentOutOfRangeException" />
		/// <exception cref="ArgumentException" />
		public static void Resize<T>(ref T[] array, int count, int newSize)
		{
			if (count < 0)
				throw new ArgumentOutOfRangeException(nameof(count), count, ExceptionMessages.ArgumentMayNotBeNegative);

			if (newSize < count)
				throw new ArgumentException($"{nameof(newSize)} ({newSize}) cannot be smaller than {nameof(count)} ({count}).");

			var newArray = new T[newSize];
			Array.Copy(array, newArray, count);
			array = newArray;
		}
	}
}
