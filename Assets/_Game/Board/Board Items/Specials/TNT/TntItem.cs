using Game.SpecialEffect;
using Game.Vfx;
using System.Collections.Generic;
using UnityEngine;

namespace Game.Board
{
	public class TntItem : SpecialItem<TntItem.Dependency>
	{
		protected override TapComponent _tapComponentMaker => new ExplodeTntTap(this);
		protected override SpecialEffectMaker.ItemType _exactSpecialType => SpecialEffectMaker.ItemType.Tnt;
		protected override List<Effect> _breakEffectsGet => new() { _explodeFx };

		[SerializeField] protected BloatAndParticles _explodeFx;

		public new class Dependency : BoardItem.Dependency
		{
			public Dependency() : base(TypeId.Special) { }
		}
	}
}