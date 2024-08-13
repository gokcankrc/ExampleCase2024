namespace Game.Board
{
	public class MergeColorsTap : TapComponent
	{
		public MergeColorsTap(BoardItem connectedItem) : base(connectedItem) { }

		protected override bool OnTap()
		{
			return BoardItemBlockProcessor.ExecuteColor(_connectedItem);
		}

		protected override void OnIndirect() { }
	}
}