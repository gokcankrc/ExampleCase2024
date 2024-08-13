using Game.Board;
using UnityEngine;

namespace Game.GoalSystem
{
	// todo: A proper window system
	[RequireComponent(typeof(CanvasGroup))]
	public class LevelEndUIHandler : MonoBehaviour
	{
		[SerializeField] private LevelEndUI SuccessUIHandler;
		[SerializeField] private LevelEndUI FailUIHandler;

		private CanvasGroup _canvasGroup;

		private void Awake()
		{
			_canvasGroup = GetComponent<CanvasGroup>();
			_canvasGroup.alpha = 1.0f;
			_canvasGroup.interactable = true;
			_canvasGroup.blocksRaycasts = true;

			LevelManager.I.LevelSucceded += OnSuccess;
			LevelManager.I.LevelActivated += Attach;
		}

		private void Attach(LevelManager levelManager)
		{
			levelManager.FailTracker.Failed += OnFail;
		}

		private void OnFail()
		{
			FailUIHandler.Activate();
		}

		private void OnSuccess()
		{
			SuccessUIHandler.Activate();
		}
	}
}
