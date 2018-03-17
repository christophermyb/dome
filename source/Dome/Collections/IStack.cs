using System;
using System.Collections.Generic;

namespace Dome.Collections
{
	public interface IStack<T> : IReadOnlyCollection<T>
	{
		void Clear();
		bool Contains(T item, IEqualityComparer<T> comparer = null);
		void Push(T item);
		bool TryPeek(out T result);
		bool TryPop(out T result);

		/// <summary>
		/// 
		/// </summary>
		/// <param name="array"></param>
		/// <param name="arrayIndex"></param>
		/// <exception cref="ArgumentNullException" />
		/// <exception cref="ArgumentOutOfRangeException" />
		/// <exception cref="ArgumentException" />
		void CopyTo(T[] array, int arrayIndex = 0);

		/// <summary>
		/// 
		/// </summary>
		/// <returns></returns>
		/// <exception cref="InvalidOperationException" />
		T Peek();

		/// <summary>
		/// 
		/// </summary>
		/// <returns></returns>
		/// <exception cref="InvalidOperationException" />
		T Pop();
	}
}