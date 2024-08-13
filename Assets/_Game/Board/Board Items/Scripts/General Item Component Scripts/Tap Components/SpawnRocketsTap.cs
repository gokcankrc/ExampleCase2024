using Game.SpecialEffect;

namespace Game.Board
{
	public class SpawnRocketsTap : SpecialTapComponent
	{
		protected RocketEffect.FireDir _fireDir;

		public SpawnRocketsTap(BoardItem connectedItem, RocketEffect.FireDir fireDir) : base(connectedItem)
		{
			_fireDir = fireDir;
			_exactSpecialType = fireDir switch
			{
				RocketEffect.FireDir.horizontal => SpecialEffectMaker.ItemType.HorizontalRocket,
				RocketEffect.FireDir.vertical => SpecialEffectMaker.ItemType.VerticalRocket,
				_ => throw new System.NotImplementedException(),
			};
		}
	}
}