using System;
using System.Collections.Generic;
using System.Threading;

namespace Dome.Collections
{
	/// <summary>
	/// Compares objects and orders them in the reverse order to which they are sorted by default.
	/// </summary>
	/// <typeparam name="T">The type of the objects to sort. The type must implement <see cref="IComparable{T}" /></typeparam>
	public class DescendingComparer<T> : IComparer<T> where T : IComparable<T>
	{
		private static volatile DescendingComparer<T> @default = null;

		public static DescendingComparer<T> Default
		{
			get
			{
				Thread.MemoryBarrier();
				if (@default == null)
				{
					var @default = new DescendingComparer<T>();
					Interlocked.CompareExchange(ref DescendingComparer<T>.@default, @default, null);
				}

				return @default;
			}
		}

		/// <summary>
		/// 
		/// </summary>
		/// <param name="x"></param>
		/// <param name="y"></param>
		/// <returns></returns>
		/// <exception cref="NullReferenceException">If y is null.</exception>
		public int Compare(T x, T y) => y.CompareTo(x);
	}
}
