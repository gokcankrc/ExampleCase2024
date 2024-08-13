using UnityEngine;

namespace Game.Vfx
{
	public abstract class Effect
	{
		protected Transform _transform;
		protected SpriteRenderer _renderer;

		public virtual void Start(Transform transform, SpriteRenderer renderer)
		{
			_transform = transform;
			_renderer = renderer;
		}

		public abstract void Update(float ratio);
	}
}