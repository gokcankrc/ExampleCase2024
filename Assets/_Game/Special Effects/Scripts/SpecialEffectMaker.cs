using Game.Board;
using Game.Vfx;
using Gokcan.PoolSystem;
using System;
using System.Collections.Generic;
using UnityEngine;

namespace Game.SpecialEffect
{
	public static class SpecialEffectMaker
	{
		private static readonly Dictionary<(int, int), Action<ISequenceTarget>> EffectsTable =
			new()
			{
				{(1, 0) , Single.HorizontalRocket},
				{(2, 0) , Single.VerticalRocket},
				{(3, 0) , Single.Tnt},
				{(1, 1) , Merged.DoubleRocket},
				{(2, 1) , Merged.DoubleRocket},
				{(3, 1) , Merged.BigHorizontalRocket},
				{(1, 2) , Merged.DoubleRocket},
				{(2, 2) , Merged.DoubleRocket},
				{(3, 2) , Merged.BigVerticalRocket},
				{(1, 3) , Merged.BigHorizontalRocket},
				{(2, 3) , Merged.BigVerticalRocket},
				{(3, 3) , Merged.BigTnt},
			};

		private static EffectSettingBundle.SpecialSequenceSetting _tntSequenceSetting => EffectSettings.I.Effects.TntSequence;

		public static Action<ISequenceTarget> GetMaker(ItemType typeA, ItemType typeB = ItemType.None)
		{
			if (EffectsTable.TryGetValue(((int)typeA, (int)typeB), out var maker))
			{
				return maker;
			}
			else
			{
				Debug.LogError($"MakerTable failure, couldn't find appropriate maker from key {typeA}, {typeB}");
				return null;
			}
		}

		public enum ItemType
		{
			None = 0,
			HorizontalRocket = 1,
			VerticalRocket = 2,
			Tnt = 3,
		}

		private static void AnyTnt(ISequenceTarget target, int thickness)
		{
			var sequence = new SpecialEffectSequence(target, callback, _tntSequenceSetting);
			sequence.Execute();

			void callback()
			{
				var dep = new ExplosionEffect.Dependency(target.GridPos, thickness);
				ExplosionEffect newEffect = PoolableBehaviour.GetInstance<ExplosionEffect>();
				newEffect.Execute(dep);

				target.OnFinalizedSequence(true);
			}
		}

		public static class Single
		{
			public static void Tnt(ISequenceTarget target)
			{
				int thickness = SpecialEffectSettings.I.Settings.ExplosionSetting.SmallSizeThickness;
				AnyTnt(target, thickness);
			}

			public static void HorizontalRocket(ISequenceTarget target)
			{
				Rocket(target, RocketEffect.FireDir.horizontal);
			}

			public static void VerticalRocket(ISequenceTarget target)
			{
				Rocket(target, RocketEffect.FireDir.vertical);
			}

			public static void Rocket(ISequenceTarget target, RocketEffect.FireDir fireDir)
			{
				Rocket(target, new RocketEffect.Dependency(target.GridPos, fireDir));
			}

			public static void Rocket(ISequenceTarget target, RocketEffect.Dependency dep)
			{
				RocketEffect newEffect = PoolableBehaviour.GetInstance<RocketEffect>();
				newEffect.Execute(dep);

				target.OnFinalizedSequence(true);
			}
		}

		public static class Merged
		{
			public static void BigTnt(ISequenceTarget target)
			{
				int thickness = SpecialEffectSettings.I.Settings.ExplosionSetting.BigSizeThickness;
				AnyTnt(target, thickness);
			}

			public static void DoubleRocket(ISequenceTarget target)
			{
				var dep = new RocketEffect.Dependency(target.GridPos, RocketEffect.FireDir.horizontal);
				Single.Rocket(target, dep);
				dep.FireDir = RocketEffect.FireDir.vertical;
				Single.Rocket(target, dep);
			}

			public static void BigHorizontalRocket(ISequenceTarget target)
			{
				Vector2Int offset = Vector2Int.up;
				var dep = new RocketEffect.Dependency(target.GridPos, RocketEffect.FireDir.horizontal);
				BigRocket(target, dep, offset);
			}

			public static void BigVerticalRocket(ISequenceTarget target)
			{
				Vector2Int offset = Vector2Int.right;
				var dep = new RocketEffect.Dependency(target.GridPos, RocketEffect.FireDir.vertical);
				BigRocket(target, dep, offset);
			}

			private static void BigRocket(ISequenceTarget target, RocketEffect.Dependency dep, Vector2Int offset)
			{
				Single.Rocket(target, dep);
				dep.GridPos += offset;
				Single.Rocket(target, dep);
				dep.GridPos -= 2 * offset;
				Single.Rocket(target, dep);
			}
		}
	}
}