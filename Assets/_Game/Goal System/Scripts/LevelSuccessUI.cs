using Game.Core;
using Gokcan.Helpers;
using UnityEngine;

namespace Game.GoalSystem
{
	public class LevelSuccessUI : LevelEndUI
	{
		[SerializeField] private float _lerpSpeed = 10f;
		[SerializeField, Range(0f, 1f)] private float _startShrinking = 0.7f;
		[SerializeField] private GameObject _canvasSpaceLink;
		[SerializeField] private GameObject _worldSpace;
		[SerializeField] private Timer _backToMenuTimer;

		public override void Activate()
		{
			base.Activate();

			_canvasSpaceLink.transform.localScale = Vector3.zero;
			_worldSpace.SetActive(true);
			_backToMenuTimer.SetToMax();
		}

		public override void Deactivate()
		{
			base.Deactivate();

			_worldSpace.SetActive(false);
		}

		private void Update()
		{
			if (!Active) return;
			Vector3 target = _backToMenuTimer.Ratio1to0 > _startShrinking ? Vector3.one : Vector3.zero;
			_canvasSpaceLink.transform.localScale = Vector3.Lerp(_canvasSpaceLink.transform.localScale, target, Time.deltaTime * _lerpSpeed);

			_backToMenuTimer.Update(Time.deltaTime);
			if (_backToMenuTimer.JustDrained)
				GameScreenManager.I.SwitchToMainScreen();
		}
	}
}
