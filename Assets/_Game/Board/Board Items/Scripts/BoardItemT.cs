
namespace Game.Board
{
	public abstract class BoardItemT<T> : BoardItem where T : BoardItem.Dependency
	{
		public void Initialize(T dep)
		{
			if (!Initialized)
				InitInternal(dep);

			ActivateInternal(dep);
		}

		protected virtual void InitInternal(T dep)
		{
			base.InitBase(dep);
		}

		protected virtual void ActivateInternal(T dep)
		{
			base.ActivateBase(dep);
		}
	}
}