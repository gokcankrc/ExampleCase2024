using Game.Board;
using Game.Core;
using System.Collections.Generic;
using UnityEngine;

namespace Game.GoalSystem
{
	public class GoalDisplayHandler : MonoBehaviour
	{
		[SerializeField] private List<GoalDisplay> displays;

		private void Awake()
		{
			GameManager.I.GameLoaded += Attach;
		}

		private void Attach()
		{
			var goalTracker = LevelManager.I.GoalTracker;

			goalTracker.Activated += OnActivated;
			goalTracker.Deactivated += OnDeactivated;
			goalTracker.Refreshed += OnRefresh;
		}

		private void OnActivated(List<Goal> goals)
		{
			for (int i = 0; i < goals.Count; i++)
				displays[i].Activate(goals[i]);
			for (int i = goals.Count; i < displays.Count; i++)
				displays[i].Deactivate();
		}

		private void OnDeactivated()
		{
			foreach (var display in displays)
				display.Deactivate();
		}

		private void OnRefresh(List<Goal> goals)
		{
			for (int i = 0; i < goals.Count; i++)
				displays[i].Refresh(goals[i]);
		}
	}
}
