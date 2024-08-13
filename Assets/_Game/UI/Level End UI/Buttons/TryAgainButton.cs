using Game.Core;
using UnityEngine;

namespace Game.UI
{
	public class TryAgainButton : MonoBehaviour
	{
		public void Clicked()
		{
			GameScreenManager.I.ReloadLevel();
		}
	}
}
