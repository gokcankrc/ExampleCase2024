using Game.Core;
using System;
using UnityEngine;

namespace Game.GoalSystem
{
	public abstract class LevelEndUI : MonoBehaviour
	{
		[SerializeField] protected CanvasGroup _canvasGroup;

		[NonSerialized] public bool Active;

		protected virtual void Awake()
		{
			GameScreenManager.I.GameScreenChanged += Deactivate;
		}

		protected virtual void Start()
		{
			Deactivate();
		}

		public virtual void Activate()
		{
			Active = true;

			_canvasGroup.alpha = 1f;
			_canvasGroup.interactable = Active;
			_canvasGroup.blocksRaycasts = Active;
		}

		public virtual void Deactivate()
		{
			Active = false;

			_canvasGroup.alpha = 0f;
			_canvasGroup.interactable = Active;
			_canvasGroup.blocksRaycasts = Active;
		}
	}
}
