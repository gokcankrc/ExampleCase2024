using UnityEngine;

namespace Game.Board
{
	public class FixedGravity : GravityComponent
	{
		public override bool IsStatic => true;

		public FixedGravity(BoardItem connectedItem) : base(connectedItem) { }

		public override void OnBelowEmpty()
		{
			return;
		}

		public override void StartFalling()
		{
			Debug.LogError("Fixed gravity is told to fall.", _connectedItem.gameObject);
			return;
		}

		public override void Update(float deltaTime)
		{
			return;
		}
	}
}