using System;
using System.Collections.Generic;
using UnityEngine;

namespace Game.Board
{
	[Serializable]
	public class Level
	{
		public int Number => level_number;
		public Vector2Int BoardSize => new(grid_width, grid_height);
		public int MoveCount => move_count;
		public FailTracker FailTracker => new MoveFailTracker(MoveCount);

		[SerializeField] private int level_number;
		[SerializeField] private int grid_width;
		[SerializeField] private int grid_height;
		[SerializeField] private int move_count;
		[SerializeField] private List<string> grid;

		public void SetSize(Vector2Int testBoardSize)
		{
			grid_width = testBoardSize.x;
			grid_height = testBoardSize.y;
		}

		public void Fill(BoardSlot[,] slots, GoalTracker goalTracker)
		{
			for (int i = 0; i < grid.Count; i++)
			{
				int x = i % grid_width;
				int y = i / grid_width;
				if (CheckOverflow(x, y, slots))
				{
					Debug.LogWarning("Level load might have gone bad.");
					continue;
				}
				SpawnItem(slots[x, y], grid[i], goalTracker);
			}
		}

		private bool CheckOverflow(int x, int y, BoardSlot[,] slots)
		{
			return x >= slots.GetLength(0) || y >= slots.GetLength(1);
		}

		private void SpawnItem(BoardSlotBase slot, string id, GoalTracker goalTracker)
		{
			if (LevelLoader.MakersTable.TryGetValue(id, out Func<BoardItem> makerAction))
			{
				BoardItem item = makerAction();
				item.MaybeAddToGoals(goalTracker);
				Vector3 spawnLocation = slot.Pos;
				item.Pos = spawnLocation;
				BoardItem.SetCouple(item, slot);
			}
			else
			{
				Debug.LogWarning($"Couldn't find id: {id} in lookup table");
			}
		}
	}
}