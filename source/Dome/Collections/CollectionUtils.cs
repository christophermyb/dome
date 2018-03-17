using System;
using System.Collections.Generic;

namespace Dome.Collections
{
	/// <summary>
	/// Handy methods for working with collections.
	/// </summary>
	public static class CollectionUtils
	{
		/// <summary>
		/// 
		/// </summary>
		/// <param name="array"></param>
		/// <param name="arrayIndex"></param>
		/// <param name="count"></param>
		/// <param name="arrayIndexParameterName"></param>
		/// <exception cref="ArgumentNullException" />
		/// <exception cref="ArgumentOutOfRangeException" />
		/// <exception cref="ArgumentException" />
		public static void CheckCopyToArguments(Array array, int arrayIndex, int count, string arrayIndexParameterName)
		{
			if (array == null)
				throw new ArgumentNullException(nameof(array));

			if (arrayIndex < 0)
				throw new ArgumentOutOfRangeException(arrayIndexParameterName, arrayIndex, ExceptionMessages.ArgumentMayNotBeNegative);

			if (arrayIndex + count > array.Length)
				throw new ArgumentException("The array is not large enough to store all of the collection's items starting at the given index.");
		}

		/// <summary>
		/// 
		/// </summary>
		/// <param name="array"></param>
		/// <param name="arrayIndex"></param>
		/// <param name="count"></param>
		/// <exception cref="ArgumentNullException" />
		/// <exception cref="ArgumentOutOfRangeException" />
		/// <exception cref="ArgumentException" />
		public static void CheckCopyToArguments(Array array, int arrayIndex, int count)
		{
			CheckCopyToArguments(array, arrayIndex, count, nameof(arrayIndex));
		}
	}
}