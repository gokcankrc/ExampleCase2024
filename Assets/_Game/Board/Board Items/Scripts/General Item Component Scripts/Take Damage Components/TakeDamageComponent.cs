namespace Game.Board
{
	public abstract class TakeDamageComponent : BoardItemComponent
	{
		public TakeDamageComponent(BoardItem connectedItem) : base(connectedItem) { }

		public abstract void TakeDamage();
	}
}