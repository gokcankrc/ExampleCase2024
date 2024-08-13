using UnityEngine;

namespace Game.Board
{
	public interface ISequenceTarget
	{
		public Transform Transform { get; }

		public Vector3 SetPosNoZCorrect { set; }
		public Vector3 Pos { get; set; }
		public Vector2Int GridPos { get; }

		public void OnEnteredSequence();
		public void OnFinalizedSequence(bool isEndPoint);
	}
}