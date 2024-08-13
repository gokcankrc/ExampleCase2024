using System.Threading.Tasks;

namespace Game.Core
{
	public interface IGameScreen
	{
		public abstract bool IsInTransition { get; }
		public abstract void DeactivateGameScreen();
		public abstract Task ActivateGameScreen();
	}
}