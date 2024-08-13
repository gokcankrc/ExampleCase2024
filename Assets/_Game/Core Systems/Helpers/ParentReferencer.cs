using UnityEngine;

namespace Gokcan.Helpers
{
	public abstract class ParentReferencer<T> : MonoBehaviour where T : MonoBehaviour
	{
		public static Transform Tr;

		protected virtual void Awake()
		{
			Tr = transform;
		}
	}
}