using UnityEngine;

namespace Gokcan.PoolSystem
{
	public abstract class PoolableBehaviour : MonoBehaviour
	{
		public virtual bool IsActive => _isActive;
		protected bool _isActive;
		protected abstract Transform DefaultParent { get; }
		protected PoolHandler _poolHandler;
		private static readonly Vector3 NullPosition = new(-25, 0, 0);

		public static T GetInstance<T>() where T : PoolableBehaviour
		{
			if (!PoolManager.AllPools.TryGetValue(typeof(T), out PoolHandler poolHandler))
				Debug.LogError($"No PoolHandler found for {typeof(T)}");

			return poolHandler.Pool.Get() as T;
		}

		public void InitPoolable(PoolHandler poolHandler)
		{
			_poolHandler = poolHandler;
			transform.parent = DefaultParent;
		}

		public static void ReturnPoolable(PoolableBehaviour poolable)
		{
			if (poolable._isActive)
				poolable._poolHandler.Pool.Release(poolable);
		}

		/// <summary>
		/// Called at the moment this is pulled from the pool
		/// </summary>
		public virtual void OnActivateFromPool()
		{
			_isActive = true;
		}

		/// <summary>
		/// Called at the moment this is plled into the pool
		/// </summary>
		public virtual void OnDeactivateFromPool()
		{
			_isActive = false;
			transform.position = NullPosition;
		}

		public static void OnDestroyPoolable(PoolableBehaviour pooledObject)
		{
			Destroy(pooledObject.gameObject);
		}
	}
}