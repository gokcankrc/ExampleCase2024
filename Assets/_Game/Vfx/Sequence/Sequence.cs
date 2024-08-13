using Gokcan.Helpers;
using System;

namespace Game.Board
{
	public abstract class Sequence
	{
		public Action Callback;

		protected abstract float _maxGet { get; }
		protected Timer _timer = new(0);

		public Sequence() { }

		public virtual void Execute()
		{
			_timer.Max = _maxGet;
			_timer.SetToMax();
			SequenceManager.I.Execute(this);
		}

		public virtual void Update(float deltaTime)
		{
			_timer.Update(deltaTime);
			if (_timer.JustDrained)
				Finilize();
		}

		public virtual void Finilize()
		{
			SequenceManager.I.Remove(this);
			Callback?.Invoke();
		}
	}
}