using UnityEngine;

namespace Game.SpecialEffect
{
	[CreateAssetMenu(fileName = "Special Effect Settings", menuName = "Board/Special Effect Visuals")]
	public class SpecialEffectSettingBundle : ScriptableObject
	{
		public RocketEffect.Setting RocketSetting;
		public ExplosionEffect.Setting ExplosionSetting;
	}
}