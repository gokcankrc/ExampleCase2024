using Game.Board;
using Game.Core;
using Gokcan.PoolSystem;
using System.Threading.Tasks;
using UnityEngine;

namespace Game.EntryPoint
{
	public abstract class DependencyInjector : MonoBehaviour
	{
		protected abstract Args GetArgs { get; }

		public Task InitializeApplication()
		{
			ReloadDomains();

			Args args = GetArgs;
			PoolManager.I.Init(new(PoolInitializer.I));
			LevelLoader.I.Init(new(args.LevelBundle));
			LevelManager.I.Init(new(args.BoardManager, args.GoalTracker));
			BoardManager.I.Init(new());

			GameScreenManager.Dependency screenManagerDep = new(
				args.MainMenuGameScreen,
				args.LevelGameScreen,
				args.InitialGameScreen);
			GameScreenManager.I.Init(screenManagerDep);
			GameManager.I.Init(new(levelStarter: LevelManager.I));

			ProgressionManager.I.Init();
			GameScreenManager.I.SwitchToInitialScreen();

			// No stages currently require async load.
			return Task.CompletedTask;
		}

		public virtual void OnApplicationLoaded()
		{
			GameManager.I.OnGameLoaded();
		}

		private static void ReloadDomains()
		{
			// Reloading domain manually rather than letting Unity do it saves up 15~ seconds per Play.
			// The catch is you need to manually reset static values.
			// Some plugings will require changes because they might rely on Unity domain reloads.
			PoolManager.AllPools = new();
		}

		protected class Args
		{
			public BoardManager BoardManager;
			public GoalTracker GoalTracker;
			public IGameScreen MainMenuGameScreen;
			public IGameScreen LevelGameScreen;
			public IGameScreen InitialGameScreen;
			public LevelBundle LevelBundle;

			public Args(
				BoardManager boardManager,
				GoalTracker goalTracker,
				IGameScreen mainMenuGameScreen,
				IGameScreen levelGameScreen,
				IGameScreen initialGameScreen,
				LevelBundle levelBundle)
			{
				BoardManager = boardManager;
				GoalTracker = goalTracker;
				MainMenuGameScreen = mainMenuGameScreen;
				LevelGameScreen = levelGameScreen;
				InitialGameScreen = initialGameScreen;
				LevelBundle = levelBundle;
			}
		}
	}
}
