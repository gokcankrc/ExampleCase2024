using UnityEngine;

namespace Game.Board
{
	public class CeilingSlot : BoardSlotBase
	{
		public override bool Available => true;

		public override void OnItemLeaving() { }

		public override void OnBelowBecameAvailable(Vector2Int availablePos)
		{
			BoardManager.I.CeilingBecameAvailable(availablePos);
		}

		public override void OnLeavingItemFarEnough()
		{
			// Gets notified by slot below, expected behaviour.
		}
	}
}