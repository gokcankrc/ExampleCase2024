
namespace Game.Board
{
	public abstract class ControllableBoardItem<T> : BoardItemT<T> where T : BoardItem.Dependency
	{
		protected override GravityComponent _gravityComponentMaker => new FallingGravity(this);
		protected override TakeDamageComponent _directDamageComponentMaker => new TakesDamage(this, new(this, 1));
		protected override TakeDamageComponent _adjacentDamageComponentMaker => new DoesntTakeDamage(this);
	}
}