using UnityEngine;

namespace Game.Board
{
	public class CeilingSpawner : MonoBehaviour
	{
		private BoardManager _boardManager;
		[SerializeField] private bool _spawnsActive;
		[SerializeField] private bool _spawnOne;

		public void Init(BoardManager boardManager)
		{
			_boardManager = boardManager;
		}

		private void SpawnAbove(BoardSlotBase slot)
		{
			if (!_spawnsActive)
				return;
			Vector3 spawnLocation = slot.AbovePos;
			SpawnCube(slot, spawnLocation);
		}

		private void SpawnCube(BoardSlotBase slot, Vector3 spawnLocation)
		{
			CubeItem newItem = BoardItemMaker.Cube.Random();
			newItem.Pos = spawnLocation;
			BoardItem.SetFallingCouple(newItem, slot);
		}

		public void ActivateSpawns()
		{
			_spawnsActive = true;
			if (_spawnOne)
			{
				var slot = _boardManager.TopmostRow[0];
				if (slot.Available)
					SpawnAbove(slot);
			}
			else
			{
				foreach (var slot in _boardManager.TopmostRow)
					if (slot.Available)
						SpawnAbove(slot);
			}
		}

		public void DeactivateSpawns()
		{
			_spawnsActive = false;
		}

		public void CeilingBecameAvailable(Vector2Int availablePos)
		{
			SpawnAbove(_boardManager.GetSlot(availablePos));
		}
	}
}