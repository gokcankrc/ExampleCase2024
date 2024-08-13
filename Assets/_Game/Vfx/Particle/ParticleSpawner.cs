using Gokcan.Helpers;
using Gokcan.PoolSystem;
using UnityEngine;

namespace Game.Vfx
{
	public class ParticleSpawner : LazySingleton<ParticleSpawner>
	{
		public static void SpawnParticle(ParticleEffect.Dependency dep, Vector3 pos)
		{
			var effect = PoolableBehaviour.GetInstance<ParticleEffect>();
			effect.Init(dep.Texture, pos);
		}
	}
}