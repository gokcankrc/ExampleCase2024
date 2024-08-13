using Game.SpecialEffect;
using System;
using UnityEngine;
using static Game.SpecialEffect.RocketEffect;

namespace Game.Board
{
	public class RocketItem : SpecialItem<RocketItem.Dependency>
	{
		protected override TapComponent _tapComponentMaker => new SpawnRocketsTap(this, _fireDir);

		protected override SpecialEffectMaker.ItemType _exactSpecialType => _fireDir switch
		{
			FireDir.horizontal => SpecialEffectMaker.ItemType.HorizontalRocket,
			FireDir.vertical => SpecialEffectMaker.ItemType.VerticalRocket,
			_ => throw new NotImplementedException(),
		};

		protected FireDir _fireDir;

		protected override void ActivateInternal(Dependency dep)
		{
			_fireDir = dep.FireDir;
			_spriteRenderer.sprite = BoardSettings.I.BoardItems.RocketSetting.GetSprite(_fireDir);

			base.ActivateInternal(dep);
		}

		public override void GetDestroyed()
		{
			_spriteRenderer.sprite = null;

			base.GetDestroyed();
		}

		[Serializable]
		public class Setting
		{
			public Sprite Horizontal;
			public Sprite Vertical;

			public Sprite GetSprite(FireDir fireDir)
			{
				return fireDir switch
				{
					FireDir.horizontal => Horizontal,
					FireDir.vertical => Vertical,
					_ => null,
				};
			}
		}

		public new class Dependency : BoardItem.Dependency
		{
			public FireDir FireDir;

			public Dependency(FireDir fireDir) : base(TypeId.Special)
			{
				FireDir = fireDir;
			}
		}
	}
}