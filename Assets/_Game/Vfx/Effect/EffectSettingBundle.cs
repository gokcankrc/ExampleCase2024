using System;
using UnityEngine;

namespace Game.Vfx
{
	[CreateAssetMenu(fileName = "Effect Setting", menuName = "Board/Effect Setting")]
	public class EffectSettingBundle : ScriptableObject
	{
		public float ParticleExplodeTimer;
		public MergeSequenceSetting MergeSequence;
		public SpecialSequenceSetting TntSequence;

		[Serializable]
		public class MergeSequenceSetting
		{
			public float Duration = 0.3f;
			public float LerpSpeed = 0.1f;
		}

		[Serializable]
		public class SpecialSequenceSetting
		{
			public float Duration = 0.3f;
			public AnimationCurve Curve;

			public SpecialSequenceSetting(float duration, AnimationCurve curve)
			{
				Duration = duration;
				Curve = curve;
			}
		}
	}
}