using HarmonyLib;
using System;
using UnityEngine;

namespace ImprovedFilter.Patches
{
	[HarmonyPatch(typeof(LootEntry))]
	public class LootEntryPatches
	{
		[HarmonyPrefix]
		[HarmonyPatch("SpawnLoot", new Type[] { typeof(CharacterContainer), typeof(Item), typeof(int), typeof(int), typeof(float), typeof(bool), typeof(Vector2), typeof(Vector2), typeof(Vector2) })]
		public static bool SpawnLootPrefix(CharacterContainer receiver, Item lootItem, int stackSize, int level, float additionalMagicChance, bool addToInventory, Vector2 position, Vector2 floorPosition, Vector2 force)
		{
			if (addToInventory)
			{
				Traverse.Create(lootItem).Field("description").SetValue($"{lootItem.Description}#");
			}

			return true;
		}
	}
}
