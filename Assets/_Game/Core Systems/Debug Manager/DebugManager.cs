using Game.Board;
using Game.Core;
using Gokcan.Helpers;
using UnityEngine;

namespace Game
{
	public class DebugManager : SceneSingleton<DebugManager>
	{
#if UNITY_EDITOR
		[SerializeField] private KeyCode _mainScreenKey = KeyCode.F5;
		[SerializeField] private KeyCode _win = KeyCode.F7;
		[SerializeField] private KeyCode _slowDown = KeyCode.F6;
		private bool _slowedDown;

		private void Update()
		{
			if (Input.GetKeyDown(_mainScreenKey))
				GameScreenManager.I.SwitchGameScreen(GameScreenManager.I.MainScreen);

			if (Input.GetKeyDown(_win))
				LevelManager.I.DebugSucceed();

			if (Input.GetKeyDown(_slowDown))
			{
				Time.timeScale = _slowedDown ? 1f : 0.1f;
				_slowedDown = !_slowedDown;
			}
		}
#endif
	}
}
