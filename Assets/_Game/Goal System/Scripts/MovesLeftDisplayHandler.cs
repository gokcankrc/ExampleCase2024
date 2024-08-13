using Game.Board;
using TMPro;
using UnityEngine;

namespace Game.GoalSystem
{
	public class MovesLeftDisplayHandler : MonoBehaviour
	{
		[SerializeField] private TextMeshProUGUI _movesLeftText;

		private void Awake()
		{
			LevelManager.I.LevelActivated += AttachToFailTracker;
		}

		private void AttachToFailTracker(LevelManager sender)
		{
			sender.FailTracker.ConditionUpdated += OnRefresh;
		}

		private void OnRefresh(FailTracker.ConditionUpdatedArgs args)
		{
			_movesLeftText.text = args.Text;
		}
	}
}
