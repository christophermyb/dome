using System;
using System.Collections.Generic;

namespace Dome
{
	public interface IStack<T> : IEnumerable<T>
	{
		T Peek();
		T Pop();
		bool TryPeek(out T result);
		bool TryPop(out T result);
	}
}