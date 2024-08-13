namespace Game.Board
{
	public class BoardItemComponent
	{
		protected BoardItem _connectedItem;

		public BoardItemComponent(BoardItem connectedItem)
		{
			_connectedItem = connectedItem;
		}

		public void Initialize(BoardItem connectedItem)
		{
			_connectedItem = connectedItem;
		}

		public virtual void OnActivate() { }
		public virtual void OnDeactivate() { }
	}
}