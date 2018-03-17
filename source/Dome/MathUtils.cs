using System;

namespace Dome
{
	public static class MathUtils
	{
		/// <summary>
		/// Handy methods for numeric primitives.
		/// </summary>
		/// <param name="length"></param>
		/// <param name="value"></param>
		/// <returns></returns>
		/// <exception cref="ArgumentOutOfRangeException" />
		public static int Wrap(int length, int value)
		{
			if (length < 1)
				throw new ArgumentOutOfRangeException(nameof(length), ExceptionMessages.ArgumentMustBePositive);

			value %= length;
			value += length;
			value %= length;

			return value;
		}
	}
}