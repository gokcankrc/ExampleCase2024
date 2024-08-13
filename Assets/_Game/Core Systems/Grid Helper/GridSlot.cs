using System;
using UnityEngine;

namespace Game.GridSystem
{
	public abstract class GridSlot : MonoBehaviour
	{
		[NonSerialized] public Vector2Int GridPos;
		public abstract bool Available { get; }
		public SpriteRenderer SizeReference;

		public void Init(Vector2Int gridPos)
		{
			GridPos = gridPos;
		}
	}
}