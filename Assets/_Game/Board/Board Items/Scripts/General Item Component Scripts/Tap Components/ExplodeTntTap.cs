using Game.SpecialEffect;

namespace Game.Board
{
	public class ExplodeTntTap : SpecialTapComponent
	{
		public ExplodeTntTap(BoardItem connectedItem) : base(connectedItem)
		{
			_exactSpecialType = SpecialEffectMaker.ItemType.Tnt;
		}
	}
}