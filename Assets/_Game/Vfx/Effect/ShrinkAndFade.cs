using Gokcan.Helpers;
using System;
using UnityEngine;

namespace Game.Vfx
{
	[Serializable]
	public class ShrinkAndFade : Effect
	{
		public override void Update(float ratio)
		{
			ratio = Mathf.Clamp01(ratio);
			_transform.localScale = Vector3.one * ratio;
			Color c = _renderer.color;
			float fadeStart = 0.6f;
			c.a = ratio.MapUnclamped(0, fadeStart, 0, 1);
			_renderer.color = c;
		}
	}
}