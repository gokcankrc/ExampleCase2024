using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace Game.Board
{
	public static class BoardItemBlockProcessor
	{
		public static bool ExecuteColor(BoardItem blockOrigin)
		{
			// todo: small optimization, since specials are calculated already,
			// hold the calculater block and use that instead of calculating a block here.
			// problem: real time actions would introduce bugs.
			List<BoardItem> adjacentItems = new();
			List<BoardItem> block = new() { blockOrigin };

			TraverseSameColor(
				blockOrigin: blockOrigin,
				block: block,
				adjacentItems: adjacentItems);

			var blockRules = BoardSettings.I.ItemBlockRules;
			return finalize(blockRules, blockOrigin, block, adjacentItems);

			static bool finalize(
				ItemBlockRule blockRules,
				BoardItem blockOrigin,
				List<BoardItem> block,
				List<BoardItem> adjacent)
			{
				if (block.Count < blockRules.MinRequiredToPop) return false;

				FindTargetRule(blockRules, block, out var targetRule);

				if (targetRule != null)
				{
					var targetBlock = block.Cast<ISequenceTarget>().ToList();
					var sequence = new MergeSequence(blockOrigin, targetBlock, callback);
					sequence.Execute();

					void callback()
					{
						var newItem = BoardItemMaker.Special.FromType(targetRule.ItemType);
						var spawnSlot = blockOrigin.ConnectedSlot;
						BoardItem.SwapItem(newItem, spawnSlot);
					}
				}
				else
				{
					foreach (var item in block)
						item.DirectDamageComponent.TakeDamage();
				}

				foreach (var item in adjacent)
					item.AdjacentDamageComponent.TakeDamage();
				return true;
			}
		}

		public static List<BoardItem> FindSpecial(BoardItem blockOrigin, HashSet<BoardItem> visited)
		{
			List<BoardItem> block = new() { blockOrigin };
			if (!blockOrigin.IsActive)
			{
				Debug.Log("not active traversal");
				return block;
			}
			if (!blockOrigin.IsColoredCube(out _)) return block;

			TraverseSameColor(
				blockOrigin: blockOrigin,
				block: block,
				unnecessaries: visited);

			var blockRules = BoardSettings.I.ItemBlockRules;
			FindTargetRule(blockRules, block, out var targetRule);

			foreach (var item in block)
				item.OnBlockRefresh(targetRule, block);

			Debug.Log($"Block: {block.Count}, {blockOrigin.GridPosSafe}", blockOrigin.gameObject);
			return block;
		}

		private static void TraverseSameColor(
			BoardItem blockOrigin,
			List<BoardItem> block,
			HashSet<BoardItem> unnecessaries = null,
			List<BoardItem> adjacentItems = null)
		{
			HashSet<BoardItem> visited = new() { blockOrigin };
			if (unnecessaries == null)
				unnecessaries = new() { };

			bool addAdjacent = adjacentItems != null;
			Queue<BoardItem> itemProcessQueue = new();
			itemProcessQueue.Enqueue(blockOrigin);
			while (itemProcessQueue.Count > 0)
			{
				var currentItem = itemProcessQueue.Dequeue();
				var neighbors = currentItem.Neighbors;
				foreach (var neighbor in neighbors)
				{
					if (neighbor != null && !visited.Contains(neighbor) && !unnecessaries.Contains(neighbor))
					{
						visited.Add(neighbor);
						bool sameColor = blockOrigin.CompareColor(neighbor);
						if (sameColor)
						{
							itemProcessQueue.Enqueue(neighbor);
							block.Add(neighbor);
						}
						else if (addAdjacent)
						{
							adjacentItems.Add(neighbor);
						}
					}
				}
			}
		}

		private static void FindTargetRule(
			ItemBlockRule blockRules,
			List<BoardItem> block,
			out ItemBlockRule.SpecialMakerRule targetRule)
		{
			int maxRuleRequirement = 0;
			targetRule = null;
			foreach (var rule in blockRules.SpecialMakerRules)
			{
				int required = rule.MinRequired - 1;

				if (block.Count > required && maxRuleRequirement < required)
				{
					maxRuleRequirement = required;
					targetRule = rule;
				}
			}
		}

		public static void ExecuteTnt(Vector2Int gridPos, int thickness)
		{
			List<BoardItem> block = new();

			Vector2Int size = Vector2Int.one * ((thickness * 2) + 1);
			Vector2Int start = gridPos - (Vector2Int.one * thickness);
			for (int x = 0; x < size.x; x++)
			{
				for (int y = 0; y < size.y; y++)
				{
					var itemToAdd = BoardManager.I.GetSlot(start + new Vector2Int(x, y)).ConnectedItem;
					if (itemToAdd != null)
						block.Add(itemToAdd);
				}
			}

			finalize(block);

			static void finalize(List<BoardItem> block)
			{
				foreach (var item in block)
					item.DirectDamageComponent.TakeDamage();
			}
		}
	}
}