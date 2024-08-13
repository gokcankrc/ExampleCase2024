using Gokcan.PoolSystem;
using System;
using static Game.SpecialEffect.RocketEffect;

namespace Game.Board
{
	public static class BoardItemMaker
	{
		public static class Cube
		{
			public static BoardItem Green() => Specific(CubeType.Green);
			public static BoardItem Red() => Specific(CubeType.Red);
			public static BoardItem Blue() => Specific(CubeType.Blue);
			public static BoardItem Yellow() => Specific(CubeType.Yellow);

			public static CubeItem Random()
			{
				CubeItem.Setting setting = BoardSettings.I.BoardItems.GetRandomCubeSetting();
				return Make(setting);
			}

			public static CubeItem Specific(CubeType type)
			{
				CubeItem.Setting setting = BoardSettings.I.BoardItems.GetCubeSetting(type);
				return Make(setting);
			}

			private static CubeItem Make(CubeItem.Setting setting)
			{
				var newItemDep = new CubeItem.Dependency(
					setting: setting);
				CubeItem newItem = PoolableBehaviour.GetInstance<CubeItem>();
				newItem.Initialize(newItemDep);

				return newItem;
			}
		}

		public static class Special
		{
			public static BoardItem FromType(SpecialType type)
			{
				return type switch
				{
					SpecialType.Rocket => RocketRandom(),
					SpecialType.Tnt => Tnt(),
					_ => null,
				};
			}

			public static TntItem Tnt()
			{
				TntItem newItem = PoolableBehaviour.GetInstance<TntItem>();
				newItem.Initialize(new());
				return newItem;
			}

			public static RocketItem RocketHorizontal()
			{
				return MakeRocket(FireDir.horizontal);
			}

			public static RocketItem RocketVertical()
			{
				return MakeRocket(FireDir.vertical);
			}

			public static RocketItem RocketRandom()
			{
				return MakeRocket(getRandomFireDir());
			}

			private static FireDir getRandomFireDir()
			{
				Array values = Enum.GetValues(typeof(FireDir));
				int random = UnityEngine.Random.Range(0, values.Length);
				return (FireDir)values.GetValue(random);
			}

			public static RocketItem MakeRocket(FireDir fireDir)
			{
				RocketItem newItem = PoolableBehaviour.GetInstance<RocketItem>();
				newItem.Initialize(new(fireDir));
				return newItem;
			}
		}

		public static class Obstacle
		{
			public static BoxItem Box()
			{
				BoxItem newItem = PoolableBehaviour.GetInstance<BoxItem>();
				newItem.Initialize(new());
				return newItem;
			}

			public static VaseItem Vase()
			{
				VaseItem newItem = PoolableBehaviour.GetInstance<VaseItem>();
				newItem.Initialize(new());
				return newItem;
			}

			public static StoneItem Stone()
			{
				StoneItem newItem = PoolableBehaviour.GetInstance<StoneItem>();
				newItem.Initialize(new());
				return newItem;
			}
		}
	}
}