namespace Game.SpecialEffect
{
	public abstract class SpecialEffectT<T> : SpecialEffect where T : SpecialEffect.Dependency
	{
		public virtual void Execute(T dep) { ExecuteBase(dep); }
	}
}