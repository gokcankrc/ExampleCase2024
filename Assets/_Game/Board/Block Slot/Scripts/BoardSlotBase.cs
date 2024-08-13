using Game.GridSystem;
using Gokcan.Helpers;
using System.Collections.Generic;
using UnityEngine;

namespace Game.Board
{
	public abstract class BoardSlotBase : GridSlot
	{
		public IEnumerable<BoardSlotBase> Neighbors => BoardManager.I.GetSlots(GridPos.GetNeighbors());
		public BoardSlotBase Above => BoardManager.I.GetSlot(GridPos + Vector2Int.up);
		public BoardSlotBase Below => BoardManager.I.GetSlot(GridPos + Vector2Int.down);

		public bool CanFallBelow => Below.Available;
		public Vector3 Pos => transform.position;
		public Vector3 AbovePos => BoardManager.I.GetWorldPos(GridPos + Vector2Int.up);
		public Vector3 BelowPos => BoardManager.I.GetWorldPos(GridPos + Vector2Int.down);

		public BoardItem ConnectedItem;

		public virtual void OnItemDestroyed()
		{
			Debug.LogError("An item got destroyed from an illegal slot.");
		}

		public virtual void OnItemLeaving()
		{
			Debug.LogError("An item left from an illegal slot.");
		}

		public virtual void OnBelowBecameAvailable(Vector2Int availablePos)
		{
			Debug.LogError("Illegal slot's below became avalable.");
		}

		public virtual void SetRestingItem(BoardItem newItem)
		{
			Debug.LogError("Illegal slot received a falling target.");
		}

		public virtual void SetFallingItem(BoardItem newItem)
		{
			Debug.LogError("Illegal slot received a falling target.");
		}

		public virtual void OnLeavingItemFarEnough()
		{
			Debug.LogError("Illegal slot is now \"Fallable To\".");
		}

		public enum ItemConnectionType
		{
			None,
			Incoming,
			Resting,
			Leaving,
		}
	}
}