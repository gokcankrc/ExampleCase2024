using System;
using UnityEngine;

namespace Game.Board
{
	[Serializable]
	public class ItemDurability
	{
		[SerializeField] private int _max = 1;

		private IBreakableItem _breakableItem;
		private int _durabilityLeft = 1;

		// To instantiate through code.
		public ItemDurability(IBreakableItem breakableItem, int durabilityLeft)
		{
			_max = durabilityLeft;
			OnActivate(breakableItem);
		}

		public void OnActivate(IBreakableItem breakableItem)
		{
			_durabilityLeft = _max;
			_breakableItem = breakableItem;
		}

		public void TakeDamage()
		{
			_durabilityLeft -= 1;
			_breakableItem.OnDamageTaken();
			if (_durabilityLeft == 0)
				_breakableItem.OnBroken();
		}
	}
}