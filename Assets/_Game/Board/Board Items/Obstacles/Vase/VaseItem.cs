using Game.Vfx;
using System;
using UnityEngine;

namespace Game.Board
{
	public class VaseItem : ObstacleItem<VaseItem.Dependency>
	{
		[SerializeField] private Visuals _setting;

		public override GoalType GetObstacleType => GoalType.Vase;

		protected override GravityComponent _gravityComponentMaker => new FallingGravity(this);
		protected override TakeDamageComponent _directDamageComponentMaker => new TakesDamage(this, _durability);
		protected override TakeDamageComponent _adjacentDamageComponentMaker => new TakesDamage(this, _durability);

		public override void OnActivateFromPool()
		{
			base.OnActivateFromPool();

			_spriteRenderer.sprite = _setting.DefaultSprite;
		}

		public override void OnDamageTaken()
		{
			base.OnDamageTaken();

			_spriteRenderer.sprite = _setting.CrackedSprite;

			_explodeIntoParticlesVfx.Start(transform, _spriteRenderer);
			_explodeIntoParticlesVfx.Update(1);
		}

		[Serializable]
		public class Visuals
		{
			public Sprite DefaultSprite;
			public Sprite CrackedSprite;
		}

		public new class Dependency : BoardItem.Dependency
		{
			public Sprite DefaultSprite;
			public Sprite CrackedSprite;

			public Dependency() : base(TypeId.Obstacle) { }
		}
	}
}