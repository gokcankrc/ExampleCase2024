using UnityEngine;

namespace Game.Board
{
	[CreateAssetMenu(fileName = "Gravity Settings", menuName = "Board/Gravity Settings")]
	public class FallingGravitySettings : ScriptableObject
	{
		public float Speed = 10f;
		public float EnoughDelayDistance = 0.5f;
	}
}