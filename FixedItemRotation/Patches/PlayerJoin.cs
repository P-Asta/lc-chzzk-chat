using BepInEx;
using BepInEx.Configuration;
using BepInEx.Logging;
using FixedItemRotation;
using GameNetcodeStuff;
using HarmonyLib;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using Unity.Netcode;
using UnityEngine;
using UnityEngine.UIElements;

namespace ItemDeclutter
{

    [HarmonyPatch(typeof(StartOfRound))]
    internal class ShipLoadItems
    {

        [HarmonyPatch("LoadShipGrabbableItems")]
        [HarmonyPostfix]
        internal static void LoadShipItemsPatch()
        {
            GameObject ship = GameObject.Find("/Environment/HangarShip");
            var ItemsOnShip = ship.GetComponentsInChildren<GrabbableObject>();

            foreach (var item in ItemsOnShip)
            {
                Plugin.logger.LogDebug($"Found {item.name}");
                StackTooltip.UpdateAllTooltips(item.name);
            }

        }


    }

}