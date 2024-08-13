using System;
using UnityEngine.Pool;

namespace Gokcan.PoolSystem
{
	public class PoolHandler
	{
		private int _counter;
		public PoolableBehaviour Prefab;
		public ObjectPool<PoolableBehaviour> Pool;

		public PoolHandler(Dependency dep)
		{
			Prefab = dep.Prefab;
			Pool = GeneratePool(this, dep);
		}

		private static ObjectPool<PoolableBehaviour> GeneratePool(PoolHandler sender, Dependency dep)
		{
			var newPool = new ObjectPool<PoolableBehaviour>(
				createFunc: sender.InstantiatePoolable,
				actionOnGet: GetPoolable,
				actionOnRelease: ReturnPoolable,
				actionOnDestroy: OnDestroyPoolable,
				collectionCheck: false,
				defaultCapacity: dep.DefaultCapacity);
			return newPool;
		}

		protected virtual PoolableBehaviour InstantiatePoolable()
		{
			PoolableBehaviour newPoolable = UnityEngine.Object.Instantiate(Prefab);
			newPoolable.InitPoolable(this);
			newPoolable.name += _counter;
			_counter++;
			return newPoolable;
		}

		private static void GetPoolable(PoolableBehaviour poolable)
		{
			poolable.OnActivateFromPool();
		}

		private static void ReturnPoolable(PoolableBehaviour poolable)
		{
			poolable.OnDeactivateFromPool();
		}

		private static void OnDestroyPoolable(PoolableBehaviour pooledObject)
		{
			UnityEngine.Object.Destroy(pooledObject.gameObject);
		}

		[Serializable]
		public class Dependency
		{
			public PoolableBehaviour Prefab;
			public int DefaultCapacity;
		}
	}
}