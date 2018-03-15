using System;
using System.Collections.Generic;

namespace Dome.Collections
{
	public interface IQueue<T> : IReadOnlyCollection<T>
	{
		void Enqueue(T item);
		T Peek();
		T Dequeue();
		bool TryPeek(out T result);
		bool TryDequeue(out T result);
	}
}