using Gokcan.Helpers;
using System;

namespace Game.Core
{
	public class GameManager : LazySingleton<GameManager>
	{
		public Action GameLoaded;

		private ILevelStarter _levelStarter;

		public void Init(Dependency dep)
		{
			_levelStarter = dep.LevelStarter;
		}

		public void OnGameLoaded()
		{
			GameLoaded?.Invoke();
		}

		public void StartLevel()
		{
			_levelStarter.StartLevel();
		}

		public class Dependency
		{
			public ILevelStarter LevelStarter;

			public Dependency(ILevelStarter levelStarter)
			{
				LevelStarter = levelStarter;
			}
		}
	}
}