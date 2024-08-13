using UnityEngine;

namespace Game.EntryPoint
{
	public class DIInitializer : MonoBehaviour
	{
		[SerializeField] private DependencyInjector _injector;

		private async void Start()
		{
			await _injector.InitializeApplication();
			_injector.OnApplicationLoaded();
		}
	}
}
