using Game.Core;
using UnityEngine;

namespace Game.UI
{
	public class BackToMenuButton : MonoBehaviour
	{
		public void Clicked()
		{
			GameScreenManager.I.SwitchToMainScreen();
		}
	}
}
