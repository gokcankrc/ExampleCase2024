namespace Game.Board
{
	public interface IBreakableItem
	{
		public abstract void OnDamageTaken();
		public abstract void OnBroken();
	}
}