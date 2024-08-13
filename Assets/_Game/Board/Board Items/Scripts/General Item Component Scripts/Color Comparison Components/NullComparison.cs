namespace Game.Board
{
	public class NullComparison : ComparisonComponent
	{
		public NullComparison(BoardItem connectedItem) : base(connectedItem) { }

		public override bool CompareColor(ComparisonComponent comparable)
		{
			return false;
		}
	}
}