using Gokcan.Helpers;
using System;
using System.Threading.Tasks;
using UnityEngine;

namespace Game.Board
{
	public class ProgressionManager : LazySingleton<ProgressionManager>
	{
		public Action ProgressionChanged;
		[NonSerialized] public Level GetCurrentLevel;
		public Task Loader;
		[NonSerialized] public int CurrentLevelNumber;

		private const string LevelSaveKey = "CurrentLevelNumber";

		public void Init()
		{
			CurrentLevelNumber = PlayerPrefs.GetInt(LevelSaveKey, 1);
			LevelManager.I.LevelSucceded += Save;
			StartLoading();
		}

		private void StartLoading()
		{
            Loader = LevelLoader.I.LoadLevel(CurrentLevelNumber - 1, LoadedLevel);
		}

		private void LoadedLevel(Level level)
		{
			GetCurrentLevel = level;
		}

		private void Save()
		{
			int levelNumber = GetCurrentLevel.Number + 1;
			SetLevel(levelNumber);
			CurrentLevelNumber = levelNumber;
			ProgressionChanged?.Invoke();
			GetCurrentLevel = null;
			StartLoading();
		}

		public static void SetLevel(int levelNumber)
		{
			PlayerPrefs.SetInt(LevelSaveKey, levelNumber);
		}
	}
}