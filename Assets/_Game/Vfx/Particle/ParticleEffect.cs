using Gokcan.PoolSystem;
using System;
using UnityEngine;

namespace Game.Vfx
{
	public class ParticleEffect : PoolableBehaviour
	{
		protected override Transform DefaultParent => ParticleEffectParent.Tr;

		[SerializeField] protected ParticleSystem _particleSystem;
		[SerializeField] protected Renderer _renderer;
		protected MaterialPropertyBlock mpb;

		private void Awake()
		{
			mpb = new();
			var main = _particleSystem.main;
			main.stopAction = ParticleSystemStopAction.Callback;
		}

		public void Init(Texture texture, Vector3 pos)
		{
			mpb.SetTexture("_MainTex", texture);
			_renderer.SetPropertyBlock(mpb);
			transform.position = new Vector3(pos.x, pos.y, transform.position.z);
		}

		public override void OnActivateFromPool()
		{
			base.OnActivateFromPool();

			_particleSystem.Play();
		}

		private void OnParticleSystemStopped()
		{
			ReturnPoolable(this);
		}

		[Serializable]
		public class Dependency
		{
			public Texture Texture;

			public Dependency(Texture texture)
			{
				Texture = texture;
			}
		}
	}
}