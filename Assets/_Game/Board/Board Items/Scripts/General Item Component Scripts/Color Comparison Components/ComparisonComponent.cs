using Game.SpecialEffect;
using System.Collections.Generic;

namespace Game.Board
{
	public abstract class ComparisonComponent : BoardItemComponent
	{
		public CubeType CubeType;
		public SpecialType SpecialType;
		public SpecialEffectMaker.ItemType ExactSpecialType { get; protected set; }

		public ComparisonComponent(BoardItem connectedItem) : base(connectedItem) { }

		public abstract bool CompareColor(ComparisonComponent comparable);

		public static List<BoardItem> FindNeighborSpecials(BoardItem boardItem)
		{
			List<BoardItem> neighborSpecials = new();
			foreach (BoardItem item in boardItem.Neighbors)
			{
				if (item == null) continue;
				if (item.Id is BoardItem.TypeId.Special)
					neighborSpecials.Add(item);
			}

			return neighborSpecials;
		}
	}
}