using Gokcan.Helpers;
using System;
using System.Collections.Generic;
using UnityEngine;

namespace Game.Board
{
	// todo: Flexilibity could be improved
	public class GoalTracker : LazySingleton<GoalTracker>
	{
		public Action<List<Goal>> Activated;
		public Action Deactivated;
		public Action<List<Goal>> Refreshed;
		public Action GoalsFinished;

		[NonSerialized] public bool Won = false;

		protected Dictionary<GoalType, Goal> GoalsTable = new();
		protected List<Goal> Goals = new();
		protected bool _pendingRefresh;

		public void Activate()
		{
			Activated?.Invoke(Goals);
			Refresh();
		}

		public void Deactivate()
		{
			GoalsTable = new();
			Goals = new();
			Deactivated?.Invoke();
		}

		public bool Exists(GoalType type)
		{
			return GoalsTable.ContainsKey(type);
		}

		public void Register(Goal newGoal)
		{
			var type = newGoal.Type;
			if (!GoalsTable.TryGetValue(type, out var goal))
			{
				GoalsTable.Add(type, newGoal);
				Goals.Add(newGoal);
			}
			else
			{
				goal.Count += 1;
			}
		}

		public void LateUpdate()
		{
			if (_pendingRefresh)
			{
				_pendingRefresh = false;
				Refreshed?.Invoke(Goals);
				CheckZero();
			}
		}

		public void Refresh()
		{
			_pendingRefresh = true;
		}

		private void CheckZero()
		{
			bool zero = true;
			foreach (var goal in Goals)
				zero = zero && goal.Finished;

			if (zero)
			{
				Won = true;
				GoalsFinished?.Invoke();
			}
		}

		public void Increase(GoalType type)
		{
			if (GoalsTable.TryGetValue(type, out var goal))
				goal.Count += 1;
			else
				Debug.LogWarning("Tried to increase goal that's not in dictionary.");
		}

		public void Reduce(GoalType type)
		{
			if (GoalsTable.TryGetValue(type, out var goal))
				goal.Count -= 1;
			else
				Debug.LogWarning("Tried to reduce goal that's not in dictionary.");

			Refresh();
		}
	}

	public enum GoalType
	{
		Box = 1,
		Stone = 2,
		Vase = 3,
	}

	public class Goal
	{
		public bool Finished => Count == 0;
		public int Count;
		public GoalType Type;
		public Sprite Sprite;

		public Goal(GoalType type, Sprite sprite)
		{
			Count = 1;
			Type = type;
			Sprite = sprite;
		}
	}
}