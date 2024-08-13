using Game.Board;
using Gokcan.PoolSystem;
using UnityEngine;

namespace Game.SpecialEffect
{
	public abstract class SpecialEffect : PoolableBehaviour
	{
		protected override Transform DefaultParent => SpecialEffectParent.Tr;

		protected void ExecuteBase(Dependency dep)
		{
			Vector3 offset = Vector3.back;
			transform.position = dep.Pos + offset;
		}

		public virtual void GetDestroyed()
		{
			ReturnPoolable(this);
		}

		public class Dependency
		{
			public Vector2Int GridPos;
			public Vector3 Pos => BoardManager.I.GetWorldPos(GridPos);

			public Dependency(Vector2Int gridPos)
			{
				GridPos = gridPos;
			}
		}
	}
}