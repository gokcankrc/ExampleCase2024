using UnityEngine;

namespace Game.Board
{
	public class BoardSlot : BoardSlotBase
	{
		public override bool Available => ItemConnection == ItemConnectionType.None;
		public bool Resting => ItemConnection == ItemConnectionType.Resting;

		public ItemConnectionType ItemConnection;

		public override void OnItemDestroyed()
		{
			ConnectedItem = null;
			ItemConnection = ItemConnectionType.None;
			Above.OnBelowBecameAvailable(GridPos);
		}

		public override void OnItemLeaving()
		{
			ItemConnection = ItemConnectionType.Leaving;
		}

		public override void OnBelowBecameAvailable(Vector2Int availablePos)
		{
			if (Resting)
				ConnectedItem.GravityComponent.OnBelowEmpty();
		}

		public override void SetRestingItem(BoardItem newItem)
		{
			ConnectedItem = newItem;
			ItemConnection = ItemConnectionType.Resting;
		}

		public override void SetFallingItem(BoardItem newItem)
		{
			ConnectedItem = newItem;
		}

		public override void OnLeavingItemFarEnough()
		{
			ConnectedItem = null;
			ItemConnection = ItemConnectionType.None;
			Above.OnBelowBecameAvailable(GridPos);
		}

#if UNITY_EDITOR
		[SerializeField] private bool GizmosEnabled = false;

		private void OnDrawGizmos()
		{
			if (!GizmosEnabled) return;

			Gizmos.color = Color.red;
			if (ConnectedItem != null)
				Gizmos.DrawLine(Pos, ConnectedItem.Pos);
		}
#endif
	}
}