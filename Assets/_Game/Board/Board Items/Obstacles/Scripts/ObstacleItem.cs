using Game.Vfx;
using System.Collections.Generic;
using UnityEngine;

namespace Game.Board
{
	public abstract class ObstacleItem<T> : BoardItemT<T> where T : BoardItem.Dependency
	{
		[SerializeField] protected ItemDurability _durability;
		[SerializeField] protected ShrinkAndFade _shrinkAndFadeVfx;
		[SerializeField] protected ExplodeIntoParticles _explodeIntoParticlesVfx;

		public abstract GoalType GetObstacleType { get; }

		protected override ComparisonComponent _comparisonComponentMaker => new NullComparison(this);
		protected override TapComponent _tapComponentMaker => new NullTap(this);
		protected override List<Effect> _breakEffectsGet => new() { _shrinkAndFadeVfx, _explodeIntoParticlesVfx };

		protected GoalTracker _goalTracker;

		public GameObject GameObject => gameObject;

		protected override void ActivateInternal(T dep)
		{
			_durability.OnActivate(this);

			base.ActivateInternal(dep);
		}

		public override void MaybeAddToGoals(GoalTracker goalTracker)
		{
			base.MaybeAddToGoals(goalTracker);

			_goalTracker = goalTracker;

			if (goalTracker.Exists(GetObstacleType))
				goalTracker.Increase(GetObstacleType);
			else
				goalTracker.Register(new(GetObstacleType, _spriteRenderer.sprite));
		}

		public override void GetDestroyedInstant()
		{
			base.GetDestroyedInstant();

			_goalTracker.Reduce(GetObstacleType);
		}

		public override bool IsColoredCube(out CubeItem cubeItem)
		{
			cubeItem = null;
			return false;
		}
	}
}