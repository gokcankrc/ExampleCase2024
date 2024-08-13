using System;

namespace Game.Board
{
	public abstract class FailTracker
	{
		public Action Failed;
		public Action<ConditionUpdatedArgs> ConditionUpdated;

		protected bool _aboutToLose;

		public abstract void Init(LevelManager levelManager);
		public abstract void Terminate(LevelManager levelManager);
		public abstract void OnTapProcessed();

		public void CanLoseNow()
		{
			Failed?.Invoke();
		}

		public class Dependency
		{
			public Level Level;
		}

		public class ConditionUpdatedArgs
		{
			public string Text;

			public ConditionUpdatedArgs(string conditionText)
			{
				Text = conditionText;
			}
		}
	}
}