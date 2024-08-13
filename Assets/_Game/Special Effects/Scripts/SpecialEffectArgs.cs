using UnityEngine;

namespace Game.SpecialEffect
{
	// todo: Could be more a lot more flexible, generic dependency probably
	/// <summary>
	/// Don't forget to properly load necessary values.
	/// </summary>
	public class SpecialEffectArgs
	{
		public Vector2Int GridPos;
		// necessary for effects involving hor/ver rockets.
		public RocketEffect.FireDir FireDir;

		public SpecialEffectArgs(Vector2Int gridPos)
		{
			GridPos = gridPos;
		}
	}
}