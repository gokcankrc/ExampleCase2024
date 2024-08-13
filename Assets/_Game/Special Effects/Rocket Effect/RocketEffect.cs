using Gokcan.PoolSystem;
using System;
using UnityEngine;

namespace Game.SpecialEffect
{
	public class RocketEffect : SpecialEffectT<RocketEffect.Dependency>
	{
		private Setting _setting;

		private void Awake()
		{
			_setting = SpecialEffectSettings.I.Settings.RocketSetting;
		}

		public override void Execute(Dependency dep)
		{
			base.Execute(dep);

			Vector3 fireVector;
			Sprite spriteA;
			Sprite spriteB;
			switch (dep.FireDir)
			{
				case FireDir.horizontal:
					fireVector = Vector3.right;
					spriteA = _setting.SpriteRight;
					spriteB = _setting.SpriteLeft;
					break;
				case FireDir.vertical:
					fireVector = Vector3.up;
					spriteA = _setting.SpriteUp;
					spriteB = _setting.SpriteDown;
					break;
				default:
					Debug.LogError("Fire direction is null", gameObject);
					return;
			}

			var rocketA = PoolableBehaviour.GetInstance<RocketProjectile>();
			var rocketB = PoolableBehaviour.GetInstance<RocketProjectile>();
			rocketA.FireOff(this, dep.Pos, fireVector, spriteA);
			rocketB.FireOff(this, dep.Pos, -fireVector, spriteB);
		}

		public enum FireDir
		{
			horizontal,
			vertical
		}

		[Serializable]
		public class Setting
		{
			public float Speed;
			public Sprite SpriteRight;
			public Sprite SpriteLeft;
			public Sprite SpriteUp;
			public Sprite SpriteDown;
		}

		public new class Dependency : SpecialEffect.Dependency
		{
			public FireDir FireDir;

			public Dependency(Vector2Int gridPos, FireDir fireDir) : base(gridPos)
			{
				FireDir = fireDir;
			}
		}
	}
}