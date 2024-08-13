using System;
using System.Collections.Generic;
using UnityEngine;

namespace Game.Vfx
{
	[Serializable]
	public class ExplodeIntoParticles : Effect
	{
		public List<Texture> Particles;

		protected float _triggerRatio;
		protected bool _triggered;

		public override void Start(Transform tr, SpriteRenderer renderer)
		{
			base.Start(tr, renderer);
			_triggerRatio = EffectSettings.I.Effects.ParticleExplodeTimer;
			_triggered = false;
		}

		public override void Update(float ratio)
		{
			if (_triggered) return;

			if (ratio > _triggerRatio)
			{
				_triggered = true;
				foreach (Texture particle in Particles)
					ParticleSpawner.SpawnParticle(new(particle), _transform.position);
			}
		}
	}
}