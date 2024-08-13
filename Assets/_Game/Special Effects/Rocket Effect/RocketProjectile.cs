using Game.Board;
using Gokcan.Helpers;
using Gokcan.PoolSystem;
using UnityEngine;

namespace Game.SpecialEffect
{
	public class RocketProjectile : PoolableBehaviour
	{
		[SerializeField] private Timer _flightTimer = new(maxAndCurrent: 1);
		[SerializeField] private SpriteRenderer _spriteRenderer;
		[SerializeField] private ParticleSystem _particleSystem;

		protected override Transform DefaultParent => SpecialEffectParent.Tr;

		private float _speed;
		private Vector3 _direction;
		private bool _isFlying = false;
		private BoardItem _recentlyDamagedItem;
		private RocketEffect _effect;

		private void Awake()
		{
			_speed = SpecialEffectSettings.I.Settings.RocketSetting.Speed;
		}

		public void FireOff(RocketEffect effect, Vector3 pos, Vector3 dir, Sprite sprite)
		{
			_effect = effect;
			_direction = dir;
			_flightTimer.SetToMax();
			_isFlying = true;
			_spriteRenderer.sprite = sprite;
			transform.position = pos + Vector3.back;
			_particleSystem.Play();
			_recentlyDamagedItem = null;
		}

		private void FixedUpdate()
		{
			if (!_isFlying) return;

			_flightTimer.Update(Time.fixedDeltaTime);
			transform.position += _direction * _speed * Time.fixedDeltaTime;

			if (_flightTimer.IsDrained)
			{
				_particleSystem.Stop();
				_isFlying = false;
				ReturnPoolable(this);
				_effect.GetDestroyed();
			}

			var slot = BoardManager.I.GetSlotOnTop(transform.position);
			var connectedItem = slot.ConnectedItem;
			if (connectedItem != null && _recentlyDamagedItem != connectedItem)
			{
				connectedItem.DirectDamageComponent.TakeDamage();
				_recentlyDamagedItem = connectedItem;
			}
		}
	}
}