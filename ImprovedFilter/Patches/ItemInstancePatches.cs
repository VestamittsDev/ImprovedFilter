using HarmonyLib;
using UnityEngine;

namespace ImprovedFilter
{
	[HarmonyPatch(typeof(ItemInstance))]
	public class ItemInstancePatches
	{
		[HarmonyPrefix]
		[HarmonyPatch("DropItem")]
		public static bool DropItemPrefix(Vector2 position, Vector2 floorPosition, Vector2 force, ItemInstance __instance)
		{
			// Do not apply to items being added directly to inventory
			string itemDesc = __instance.Description;
			bool addToInventory = itemDesc.EndsWith("#");
			if (addToInventory)
			{
				// Remove addToInventory mark
				Traverse.Create(__instance.Item).Field("description").SetValue(itemDesc.Substring(0, itemDesc.Length - 1));

				return true;
			}

			// Do not apply to items dropped from inventory
			bool inInventory = __instance.State.HasFlag(ItemInstanceState.InInventory);
			if (inInventory)
			{
				return true;
			}

			// Only apply to equips and glyphs
			if (!IsEquipment(__instance))
			{
				return true;
			}

			// Check against current item filter, if it does not pass destroy and do not execute DropItem
			bool passedFilter = Singleton<ItemFilterManager>.instance.PassedFilter(__instance);
			if (!passedFilter)
			{
				__instance.DestroyItem();
				return false;
			}

			return true;
		}

		#region Helpers
		private static bool IsEquipment(ItemInstance item)
		{
			bool isMainHand = item.Attributes.HasFlag(ItemAttributes.MainHand);
			bool isOffHand = item.Attributes.HasFlag(ItemAttributes.OffHand);
			bool isHead = item.Attributes.HasFlag(ItemAttributes.Head);
			bool isChest = item.Attributes.HasFlag(ItemAttributes.Chest);
			bool isAmulet = item.Attributes.HasFlag(ItemAttributes.Amulets);
			bool isGlyph = item.Attributes.HasFlag(ItemAttributes.SimpleGlyph);
			bool isCompoundGlyph = item.Attributes.HasFlag(ItemAttributes.CompoundGlyph);

			return isMainHand || isOffHand || isHead || isChest || isAmulet || isGlyph || isCompoundGlyph;
		}
		#endregion
	}
}
