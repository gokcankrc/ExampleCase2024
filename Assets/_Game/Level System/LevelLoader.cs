using Gokcan.Helpers;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using UnityEngine;
using UnityEngine.AddressableAssets;
using UnityEngine.ResourceManagement.AsyncOperations;
using static Game.Board.BoardItemMaker;

namespace Game.Board
{
	public class LevelLoader : LazySingleton<LevelLoader>
	{
		public static readonly Dictionary<string, Func<BoardItem>> MakersTable =
			new()
			{
				{ "r", Cube.Green},
				{ "g", Cube.Red},
				{ "b", Cube.Blue},
				{ "y", Cube.Yellow },
				{ "rand", Cube.Random },
				{ "t", Special.Tnt },
				{ "rov", Special.RocketVertical },
				{ "roh", Special.RocketHorizontal },
				{ "bo", Obstacle.Box },
				{ "s", Obstacle.Stone },
				{ "v", Obstacle.Vase }
			};

		[NonSerialized] public LevelBundle LevelBundle;

		public void Init(Dependency dep)
		{
			LevelBundle = dep.LevelBundle;
		}

		public Task LoadLevel(int levelIndex, Action<Level> loadedAction)
		{
			if (levelIndex >= LevelBundle.LevelAssets.Count || levelIndex < 0)
				levelIndex = 0;

			var tcs = new TaskCompletionSource<Level>();
			action();
			return tcs.Task;

			async void action()
			{
				AsyncOperationHandle<TextAsset> handle = default;
				try
				{
					AssetReference levelAsset = LevelBundle.LevelAssets[levelIndex];
					handle = levelAsset.LoadAssetAsync<TextAsset>();

					_ = await handle.Task;
					Level level = JsonUtility.FromJson<Level>(handle.Result.text);

					loadedAction?.Invoke(level);
					tcs.SetResult(level);
				}
				catch (Exception e)
				{
					Debug.LogError(e);
					tcs.SetException(e);
				}
				finally
				{
					if (handle.IsValid())
						Addressables.Release(handle);
				}
			}
		}

		public class Dependency
		{
			public LevelBundle LevelBundle;

			public Dependency(LevelBundle levelBundle)
			{
				LevelBundle = levelBundle;
			}
		}
	}
}