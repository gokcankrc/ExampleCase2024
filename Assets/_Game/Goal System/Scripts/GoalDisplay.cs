using Game.Board;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace Game.GoalSystem
{
	public class GoalDisplay : MonoBehaviour
	{
		[SerializeField] private Image _goal;
		[SerializeField] private Image _okTick;
		[SerializeField] private TextMeshProUGUI _text;

		public void Activate(Goal goal)
		{
			_goal.enabled = true;
			_goal.sprite = goal.Sprite;
			_okTick.enabled = false;
			_text.text = "";
		}

		public void Deactivate()
		{
			_goal.enabled = false;
			_okTick.enabled = false;
			_text.text = "";
		}

		public void Refresh(Goal goal)
		{
			if (goal.Finished)
			{
				_text.text = "";
				_okTick.enabled = true;
			}
			else
			{
				_text.text = "" + goal.Count;
			}
		}
	}
}
