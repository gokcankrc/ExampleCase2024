using Game.Core;
using Game.Vfx;
using Gokcan.Helpers;
using Gokcan.PoolSystem;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.EventSystems;

namespace Game.Board
{
	public abstract class BoardItem : PoolableBehaviour, IPointerClickHandler, IBreakableItem, ISequenceTarget
	{
		protected abstract ComparisonComponent _comparisonComponentMaker { get; }
		protected abstract GravityComponent _gravityComponentMaker { get; }
		protected abstract TapComponent _tapComponentMaker { get; }
		protected abstract TakeDamageComponent _directDamageComponentMaker { get; }
		protected abstract TakeDamageComponent _adjacentDamageComponentMaker { get; }
		public abstract bool IsColoredCube(out CubeItem cubeItem);
		protected abstract List<Effect> _breakEffectsGet { get; }

		protected override Transform DefaultParent => BoardItemsParent.Tr;

		[SerializeField] protected SpriteRenderer _spriteRenderer;
		[SerializeField] protected Timer _destroyingTimer = new(0.2f);

		public IEnumerable<BoardItem> Neighbors => ConnectedSlot.Neighbors.Select(x => x.ConnectedItem);
		public bool Resting => GravityComponent.FallState == GravityComponent.State.Stopped;
		public Transform Transform => transform;

		public Vector2Int GridPosSafe => ConnectedSlot != null ? ConnectedSlot.GridPos : new(-10, -10);
		public Vector2Int GridPos => ConnectedSlot.GridPos;
		public Vector3 SetPosNoZCorrect { set => transform.position = value; }
		public Vector3 Pos
		{
			get => transform.position;
			set
			{
				transform.position = value;
				RefreshZLevel();
			}
		}

		public static bool TapsDisabled = false;

		[NonSerialized] public ComparisonComponent ComparisonComponent;
		[NonSerialized] public GravityComponent GravityComponent;
		[NonSerialized] public TapComponent TapComponent;
		[NonSerialized] public TakeDamageComponent DirectDamageComponent;
		[NonSerialized] public TakeDamageComponent AdjacentDamageComponent;
		[NonSerialized] public BoardSlotBase ConnectedSlot;
		[NonSerialized] public TypeId Id;
		[NonSerialized] public bool GettingDestroyed;
		[NonSerialized] public bool Initialized;

		protected IEnumerable<BoardItemComponent> _allComponents;

		protected void InitBase(Dependency dep)
		{
			if (Initialized)
				return;

			Id = dep.Id;
			BoardManager.I.NewItem(this);

			Initialized = true;
		}

		protected void ActivateBase(Dependency dep)
		{
			GettingDestroyed = false;

			transform.localScale = Vector3.one;

			Color c = _spriteRenderer.color;
			c.a = 1;
			_spriteRenderer.color = c;

			RefreshZLevel();

			initNewComponents();

			void initNewComponents()
			{
				ComparisonComponent = _comparisonComponentMaker;
				GravityComponent = _gravityComponentMaker;
				TapComponent = _tapComponentMaker;
				DirectDamageComponent = _directDamageComponentMaker;
				AdjacentDamageComponent = _adjacentDamageComponentMaker;

				var components = new BoardItemComponent[5];
				components[0] = ComparisonComponent;
				components[1] = GravityComponent;
				components[2] = TapComponent;
				components[3] = DirectDamageComponent;
				components[4] = AdjacentDamageComponent;
				_allComponents = components;

				GravityComponent.Moved += OnMove;
				GravityComponent.Stopped += OnStop;

				foreach (var component in _allComponents)
					component.OnActivate();
			}
		}

		public override void OnActivateFromPool()
		{
			base.OnActivateFromPool();
		}

		public override void OnDeactivateFromPool()
		{
			base.OnDeactivateFromPool();

			if (ConnectedSlot != null)
			{
				ConnectedSlot.OnItemDestroyed();
				ConnectedSlot = null;
			}
			GettingDestroyed = false;

			foreach (var component in _allComponents)
				component.OnDeactivate();
		}

		public virtual void GetDestroyedInstant()
		{
			ReturnPoolable(this);
		}

		// todo: make here a Sequence instead
		public virtual void GetDestroyed()
		{
			if (GettingDestroyed) return;
			GettingDestroyed = true;
			LevelManager.ActiveActionCounter += 1;

			StartCoroutine(destroyingCoroutine());

			IEnumerator destroyingCoroutine()
			{
				_destroyingTimer.SetToMax();
				foreach (Effect effect in _breakEffectsGet)
					effect.Start(transform, _spriteRenderer);

				while (!_destroyingTimer.IsDrained)
				{
					_destroyingTimer.Update(Time.deltaTime);
					foreach (Effect effect in _breakEffectsGet)
						effect.Update(_destroyingTimer.Ratio1to0);

					yield return null;
				}

				LevelManager.ActiveActionCounter -= 1;
				GetDestroyedInstant();
			}
		}

		private void FixedUpdate()
		{
			GravityComponent.Update(Time.fixedDeltaTime);
		}

		public virtual void OnEnteredSequence()
		{
			GettingDestroyed = true;
		}

		public virtual void OnFinalizedSequence(bool isEndPoint)
		{
			GetDestroyedInstant();
		}

		public virtual void OnDamageTaken() { }

		public virtual void OnBroken()
		{
			GetDestroyed();
		}

		#region Movement and Position
		protected virtual void OnMove()
		{
			RefreshZLevel();
		}

		protected virtual void OnStop()
		{
			RefreshZLevel();
		}

		private void RefreshZLevel()
		{
			Vector3 pos = transform.position;
			pos.z = (-pos.y * 0.03f) + (pos.x * 0.0006f);
			transform.position = pos;
		}
		#endregion

		#region Setting slot connections
		protected void SetRestingSlot(BoardSlotBase slot)
		{
			ConnectedSlot = slot;
		}

		protected void SetFallingSlot(BoardSlotBase slot)
		{
			if (GravityComponent.IsStatic)
				Debug.LogWarning("Tried to set falling newItem static newItem.");

			ConnectedSlot = slot;
		}

		public static void RefreshCouple(BoardItem item)
		{
			SetCouple(item, item.ConnectedSlot);
		}

		public static void SetCouple(BoardItem item, BoardSlotBase slot, bool snap = false)
		{
			var below = slot.Below;
			if (below.Available)
				SetFallingCouple(item, below);
			else
				SetRestedCouple(item, slot, false);
			item.Pos = slot.Pos;
		}

		public static void SwapItem(BoardItem itemToSwapTo, BoardSlotBase slot)
		{
			var oldItem = slot.ConnectedItem;
			SetCouple(itemToSwapTo, slot, true);
			if (oldItem != null)
			{
				oldItem.ConnectedSlot = null;
				oldItem.GetDestroyedInstant();
			}
		}

		public static void SetFallingCouple(BoardItem item, BoardSlotBase slot)
		{
			slot.SetFallingItem(item);
			if (item.ConnectedSlot != null)
				item.ConnectedSlot.OnItemLeaving();
			item.SetFallingSlot(slot);
			item.GravityComponent.StartFalling();
		}

		public static void SetRestedCouple(BoardItem item, BoardSlotBase slot, bool snap = false)
		{
			item.SetRestingSlot(slot);
			slot.SetRestingItem(item);
			if (snap)
				item.Pos = slot.Pos;
		}
		#endregion

		public void OnPointerClick(PointerEventData eventData)
		{
			if (Resting && !GettingDestroyed && !TapsDisabled)
			{
				bool success = TapComponent.Tap();
				if (success)
					BoardManager.I.OnTapProcessed();
			}
		}

		public bool CompareColor(BoardItem neighbor)
		{
			return ComparisonComponent.CompareColor(neighbor.ComparisonComponent);
		}

		public virtual void OnBlockRefresh(ItemBlockRule.SpecialMakerRule targetRule, List<BoardItem> block) { }

		public virtual void MaybeAddToGoals(GoalTracker goalTracker) { }

		public enum TypeId
		{
			Cube = 1,
			Special = 2,
			Obstacle = 3,
		}

		public class Dependency
		{
			public TypeId Id;

			public Dependency(TypeId id)
			{
				Id = id;
			}
		}
	}
}