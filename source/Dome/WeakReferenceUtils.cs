using System;

namespace Dome
{
	/// <summary>
	/// Provides handy methods for working with <see cref="WeakReference{T}" />.
	/// </summary>
	public static class WeakReferenceUtils
	{
		/// <summary>
		/// Retrieves the target from a <see cref="WeakReference{T}" /> if it has one, or creates an instance of <typeparamref name="T"/> and sets that as the target before returning the instance;
		/// </summary>
		/// <typeparam name="T">The type of the target.</typeparam>
		/// <param name="reference"></param>
		/// <param name="factoryMethod">The delegate to invoke to create a new target if neccessary. If this is null, then a target is instantiated via <typeparamref name="T" />'s public default constructor.</param>
		/// <exception cref="MissingMethodException" />
		public static T GetLazilyInitializedTarget<T>(this WeakReference<T> reference, Func<T> factoryMethod = null) where T : class
		{
			if (!reference.TryGetTarget(out T target))
			{
				if (factoryMethod == null)
					target = Activator.CreateInstance<T>();
				else
					target = factoryMethod.Invoke();

				reference.SetTarget(target);
			}

			return target;
		}
	}
}
