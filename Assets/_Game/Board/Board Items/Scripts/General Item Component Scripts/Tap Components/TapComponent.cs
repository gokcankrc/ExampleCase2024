using Game.SpecialEffect;

namespace Game.Board
{
	public abstract class TapComponent : BoardItemComponent
	{
		protected bool _actionTriggered;

		public virtual SpecialEffectMaker.ItemType GetExactSpecialType => SpecialEffectMaker.ItemType.None;

		protected TapComponent(BoardItem connectedItem) : base(connectedItem) { }

		protected abstract bool OnTap();
		protected abstract void OnIndirect();

		public override void OnActivate()
		{
			base.OnActivate();
			_actionTriggered = false;
		}

		public bool Tap()
		{
			if (_actionTriggered) return false;
			_actionTriggered = true;
			return OnTap();
		}

		public void Indirect()
		{
			if (_actionTriggered) return;
			_actionTriggered = true;
			OnIndirect();
		}
	}
}