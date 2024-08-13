using Game.Core;
using Gokcan.Helpers;
using System.Threading.Tasks;
using UnityEngine;

namespace Game.Board
{
	public class MainMenuHandler : SceneSingleton<MainMenuHandler>, IGameScreen
	{
		[SerializeField] private CanvasGroup CanvasGroup;
		public bool IsInTransition => false;

		public Task ActivateGameScreen()
		{
			CanvasGroup.gameObject.SetActive(true);
			CanvasGroup.alpha = 1;
			CanvasGroup.interactable = true;
			return Task.CompletedTask;
		}

		public void DeactivateGameScreen()
		{
			CanvasGroup.gameObject.SetActive(false);
			CanvasGroup.alpha = 0f;
			CanvasGroup.interactable = false;
		}
	}
}