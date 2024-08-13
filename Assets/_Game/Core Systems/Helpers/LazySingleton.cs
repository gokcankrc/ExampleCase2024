using UnityEngine;

namespace Gokcan.Helpers
{
	/// <summary>
	/// Lazily instantiated Singleton.
	/// </summary>
	/// <typeparam name="T"></typeparam>
	[DisallowMultipleComponent]
	public abstract class LazySingleton<T> : MonoBehaviour, ISingleton where T : LazySingleton<T>
	{
		public static T I
		{
			get
			{
				if (_i == null)
				{
					var name = typeof(T).ToString() + " (LazySingleton)";
					GameObject singletonObject = new GameObject(name);
					_i = singletonObject.AddComponent<T>();
					DontDestroyOnLoad(singletonObject);
				}
				return _i;
			}
			private set => _i = value;
		}
		private static T _i;

		protected virtual void Awake()
		{
			if (_i != null && _i != this)
			{
				Debug.LogWarning($"There is already an instance of type {typeof(T)}");
				Destroy(this);
				return;
			}

			I = this as T;
		}
	}
}