using UnityEngine;

namespace Gokcan.Helpers
{
	public static class CanvasExtensions
	{
		public static Matrix4x4 GetCanvasMatrix(this Canvas _Canvas)
		{
			RectTransform rectTr = _Canvas.transform as RectTransform;
			Matrix4x4 canvasMatrix = rectTr.localToWorldMatrix;
			canvasMatrix *= Matrix4x4.Translate(-rectTr.sizeDelta / 2);
			return canvasMatrix;
		}

		public static void ToggleOn(this CanvasGroup canvasGroup)
		{
			canvasGroup.alpha = 1;
			canvasGroup.interactable = true;
			canvasGroup.blocksRaycasts = true;
		}

		public static void ToggleOff(this CanvasGroup canvasGroup)
		{
			canvasGroup.alpha = 0;
			canvasGroup.interactable = false;
			canvasGroup.blocksRaycasts = false;
		}
	}
}