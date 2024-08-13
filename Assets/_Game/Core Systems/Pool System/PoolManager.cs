using Gokcan.Helpers;
using System;
using System.Collections.Generic;

namespace Gokcan.PoolSystem
{
	public class PoolManager : LazySingleton<PoolManager>
	{
		public static Dictionary<Type, PoolHandler> AllPools;

		public void Init(Dependency dep)
		{
			InitialzePools(dep.PoolInitializer);
		}

		public void InitialzePools(PoolInitializer poolInitializer)
		{
			foreach (var poolDep in poolInitializer.PoolDependencies)
			{
				PoolHandler newPool = new(poolDep);
				AllPools.Add(poolDep.Prefab.GetType(), newPool);
			}
		}

		public class Dependency
		{
			public PoolInitializer PoolInitializer;

			public Dependency(PoolInitializer poolInitializer)
			{
				PoolInitializer = poolInitializer;
			}
		}
	}
}