using Game.Vfx;
using System;
using UnityEngine;

namespace Game.Board
{
	public class SpecialEffectSequence : Sequence
	{
		public ISequenceTarget Target;
		public EffectSettingBundle.SpecialSequenceSetting SequenceSettings;

		protected override float _maxGet => SequenceSettings.Duration;

		public SpecialEffectSequence(
			ISequenceTarget target,
			Action callback,
			EffectSettingBundle.SpecialSequenceSetting settings) : base()
		{
			Target = target;
			Callback = callback;
			SequenceSettings = settings;
		}

		public override void Execute()
		{
			base.Execute();

			// just a little push is enough to overcome ZFighting.
			Target.SetPosNoZCorrect = Target.Pos + Vector3.back;
			Target.OnEnteredSequence();
		}

		public override void Update(float deltaTime)
		{
			float scale = SequenceSettings.Curve.Evaluate(1 - _timer.Ratio1to0);
			Target.Transform.localScale = Vector3.one * scale;

			base.Update(deltaTime);
		}
	}
}