using Game.Board;
using Game.Core;
using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;

namespace Game.UI
{
	public class LevelSelectButton : MonoBehaviour, IPointerClickHandler
	{
		[SerializeField] private TextMeshProUGUI _text;
		private bool _finished;

		private void Awake()
		{
			GameManager.I.GameLoaded += Refresh;
			ProgressionManager.I.ProgressionChanged += Refresh;
		}

		private void Refresh()
		{
			int num = ProgressionManager.I.CurrentLevelNumber;
			if (num > LevelLoader.I.LevelBundle.LevelAssets.Count)
			{
				_finished = true;
				_text.text = "Finished!";
			}
			else
			{
				_text.text = "Level " + num;
			}
		}

		public void OnPointerClick(PointerEventData eventData)
		{
			if (!_finished)
				GameScreenManager.I.SwitchGameScreen(GameScreenManager.I.LevelScreen);
		}
	}
}