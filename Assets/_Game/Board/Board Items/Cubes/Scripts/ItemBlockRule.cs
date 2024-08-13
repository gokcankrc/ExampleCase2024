using System;
using System.Collections.Generic;
using UnityEngine;

namespace Game.Board
{
	[CreateAssetMenu(fileName = "Item Block Rule", menuName = "Board/Item Block Rule")]
	public class ItemBlockRule : ScriptableObject
	{
		public int MinRequiredToPop;
		public List<SpecialMakerRule> SpecialMakerRules;

		[Serializable]
		public class SpecialMakerRule
		{
			public int MinRequired;
			public SpecialType ItemType;
		}
	}
}