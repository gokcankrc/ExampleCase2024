using UnityEngine;

namespace Game.Board
{
	public class FallingGravity : GravityComponent
	{
		protected FallingGravitySettings _dep;

		public override bool IsStatic => false;
		protected Vector3 Pos => _connectedItem.Pos;
		protected bool _falling => FallState == State.Falling;
		private float _enoughDelayDistance => _dep.EnoughDelayDistance;
		protected bool _notifiedAbove = true;
		protected BoardSlotBase _aboveSlotToNotify = null;

		public FallingGravity(BoardItem connectedItem) : base(connectedItem)
		{
			_dep = BoardSettings.I.GravitySettings;
		}

		public override void OnActivate()
		{
			base.OnActivate();
		}

		public override void OnDeactivate()
		{
			base.OnDeactivate();

			if (!_notifiedAbove)
				NotifyAbove();

			if (_falling)
				LevelManager.ActiveActionCounter -= 1;
			FallState = State.Stopped;
			_aboveSlotToNotify = null;
			_notifiedAbove = true;
		}

		public override void OnBelowEmpty()
		{
			if (_connectedItem.GettingDestroyed) return;

			BoardItem.RefreshCouple(_connectedItem);
		}

		public override void StartFalling()
		{
			if (_falling) return;
			FallState = State.Falling;
			LevelManager.ActiveActionCounter += 1;
			_aboveSlotToNotify = _connectedItem.ConnectedSlot.Above;
			_notifiedAbove = false;
		}

		public override void Update(float deltaTime)
		{
			if (!_falling) return;

			Fall(deltaTime);

			if (CheckFellFarEnough(_aboveSlotToNotify))
				NotifyAbove();

			var slotFallingTo = _connectedItem.ConnectedSlot;
			if (CheckReached(slotFallingTo))
			{
				if (slotFallingTo.CanFallBelow)
					UpdateTargetSlot(slotFallingTo.Below);
				else
					StopAt(slotFallingTo);
			}
		}

		private void NotifyAbove()
		{
			_aboveSlotToNotify.OnLeavingItemFarEnough();
			_aboveSlotToNotify = null;
			_notifiedAbove = true;
		}

		private void Fall(float deltaTime)
		{
			_connectedItem.Pos += Vector3.down * (_dep.Speed * deltaTime);
			Moved?.Invoke();
		}

		private bool CheckFellFarEnough(BoardSlotBase slot)
		{
			if (_notifiedAbove) return false;

			return slot.Pos.y > Pos.y + _enoughDelayDistance;
		}

		private bool CheckReached(BoardSlotBase slot)
		{
			return slot.Pos.y > Pos.y;
		}

		private void StopAt(BoardSlotBase slot)
		{
			if (_falling)
				LevelManager.ActiveActionCounter -= 1;
			FallState = State.Stopped;
			_connectedItem.Pos = slot.Pos;
			BoardItem.SetRestedCouple(_connectedItem, slot);
			Stopped?.Invoke();
		}

		private void UpdateTargetSlot(BoardSlotBase target)
		{
			if (!_notifiedAbove)
			{
				NotifyAbove();
				Debug.LogWarning("Fail safe triggered, target did not notify above till now.", _connectedItem.gameObject);
			}
			_aboveSlotToNotify = _connectedItem.ConnectedSlot;
			_notifiedAbove = false;
			BoardItem.SetFallingCouple(_connectedItem, target);
		}
	}
}