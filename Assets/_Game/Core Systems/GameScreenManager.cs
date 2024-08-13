using Gokcan.Helpers;
using System;

namespace Game.Core
{
	public class GameScreenManager : LazySingleton<GameScreenManager>
	{
		public Action GameScreenChanging;
		[NonSerialized] public bool InTransition;
		public Action GameScreenChanged;
		public IGameScreen MainScreen;
		public IGameScreen LevelScreen;
		public IGameScreen InitialScreen;
		public IGameScreen CurrentScreen;

		public void Init(Dependency dep)
		{
			MainScreen = dep.MainMenuGameScreen;
			LevelScreen = dep.LevelGameScreen;
			InitialScreen = dep.InitialGameScreen;
		}

		public void SwitchToInitialScreen()
		{
			SwitchIn(InitialScreen);
		}

		public void SwitchToMainScreen()
		{
			SwitchGameScreen(MainScreen);
		}

		public void ReloadLevel()
		{
			SwitchGameScreen(LevelScreen);
		}

		public void SwitchGameScreen(IGameScreen newScreen)
		{
			if (CurrentScreen.IsInTransition) return;

			SwitchOut();
			SwitchIn(newScreen);
		}

		private void SwitchOut()
		{
			InTransition = true;
			GameScreenChanging?.Invoke();
			CurrentScreen.DeactivateGameScreen();
		}

		private async void SwitchIn(IGameScreen newScreen)
		{
			CurrentScreen = newScreen;
			await CurrentScreen.ActivateGameScreen();
			InTransition = false;
			GameScreenChanged?.Invoke();
		}

		public class Dependency
		{
			public IGameScreen MainMenuGameScreen;
			public IGameScreen LevelGameScreen;
			public IGameScreen InitialGameScreen;

			public Dependency(
				IGameScreen mainMenuGameScreen,
				IGameScreen levelGameScreen,
				IGameScreen initialGameScreen)
			{
				MainMenuGameScreen = mainMenuGameScreen;
				LevelGameScreen = levelGameScreen;
				InitialGameScreen = initialGameScreen;
			}
		}
	}
}