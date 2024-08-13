using Game.SpecialEffect;
using System;
using System.Collections.Generic;
using static Game.SpecialEffect.SpecialEffectMaker;

namespace Game.Board
{
	public abstract class SpecialTapComponent : TapComponent
	{
		public override ItemType GetExactSpecialType => _exactSpecialType;

		protected ItemType _exactSpecialType;

		protected SpecialTapComponent(BoardItem connectedItem) : base(connectedItem) { }

		protected override bool OnTap()
		{
			OnSpecialTap(GetExactSpecialType);
			return true;
		}

		protected override void OnIndirect()
		{
			OnSpecialIndirect(GetExactSpecialType);
		}

		protected void OnSpecialTap(ItemType type)
		{
			List<BoardItem> neighborSpecials = ComparisonComponent.FindNeighborSpecials(_connectedItem);
			if (neighborSpecials.Count > 0)
			{
				BoardItem chosenSpecial = neighborSpecials[UnityEngine.Random.Range(0, neighborSpecials.Count - 1)];
				ItemType neighborType = chosenSpecial.TapComponent.GetExactSpecialType;

				MergedArgs args = new();
				args.target = _connectedItem;
				args.targetType = type;
				args.neighbor = chosenSpecial;
				args.neighborType = neighborType;
				MergedEffect(args);
			}
			else
			{
				SingleEffect(_connectedItem, type);
			}
		}

		protected void OnSpecialIndirect(ItemType type)
		{
			SingleEffect(_connectedItem, type);
		}

		private static void MergedEffect(MergedArgs args)
		{
			List<ISequenceTarget> block = new() { args.target, args.neighbor };

			var sequence = new MergeSequence(args.target, block, callback);
			sequence.Execute();

			void callback()
			{
				var maker = SpecialEffectMaker.GetMaker(args.targetType, args.neighborType);
				maker.Invoke(args.target);
			}
		}

		private struct MergedArgs
		{
			public ISequenceTarget target;
			public ItemType targetType;
			public ISequenceTarget neighbor;
			public ItemType neighborType;
		}

		private static void SingleEffect(ISequenceTarget target, ItemType typeA)
		{
			Action action = GetMakerAction(target, typeA, ItemType.None);
			action?.Invoke();
		}

		private static Action GetMakerAction(ISequenceTarget target, ItemType typeA, ItemType typeB)
		{
			var maker = SpecialEffectMaker.GetMaker(typeA, typeB);
			return () => { maker.Invoke(target); };
		}
	}
}