
namespace Game.Board
{
	public class CubeTypeComparison : ComparisonComponent
	{
		public CubeTypeComparison(BoardItem connectedItem, CubeType cubeType) : base(connectedItem)
		{
			CubeType = cubeType;
		}

		public override bool CompareColor(ComparisonComponent comparable)
		{
			return CubeType == comparable.CubeType;
		}
	}
}