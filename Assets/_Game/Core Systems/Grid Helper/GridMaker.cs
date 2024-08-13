using Gokcan.Helpers;
using UnityEngine;

namespace Game.GridSystem
{
	public static class GridMaker
	{
		public static void SpawnGrid<T>(GridSpawnArgs<T> args) where T : GridSlot
		{
			Vector2[,] posArray = GeneratePositions(args);
			InstantiateAndPlaceObjects(args, posArray);
		}

		public static Vector2[,] GeneratePositions<T>(GridSpawnArgs<T> args) where T : GridSlot
		{
			Vector2Int dimensions = args.Dimensions;
			Grid grid = args.Grid;
			Vector2 startPos = -grid.CellToWorld((Vector3Int)dimensions - Vector3Int.one) / 2;
			startPos += Vector2.down * args.StartOffset;

			Vector2[,] posArray = new Vector2[dimensions.x, dimensions.y];
			for (int x = 0; x < dimensions.x; x++)
			{
				for (int y = 0; y < dimensions.y; y++)
				{
					Vector2 pos = grid.CellToWorld(new(x, y));
					posArray[x, y] = pos + startPos;
				}
			}
			Vector2 bgSize = grid.GetTotalBounds(dimensions).size + args.BackgroundBoundsOffset.size;
			args.Background.size = bgSize;
			args.Background.transform.position = args.BackgroundBoundsOffset.center + (Vector3.down * args.StartOffset);
			return posArray;
		}

		public static void InstantiateAndPlaceObjects<T>(GridSpawnArgs<T> args, Vector2[,] posArray)
			where T : GridSlot
		{
			Vector2Int dimensions = args.Dimensions;

			for (int x = 0; x < dimensions.x; x++)
				for (int y = 0; y < dimensions.y; y++)
					args.GridToFill[x, y] = spawnBoardSlot(new Vector2Int(x, y), posArray[x, y]);

			T spawnBoardSlot(Vector2Int gridPos, Vector2 pos)
			{
				T newSlot = Object.Instantiate(args.Prefab, args.CellParent);
				newSlot.name = $"{newSlot.name} {gridPos.x}-{gridPos.y}";
				newSlot.Init(gridPos);
				newSlot.transform.position = pos;
				return newSlot;
			}
		}

		public static Vector3 GetCanvasPos(Vector2 start, Vector2 spacing, Vector2Int boardPos)
		{
			Vector2 canvasPos = spacing;
			canvasPos.Scale(boardPos);
			canvasPos += start;
			return canvasPos;
		}

		public class GridSpawnArgs<T> where T : GridSlot
		{
			public float StartOffset;
			public Grid Grid;
			public Transform CellParent;
			public SpriteRenderer Background;
			public Bounds BackgroundBoundsOffset;
			public Vector2Int Dimensions;
			public T Prefab;
			public T[,] GridToFill;

			public GridSpawnArgs(
				float startOffset,
				Grid grid,
				Transform cellParent,
				SpriteRenderer background,
				Bounds backgroundBoundsOffset,
				T prefab,
				T[,] gridToFill)
			{
				StartOffset = startOffset;
				Grid = grid;
				CellParent = cellParent;
				Background = background;
				BackgroundBoundsOffset = backgroundBoundsOffset;
				Prefab = prefab;
				GridToFill = gridToFill;
				Dimensions = new Vector2Int(gridToFill.GetLength(0), gridToFill.GetLength(1));
			}
		}
	}
}