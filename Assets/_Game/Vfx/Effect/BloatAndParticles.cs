using Gokcan.Helpers;
using System;
using UnityEngine;

namespace Game.Vfx
{
	[Serializable]
	public class BloatAndParticles : Effect
	{
		public GameObject ParticleObject;

		protected float _triggerRatio;
		protected bool _triggered;
		protected Vector3 _pos;

		public override void Start(Transform tr, SpriteRenderer renderer)
		{
			base.Start(tr, renderer);
			_pos = tr.position;
			_triggerRatio = EffectSettings.I.Effects.ParticleExplodeTimer;
			_triggered = false;
		}

		public override void Update(float ratio)
		{
			ratio = Mathf.Clamp01(ratio);
			_transform.localScale = Vector3.one * ratio;
			Color c = _renderer.color;
			float fadeStart = 0.6f;
			c.a = ratio.MapUnclamped(0, fadeStart, 0, 1);
			_renderer.color = c;

			if (_triggered) return;

			if (ratio > _triggerRatio)
			{
				_triggered = true;
				var fx = UnityEngine.Object.Instantiate(ParticleObject);
				fx.transform.position = _pos;
			}
		}
	}
}