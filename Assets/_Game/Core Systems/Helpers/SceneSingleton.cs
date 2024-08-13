using UnityEngine;

namespace Gokcan.Helpers
{
	/// <summary>
	/// Singleton for those scripts that already exist in the first Scene.
	/// </summary>
	[DisallowMultipleComponent]
	public abstract class SceneSingleton<T> : MonoBehaviour, ISingleton where T : SceneSingleton<T>
	{
		public static T I
		{
			get
			{
#if UNITY_EDITOR
				if (_i == null)
					TryToFind();
#endif
				if (_i == null)
					Debug.LogError($"SceneSingleton of type {typeof(T)} doesn't exist");
				return _i;
			}
			private set => _i = value;
		}
		private static T _i;

		protected virtual void Awake()
		{
			if (I != null && I != this)
			{
				Debug.LogWarning($"There is already an instance of type {typeof(T)}");
				Destroy(this);
				return;
			}

			I = this as T;
		}

#if UNITY_EDITOR
		private static void TryToFind()
		{
			T foundInstance = FindAnyObjectByType(typeof(T)) as T;
			if (foundInstance != null)
				_i = foundInstance;
		}
#endif
	}
}