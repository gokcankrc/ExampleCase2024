using Game.Board;
using System;
using UnityEngine;

namespace Game.SpecialEffect
{
	public class ExplosionEffect : SpecialEffectT<ExplosionEffect.Dependency>
	{
		public override void Execute(Dependency dep)
		{
			base.Execute(dep);

			BoardItemBlockProcessor.ExecuteTnt(dep.GridPos, dep.Thickness);
			GetDestroyed();
		}

		[Serializable]
		public class Setting
		{
			public int SmallSizeThickness;
			public int BigSizeThickness;
		}

		public new class Dependency : SpecialEffect.Dependency
		{
			public int Thickness;

			public Dependency(Vector2Int gridPos, int size) : base(gridPos)
			{
				Thickness = size;
			}
		}
	}
}