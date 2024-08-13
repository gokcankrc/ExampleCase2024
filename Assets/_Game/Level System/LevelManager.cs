using Game.Core;
using Gokcan.Helpers;
using System;
using System.Threading.Tasks;

namespace Game.Board
{
	public class LevelManager : LazySingleton<LevelManager>, ILevelStarter, IGameScreen
	{
		public bool IsInTransition => _isInTransition;
		private bool _isInTransition = false;

		private static int _activeActionCounterField = 0;
		public static int ActiveActionCounter
		{
			get => _activeActionCounterField;
			set
			{
				_activeActionCounterField = value;
				if (_activeActionCounterField == 0)
					I.ActiveActionsDone();
			}
		}

		public Action LevelSucceded;
		public Action<LevelManager> LevelActivated;
		public Action<LevelManager> LevelDeactivated;

		[NonSerialized] public Level CurrentLevel;
		[NonSerialized] public GoalTracker GoalTracker;
		[NonSerialized] public FailTracker FailTracker;
		[NonSerialized] public BoardManager BoardManager;

		private bool _loseConditionIsMet;

		public void Init(Dependency dep)
		{
			GoalTracker = dep.GoalTracker;
			GoalTracker.GoalsFinished += OnLevelSucceded;
			BoardManager = dep.BoardManager;
		}

		public async Task ActivateGameScreen()
		{
			_isInTransition = true;
			_loseConditionIsMet = false;
			ActiveActionCounter = 0;
			await ProgressionManager.I.Loader;
			CurrentLevel = ProgressionManager.I.GetCurrentLevel;
			FailTracker = CurrentLevel.FailTracker;
			FailTracker.Init(this);

			BoardManager.ActivateBoard(this, CurrentLevel);
			GoalTracker.Activate();
			LevelActivated?.Invoke(this);
			_isInTransition = false;
			BoardManager.I.RecalculateSpecialShowers();
		}

		public void DeactivateGameScreen()
		{
			FailTracker.Terminate(this);
			BoardManager.DeactivateBoard();
			GoalTracker.Deactivate();
			LevelDeactivated?.Invoke(this);
		}

		public void StartLevel()
		{
			GameScreenManager.I.SwitchGameScreen(this);
		}

		private void OnLevelSucceded()
		{
			LevelSucceded?.Invoke();
		}

		public void OnLoseConditionMet()
		{
			_loseConditionIsMet = true;
		}

		private void ActiveActionsDone()
		{
			if (_isInTransition) return;

			if (_loseConditionIsMet && ActiveActionCounter == 0)
			{
				if (!GoalTracker.Won)
				{
					_loseConditionIsMet = false;
					FailTracker.CanLoseNow();
				}
			}

			BoardManager.I.RecalculateSpecialShowers();
		}

		public void DebugSucceed()
		{
			LevelSucceded?.Invoke();
		}

		public class Dependency
		{
			public BoardManager BoardManager;
			public GoalTracker GoalTracker;

			public Dependency(BoardManager boardManager, GoalTracker goalTracker)
			{
				BoardManager = boardManager;
				GoalTracker = goalTracker;
			}
		}
	}
}