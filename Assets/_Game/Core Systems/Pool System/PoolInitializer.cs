using Gokcan.Helpers;
using System.Collections.Generic;

namespace Gokcan.PoolSystem
{
	public class PoolInitializer : SceneSingleton<PoolInitializer>
	{
		public List<PoolHandler.Dependency> PoolDependencies;
	}
}