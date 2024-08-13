namespace Game.Board
{
	public class StoneItem : ObstacleItem<StoneItem.Dependency>
	{
		public override GoalType GetObstacleType => GoalType.Stone;

		protected override GravityComponent _gravityComponentMaker => new FixedGravity(this);
		protected override TakeDamageComponent _directDamageComponentMaker => new TakesDamage(this, _durability);
		protected override TakeDamageComponent _adjacentDamageComponentMaker => new DoesntTakeDamage(this);

		public new class Dependency : BoardItem.Dependency
		{
			public Dependency() : base(TypeId.Obstacle) { }
		}
	}
}