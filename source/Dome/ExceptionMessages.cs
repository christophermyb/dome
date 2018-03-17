using System;
using System.Collections;

namespace Dome
{
	/// <summary>
	/// Frequently occurring exception messages.
	/// </summary>
	public static class ExceptionMessages
	{
		public const string ArgumentMayNotBeNegative = "Argument may not be negative.";
		public const string ArgumentMustBePositive = "Argument must be positive.";
		public const string CollectionContentsHaveChanged = "The contents of the collection have changed.";
		public const string CollectionIsEmpty = "The collection is empty.";
		public const string CollectionIsReadOnly = "The collection is read-only.";

		public static readonly string ArgumentMustBeLessThanCount = $"Argument must be less than {nameof(ICollection.Count)}.";
		public static readonly string ArgumentMayNotBeLargerThanCount = $"Argument may not be larger than {nameof(ICollection.Count)}.";
		public static readonly string CapacityMayNotBeLessThanCount = $"{nameof(CollectionBase.Capacity)} may not be less than {nameof(ICollection.Count)}.";
	}
}
