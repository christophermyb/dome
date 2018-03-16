using System;
using System.Collections.Generic;

namespace Dome.Collections
{
	public interface IQueue<T> : IReadOnlyCollection<T>
	{
		void Enqueue(T item);
		bool TryPeek(out T result);
		bool TryDequeue(out T result);

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
		T Dequeue();
	}
}