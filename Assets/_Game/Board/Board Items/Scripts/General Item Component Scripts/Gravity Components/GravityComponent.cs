using System;

namespace Game.Board
{
	public abstract class GravityComponent : BoardItemComponent
	{
		public abstract bool IsStatic { get; }
		public Action Moved;
		public Action Stopped;
		public State FallState = State.Stopped;

		public GravityComponent(BoardItem connectedItem) : base(connectedItem) { }

		public abstract void OnBelowEmpty();
		public abstract void StartFalling();
		public abstract void Update(float deltaTime);

		public enum State
		{
			Falling,
			Stopped
		}

		public enum SpawnType
		{
			Rested,
			Falling,
		}
	}
}