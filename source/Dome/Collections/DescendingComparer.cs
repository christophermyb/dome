using System;
using System.Collections.Generic;

namespace Dome.Collections
{
	/// <summary>
	/// Compares objects and orders them in the reverse order to which they are sorted by default.
	/// </summary>
	/// <typeparam name="T">The type of the objects to sort. The type must implement <see cref="IComparable{T}" /></typeparam>
	/// <exception cref="NullReferenceException">If y is null.</exception>
	public class DescendingComparer<T> : IComparer<T> where T : IComparable<T>
	{
		public int Compare(T x, T y)
		{
			return y.CompareTo(x);
		}
	}
}
