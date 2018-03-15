using System;
using System.Collections.Generic;

namespace Dome.Collections
{
	public interface IStack<T> : IReadOnlyCollection<T>
	{
		void Push(T item);
		T Peek();
		T Pop();
		bool TryPeek(out T result);
		bool TryPop(out T result);
	}
}