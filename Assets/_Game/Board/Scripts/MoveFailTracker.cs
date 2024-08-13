namespace Game.Board
{
	public class MoveFailTracker : FailTracker
	{
		protected int _movesLeft;
		protected LevelManager _levelManager;

		public MoveFailTracker(int movesLeft)
		{
			_movesLeft = movesLeft;
		}

		public override void Init(LevelManager levelManager)
		{
			_levelManager = levelManager;
			levelManager.LevelActivated += Refresh;
			ConditionUpdated?.Invoke(new("" + _movesLeft));
			BoardItem.TapsDisabled = false;
		}

		public override void Terminate(LevelManager levelManager)
		{
			levelManager.LevelActivated -= Refresh;
		}

		public override void OnTapProcessed()
		{
			_movesLeft -= 1;
			Refresh();
		}

		private void Refresh(LevelManager levelManager)
		{
			Refresh();
		}

		private void Refresh()
		{
			ConditionUpdated?.Invoke(new("" + _movesLeft));
			if (_movesLeft == 0)
			{
				BoardItem.TapsDisabled = true;
				_levelManager.OnLoseConditionMet();
			}
		}
	}
}