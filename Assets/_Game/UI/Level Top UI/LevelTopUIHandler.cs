using Game.Board;
using Gokcan.Helpers;
using UnityEngine;

namespace Game.UI
{
	[RequireComponent(typeof(CanvasGroup))]
	public class LevelTopUIHandler : MonoBehaviour
	{
		private CanvasGroup _canvasGroup;

		private void Awake()
		{
			_canvasGroup = GetComponent<CanvasGroup>();
			_canvasGroup.ToggleOff();

			var levelManager = LevelManager.I;
			levelManager.LevelActivated += OnActivated;
			levelManager.LevelDeactivated += OnDeactivated;
		}

		private void OnActivated(LevelManager levelManager)
		{
			_canvasGroup.ToggleOn();
		}

		private void OnDeactivated(LevelManager levelManager)
		{
			_canvasGroup.ToggleOff();
		}
	}
}
