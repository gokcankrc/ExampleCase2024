using Game.SpecialEffect;
using Game.Vfx;
using System.Collections.Generic;
using UnityEngine;

namespace Game.Board
{
	public abstract class SpecialItem<T> : ControllableBoardItem<T> where T : BoardItem.Dependency
	{
		protected abstract SpecialEffectMaker.ItemType _exactSpecialType { get; }

		[SerializeField] private SpecialType _specialType;

		protected override ComparisonComponent _comparisonComponentMaker => new SpecialComparison(this, _exactSpecialType);
		protected override List<Effect> _breakEffectsGet => new();

		public override void OnBroken()
		{
			if (!GettingDestroyed)
				TapComponent.Indirect();
			base.OnBroken();
		}

		public override void OnFinalizedSequence(bool isEndPoint)
		{
			if (isEndPoint)
			{
				foreach (var effect in _breakEffectsGet)
				{
					effect.Start(transform, _spriteRenderer);
					effect.Update(1);
				}
			}

			base.OnFinalizedSequence(isEndPoint);
		}

		public override bool IsColoredCube(out CubeItem cubeItem)
		{
			cubeItem = null;
			return false;
		}
	}

	public enum SpecialType
	{
		Rocket = 1,
		Tnt = 2,
	}
}