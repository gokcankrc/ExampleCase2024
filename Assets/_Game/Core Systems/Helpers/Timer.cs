using System;
using UnityEngine;
using UnityEngine.Experimental.GlobalIllumination;

namespace Gokcan.Helpers
{
	[Serializable]
	public class Timer
	{
		public Action OnJustDrained;

		public float Max;
		public bool Loop;

		public float Ratio1to0 => Current / Max;
		public float Ratio0to1 => 1 - (Current / Max);
		public float TimePassed => Max - Current;

		public float Current { get; set; }
		public bool JustDrained { get; set; }
		public bool IsDrained { get; set; }

		public Timer(float maxAndCurrent)
		{
			Max = maxAndCurrent;
			Current = maxAndCurrent;

			OnJustDrained = null;
			Loop = false;
			JustDrained = false;
			IsDrained = false;
		}

		public Timer(float max, float current = 0, bool loop = false)
		{
			Max = max;
			Loop = loop;
			Current = current;
			if (loop == true && current == 0)
				Current = max;

			OnJustDrained = null;
			JustDrained = false;
			IsDrained = false;
		}

		public void Update(float deltaTime)
		{
			bool isWayTooBelowZero = Current < -deltaTime;
			Current -= deltaTime;

			JustDrained = false;

			if (IsDrained == false && Current <= 0)
			{
				JustDrained = true;
				if (Loop)
				{
					if (isWayTooBelowZero)
						SetToMax();
					else
						SetToMaxAdditive();
				}

				OnJustDrained?.Invoke();
			}

			IsDrained = Current <= 0;
		}

		public void SetToMax()
		{
			Current = Max;
			JustDrained = false;
			IsDrained = false;
		}

		public void SetToMaxAdditive()
		{
			// For more consistently-timed loops
			Current += Max;
			JustDrained = false;
			IsDrained = false;
		}

		public void ClampedChange(float change)
		{
			Current = Mathf.Clamp(Current + change, 0, Max);
		}

		public void Clamp()
		{
			Current = Mathf.Clamp(Current, 0, Max);
		}
	}
}