using Game.Core;
using Game.GridSystem;
using Gokcan.Helpers;
using Gokcan.PoolSystem;
using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace Game.Board
{
	[RequireComponent(typeof(Grid))]
	public class BoardManager : SceneSingleton<BoardManager>
	{
		[SerializeField] private Transform _boardParent;
		[SerializeField] private SpriteRenderer _boardBackground;
		[SerializeField] private BoardSlot _slotPrefab;
		[SerializeField] private Bounds _backgroundBoundsOffset;
		[SerializeField] private SpriteMask _spriteMask;
		[SerializeField] private CeilingSpawner _spawner;
		[SerializeField] private float _startOffset = 3f;

		[NonSerialized] public Action<BoardManager> BoardActivated;
		[NonSerialized] public BoardSlot[] TopmostRow;

		private Vector2Int _gridSize;
		private CeilingSlot _ceilingSlot;
		private SideBorderSlot _sideBorderSlot;
		private BedrockSlot _bedrockSlot;
		private Vector3 _zeroPos;
		private FailTracker _failTracker;
		private readonly List<BoardItem> _boardItems = new();
		private BoardSlot[,] _slots;
		private Grid _grid;

		private readonly Vector3 _nullPosition = new(-15, 15, 0);

		protected override void Awake()
		{
			base.Awake();

			_grid = gameObject.GetComponent<Grid>();
			_grid.cellSize = _slotPrefab.SizeReference.bounds.size;
		}

		public void Init(Dependency dep)
		{
			LevelManager.I.LevelActivated += Attach;

			createBorderSlots(ref _ceilingSlot);
			createBorderSlots(ref _sideBorderSlot);
			createBorderSlots(ref _bedrockSlot);
			_spawner.Init(this);

			void createBorderSlots<T>(ref T slotField) where T : BoardSlotBase
			{
				var name = typeof(T).ToString();
				var tObject = Instantiate(new GameObject(name), transform);
				slotField = tObject.AddComponent<T>();
			}
		}

		public void ActivateBoard(LevelManager levelManager, Level level)
		{
			_boardBackground.enabled = true;
			_boardParent.position = Vector3.zero;
			_gridSize = level.BoardSize;
			_slots = new BoardSlot[_gridSize.x, _gridSize.y];
			spawnAndFill(levelManager, level);
			_spawner.ActivateSpawns();

			handleSpriteMask();
			setBorderSlotLocations();

			BoardActivated?.Invoke(this);

			void spawnAndFill(LevelManager levelManager, Level level)
			{
				var args = new GridMaker.GridSpawnArgs<BoardSlot>(
					startOffset: _startOffset,
					grid: _grid,
					cellParent: BoardSlotsParent.Tr,
					background: _boardBackground,
					backgroundBoundsOffset: _backgroundBoundsOffset,
					prefab: _slotPrefab,
					gridToFill: _slots);
				GridMaker.SpawnGrid(args);
				_zeroPos = _slots[0, 0].Pos;

				level.Fill(_slots, levelManager.GoalTracker);

				TopmostRow = Enumerable.Range(0, _gridSize.x)
					.Select(x => _slots[x, _gridSize.y - 1])
					.ToArray();
			}

			void handleSpriteMask()
			{
				var _maskTr = _spriteMask.transform;
				var pos = _boardBackground.transform.position;
				pos += new Vector3(0, _boardBackground.bounds.size.y / 2, 0);
				pos -= _backgroundBoundsOffset.center + _backgroundBoundsOffset.extents / 2;
				_maskTr.position = pos;
				var scale = _maskTr.localScale;
				scale.x = _boardBackground.bounds.size.x * 2;
				_maskTr.localScale = scale;
			}

			void setBorderSlotLocations()
			{
				_ceilingSlot.transform.position = _slots[0, _gridSize.y - 1].AbovePos;
				_ceilingSlot.GridPos = new Vector2Int(0, _gridSize.y);
				_bedrockSlot.transform.position = _slots[0, 0].BelowPos;
				_bedrockSlot.GridPos = new Vector2Int(0, -1);
			}
		}

		private void Attach(LevelManager manager)
		{
			_failTracker = manager.FailTracker;
		}

		public void DeactivateBoard()
		{
			_boardBackground.enabled = false;
			_boardParent.position = _nullPosition;
			_spawner.DeactivateSpawns();
			for (int i = _boardItems.Count - 1; i >= 0; i--)
				PoolableBehaviour.ReturnPoolable(_boardItems[i]);
			for (int i = _slots.GetLength(0) - 1; i >= 0; i--)
				for (int j = _slots.GetLength(1) - 1; j >= 0; j--)
					Destroy(_slots[i, j].gameObject);
		}

		public BoardSlotBase GetSlot(Vector2Int v)
		{
			if (v.y < 0)
				return _bedrockSlot;
			else if (v.y > _gridSize.y - 1)
				return _ceilingSlot;
			else if (v.x < 0 || v.x > _gridSize.x - 1)
				return _sideBorderSlot;
			else
				return _slots[v.x, v.y];
		}

		public IEnumerable<BoardSlotBase> GetSlots(Vector2Int[] vs)
		{
			BoardSlotBase[] slots = new BoardSlotBase[vs.Length];
			for (int i = 0; i < vs.Length; i++)
				slots[i] = GetSlot(vs[i]);

			return slots;
		}

		public Vector3 GetWorldPos(Vector2Int gridPos)
		{
			return _zeroPos + _grid.CellToWorld((Vector3Int)gridPos);
		}

		public BoardSlotBase GetSlotOnTop(Vector3 position)
		{
			Vector2Int gridPos = (Vector2Int)_grid.WorldToCell(position - _zeroPos);

			return GetSlot(gridPos);
		}

		public void OnTapProcessed()
		{
			_failTracker.OnTapProcessed();
		}

		public void RecalculateSpecialShowers()
		{
			HashSet<BoardItem> visited = new() { };
			for (int i = 0; i < _boardItems.Count; i++)
			{
				Debug.Log($"traversing: {i}, at {_boardItems[i].GridPosSafe}", _boardItems[i].gameObject);
				if (!visited.Contains(_boardItems[i]))
				{
					List<BoardItem> block = BoardItemBlockProcessor.FindSpecial(_boardItems[i], visited);
					visited.UnionWith(block);
				}
			}
		}

		public void NewItem(BoardItem newItem)
		{
			_boardItems.Add(newItem);
		}

		public void CeilingBecameAvailable(Vector2Int availablePos)
		{
			_spawner.CeilingBecameAvailable(availablePos);
		}

		public class Dependency { }
	}
}