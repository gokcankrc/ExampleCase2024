using Game.Board;
using Game.Core;
using UnityEngine;

namespace Game.EntryPoint
{
	public class DefaultApplicationStarter : DependencyInjector
	{
		[SerializeField] private bool _testLevelActive;
		[SerializeField] private Vector2Int _testBoardSize;
		[SerializeField] private bool _startLevelInstantly;
		[SerializeField] private LevelBundle _levelBundle;

		protected override Args GetArgs => new(
					boardManager: BoardManager.I,
					goalTracker: GoalTracker.I,
					mainMenuGameScreen: MainMenuHandler.I,
					levelGameScreen: LevelManager.I,
					initialGameScreen: MainMenuHandler.I,
					levelBundle: _levelBundle);

		public override void OnApplicationLoaded()
		{
			base.OnApplicationLoaded();

			LateCall();
		}

		public async void LateCall()
		{
			await ProgressionManager.I.Loader;
			if (_testLevelActive)
				ProgressionManager.I.GetCurrentLevel.SetSize(_testBoardSize);
			if (_startLevelInstantly)
				GameManager.I.StartLevel();
		}
	}
}
