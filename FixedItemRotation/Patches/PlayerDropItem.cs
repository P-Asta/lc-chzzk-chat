using GameNetcodeStuff;
using HarmonyLib;
using UnityEngine;

namespace FixedItemRotation
{

    [HarmonyPatch(typeof(PlayerControllerB))]
    internal class PlayerDropItemPatch
    {

        [HarmonyPatch("SetObjectAsNoLongerHeld")]
        [HarmonyPrefix]
        internal static void ItemDroppedOnShipPatch(bool droppedInElevator, bool droppedInShipRoom, ref Vector3 targetFloorPosition, ref GrabbableObject dropObject, ref int floorYRot)
        {
            if (droppedInShipRoom) { 
                floorYRot = 1;
            }
        }

        [HarmonyPatch(typeof(GrabbableObject))]
        [HarmonyPatch("DiscardItemClientRpc")]
        [HarmonyPostfix]
        internal static void ItemDroppedOnShipPatchPostfix(GrabbableObject __instance)
        {
            if (__instance.isInShipRoom)
            {
                string itemName = __instance.itemProperties.itemName;
                __instance.floorYRot = 1;
                Plugin.logger.LogInfo($"dropped {itemName}");
                __instance.customGrabTooltip = StackTooltip.UpdateAllTooltips(itemName);
            }
            else
            {
                __instance.customGrabTooltip = null;
            }
        }


    }

}