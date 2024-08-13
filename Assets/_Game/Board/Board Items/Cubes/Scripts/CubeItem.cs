using Game.Vfx;
using Gokcan.Helpers;
using System;
using System.Collections.Generic;
using UnityEngine;

namespace Game.Board
{
	public class CubeItem : ControllableBoardItem<CubeItem.Dependency>
	{
		protected override ComparisonComponent _comparisonComponentMaker => new CubeTypeComparison(this, Type);
		protected override TapComponent _tapComponentMaker => new MergeColorsTap(this);

		protected override List<Effect> _breakEffectsGet =>
			new() { _shrinkAndFadeVfx, _explodeIntoParticlesVfx };

		[SerializeField] protected SpriteRenderer _oldTransitioninRenderer;
		[SerializeField] protected ShrinkAndFade _shrinkAndFadeVfx;
		[SerializeField] protected ExplodeIntoParticles _explodeIntoParticlesVfx;
		[SerializeField] protected Timer _swapTimer = new Timer(0.4f);

		public CubeType Type;
		private Dependency _dep;
		private List<BoardItem> _block;

		protected override void ActivateInternal(Dependency dep)
		{
			Type = dep.Type;
			_explodeIntoParticlesVfx.Particles[0] = dep.ParticleSprite;
			_spriteRenderer.sprite = dep.DefaultSprite;
			_oldTransitioninRenderer.sprite = dep.DefaultSprite;
			_oldTransitioninRenderer.color = new Color(1, 1, 1, 0);
			_dep = dep;

			base.ActivateInternal(dep);
		}

		protected void Update()
		{
			if (!_swapTimer.IsDrained)
			{
				_swapTimer.Update(Time.deltaTime);
				if (_swapTimer.JustDrained)
					_oldTransitioninRenderer.sprite = _spriteRenderer.sprite;

				var c = _oldTransitioninRenderer.color;
				c.a = Mathf.Lerp(1, 0, _swapTimer.Ratio0to1);
				_oldTransitioninRenderer.color = c;
			}
		}

		public override bool IsColoredCube(out CubeItem cubeItem)
		{
			cubeItem = this;
			return true;
		}

		public override void OnBlockRefresh(ItemBlockRule.SpecialMakerRule targetRule, List<BoardItem> block)
		{
			Sprite desriedSprite;
			if (targetRule == null)
			{
				desriedSprite = _dep.DefaultSprite;
			}
			else
			{
				switch (targetRule.ItemType)
				{
					case SpecialType.Rocket:
						desriedSprite = _dep.RocketSprite;
						break;
					case SpecialType.Tnt:
						desriedSprite = _dep.TntSprite;
						break;
					default:
						Debug.LogWarning("The target rule contained no ItemType.");
						desriedSprite = _dep.DefaultSprite;
						break;
				}
			}

			if (_spriteRenderer.sprite == desriedSprite)
				return;

			_oldTransitioninRenderer.color = new Color(1, 1, 1, 1);
			_oldTransitioninRenderer.sprite = _spriteRenderer.sprite;

			_spriteRenderer.sprite = desriedSprite;

			_swapTimer.SetToMax();
		}

		[Serializable]
		public class Setting
		{
			public CubeType Type;
			public Sprite DefaultSprite;
			public Sprite RocketSprite;
			public Sprite TntSprite;
			public Texture ParticleSprite;
		}

		public new class Dependency : BoardItem.Dependency
		{
			public CubeType Type;
			public Sprite DefaultSprite;
			public Sprite RocketSprite;
			public Sprite TntSprite;
			public Texture ParticleSprite;

			public Dependency(Setting setting) : base(TypeId.Cube)
			{
				DefaultSprite = setting.DefaultSprite;
				RocketSprite = setting.RocketSprite;
				TntSprite = setting.TntSprite;
				ParticleSprite = setting.ParticleSprite;
				Type = setting.Type;
			}
		}
	}

	public enum CubeType
	{
		Blue = 1,
		Green = 2,
		Yellow = 3,
		Red = 4,
	}
}