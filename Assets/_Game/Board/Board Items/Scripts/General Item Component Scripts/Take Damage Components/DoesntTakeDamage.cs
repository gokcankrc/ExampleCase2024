namespace Game.Board
{
	public class DoesntTakeDamage : TakeDamageComponent
	{
		public DoesntTakeDamage(BoardItem connectedItem) : base(connectedItem) { }

		public override void TakeDamage() { }
	}
}