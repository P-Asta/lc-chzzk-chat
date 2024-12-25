using BepInEx.Logging;
using GameNetcodeStuff;
using HarmonyLib;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using Unity.Netcode;
using UnityEngine;

namespace FixedItemRotation
{

    [HarmonyPatch(typeof(PlayerControllerB))]
    internal class PlayerPickupItem
    {

        [HarmonyPatch("GrabObjectClientRpc")]
        [HarmonyPostfix]
        internal static void GrabObjectPatch(bool grabValidated, ref NetworkObjectReference grabbedObject)
        {

            NetworkObject networkObject;
            grabbedObject.TryGet(out networkObject);

            if (networkObject == null) return;

            GrabbableObject grabbableObject = networkObject.GetComponent<GrabbableObject>();

            if (grabbableObject == null) return;

            string itemName = grabbableObject.itemProperties.itemName;
            grabbableObject.customGrabTooltip = StackTooltip.UpdateAllTooltips(itemName);

        }

    }
}