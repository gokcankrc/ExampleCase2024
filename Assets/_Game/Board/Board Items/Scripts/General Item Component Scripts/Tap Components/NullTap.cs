namespace Game.Board
{
	public class NullTap : TapComponent
	{
		public NullTap(BoardItem connectedItem) : base(connectedItem) { }

		protected override bool OnTap() { return false; }

		protected override void OnIndirect() { }
	}
}