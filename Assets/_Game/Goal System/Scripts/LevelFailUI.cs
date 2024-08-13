using UnityEngine;

namespace Game.GoalSystem
{
	public class LevelFailUI : LevelEndUI
	{
		[SerializeField] private float _lerpSpeed = 10f;

		public override void Activate()
		{
			base.Activate();

			transform.localScale = Vector3.zero;
		}

		private void Update()
		{
			if (!Active) return;

			transform.localScale = Vector3.Lerp(transform.localScale, Vector3.one, Time.deltaTime * _lerpSpeed);
		}
	}
}
