using HarmonyLib;
using UnityEngine;

namespace ImprovedFilter
{
	[HarmonyPatch(typeof(ItemInstance))]
	public class ImprovedFilterPatch
	{
		[HarmonyPrefix]
		[HarmonyPatch("DropItem")]
		public static void DropItemPostfix(Vector2 position, Vector2 floorPosition, Vector2 force, ItemInstance __instance)
		{
			bool inInventory = __instance.State.HasFlag(ItemInstanceState.InInventory);
			if (inInventory) return;

			bool passedFilter = Singleton<ItemFilterManager>.instance.PassedFilter(__instance);
			if (!passedFilter) __instance.DestroyItem();
		}
	}
}
