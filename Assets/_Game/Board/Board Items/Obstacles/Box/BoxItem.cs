namespace Game.Board
{
	public class BoxItem : ObstacleItem<BoxItem.Dependency>
	{
		public override GoalType GetObstacleType => GoalType.Box;

		protected override GravityComponent _gravityComponentMaker => new FixedGravity(this);
		protected override TakeDamageComponent _directDamageComponentMaker => new TakesDamage(this, _durability);
		protected override TakeDamageComponent _adjacentDamageComponentMaker => new TakesDamage(this, _durability);

		public new class Dependency : BoardItem.Dependency
		{
			public Dependency() : base(TypeId.Obstacle) { }
		}
	}
}