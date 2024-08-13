using Game.Core;
using Gokcan.Helpers;
using System.Collections.Generic;
using UnityEngine;

namespace Game.Board
{
	public class SequenceManager : LazySingleton<SequenceManager>
	{
		private List<Sequence> _sequences = new();
		private List<Sequence> _toBeRemoved = new();

		protected override void Awake()
		{
			base.Awake();

			GameScreenManager.I.GameScreenChanged += RemoveAll;
		}

		private void RemoveAll()
		{
			_sequences = new();
		}

		public void Execute(Sequence sequence)
		{
			_sequences.Add(sequence);
			LevelManager.ActiveActionCounter += 1;
		}

		public void Remove(Sequence sequence)
		{
			_toBeRemoved.Add(sequence);
			LevelManager.ActiveActionCounter -= 1;
		}

		private void Update()
		{
			foreach (var sequence in _toBeRemoved)
				_sequences.Remove(sequence);
			_toBeRemoved = new();

			for (int i = _sequences.Count - 1; i >= 0; i--)
				_sequences[i].Update(Time.deltaTime);
		}
	}
}