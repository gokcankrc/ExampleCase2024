using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AddressableAssets;

namespace Game.Board
{
	[CreateAssetMenu(fileName = "Level _boardItems", menuName = "Board/Level _boardItems")]
	public class LevelBundle : ScriptableObject
	{
		public List<AssetReference> LevelAssets;
	}
}