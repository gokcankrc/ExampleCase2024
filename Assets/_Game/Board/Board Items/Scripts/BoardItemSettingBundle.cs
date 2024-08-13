using System.Collections.Generic;
using UnityEngine;

namespace Game.Board
{
	[CreateAssetMenu(fileName = "Board Item Settings", menuName = "Board/Board Item Visuals")]
	public class BoardItemSettingBundle : ScriptableObject
	{
		public List<CubeItem.Setting> CubeSettings;
		public RocketItem.Setting RocketSetting;

		public CubeItem.Setting GetRandomCubeSetting()
		{
			return CubeSettings[Random.Range(0, CubeSettings.Count - 1)];
		}

		public CubeItem.Setting GetCubeSetting(CubeType type)
		{
			return CubeSettings.Find(x => x.Type == type);
		}
	}
}