using Game.Vfx;
using System;
using System.Collections.Generic;
using UnityEngine;

namespace Game.Board
{
	public class MergeSequence : Sequence
	{
		public ISequenceTarget BlockOrigin;
		public List<ISequenceTarget> Block;
		public EffectSettingBundle.MergeSequenceSetting SequenceSettings;

		protected override float _maxGet => SequenceSettings.Duration;

		public MergeSequence(
			ISequenceTarget blockOrigin,
			List<ISequenceTarget> block,
			Action callback) : base()
		{
			BlockOrigin = blockOrigin;
			Block = block;
			Callback = callback;
			SequenceSettings = EffectSettings.I.Effects.MergeSequence;
		}

		public override void Execute()
		{
			base.Execute();

			foreach (ISequenceTarget target in Block)
			{
				// just a little push is enough to overcome ZFighting.
				target.SetPosNoZCorrect = target.Pos + Vector3.back;
				target.OnEnteredSequence();
			}
		}

		public override void Update(float deltaTime)
		{
			for (int i = 0; i < Block.Count; i++)
				Block[i].SetPosNoZCorrect = Vector3.Lerp(Block[i].Pos, BlockOrigin.Pos, SequenceSettings.LerpSpeed);

			base.Update(deltaTime);
		}

		public override void Finilize()
		{
			base.Finilize();

			// Block origin will be merged into, so it has other functionalities, so we don't touch.
			Block.Remove(BlockOrigin);
			foreach (ISequenceTarget target in Block)
				target.OnFinalizedSequence(false);
		}
	}
}