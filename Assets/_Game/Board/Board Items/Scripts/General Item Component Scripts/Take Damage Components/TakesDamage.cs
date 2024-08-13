namespace Game.Board
{
	public class TakesDamage : TakeDamageComponent
	{
		protected ItemDurability _durability;

		public TakesDamage(BoardItem connectedItem, ItemDurability durability) : base(connectedItem)
		{
			_durability = durability;
		}

		public TakesDamage(BoardItem connectedItem) : base(connectedItem)
		{
			_durability = new(connectedItem, 1);
		}

		public override void TakeDamage()
		{
			_durability.TakeDamage();
		}
	}
}