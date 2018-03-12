using System;
using System.Threading;

namespace Dome
{
	public static class Singleton<T> where T : class
	{
		private static T instance = null;

		/// <summary>
		/// 
		/// </summary>
		/// <param name="factoryMethod"></param>
		/// <returns></returns>
		/// <exception cref="MissingMethodException"
		/// <remarks>In the highly unlikely event that more than one thread tries to call this method for the first time simultaneously, it is possible for more than one instance of <typeparamref name="T" /> to be created, but the same instance will still be returned on each thread.</remarks>
		public static T GetLazilyInitializedInstance(Func<T> factoryMethod = null)
		{
			if (instance == null)
			{
				T instance;
				if (factoryMethod == null)
					instance = Activator.CreateInstance<T>();
				else
					instance = factoryMethod.Invoke();

				Interlocked.CompareExchange(ref Singleton<T>.instance, instance, null);
			}

			return instance;
		}
	}
}