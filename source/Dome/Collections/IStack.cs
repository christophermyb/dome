using System;
using System.Collections.Generic;

namespace Dome.Collections
{
	public interface IStack<T> : IReadOnlyCollection<T>
	{
		void Push(T item);
		bool TryPeek(out T result);
		bool TryPop(out T result);

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