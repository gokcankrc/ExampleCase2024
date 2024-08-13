using Game.SpecialEffect;

namespace Game.Board
{
	public class SpecialComparison : ComparisonComponent
	{
		public SpecialComparison(BoardItem connectedItem, SpecialEffectMaker.ItemType exactSpecialType) : base(connectedItem)
		{
			ExactSpecialType = exactSpecialType;
			SpecialType = exactSpecialType switch
			{
				SpecialEffectMaker.ItemType.VerticalRocket => SpecialType.Rocket,
				SpecialEffectMaker.ItemType.HorizontalRocket => SpecialType.Rocket,
				SpecialEffectMaker.ItemType.Tnt => SpecialType.Tnt,
				SpecialEffectMaker.ItemType.None => throw new System.NotImplementedException(),
				_ => throw new System.NotImplementedException(),
			};
		}

		public override bool CompareColor(ComparisonComponent comparable)
		{
			return false;
		}
	}
}