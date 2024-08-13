using Gokcan.Helpers;

namespace Game.Board
{
	public class BoardSettings : SceneSingleton<BoardSettings>
	{
		public BoardItemSettingBundle BoardItems;
		public ItemBlockRule ItemBlockRules;
		public FallingGravitySettings GravitySettings;
	}
}
