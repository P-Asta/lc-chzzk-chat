using BepInEx;
using HarmonyLib;
namespace FixedItemRotation
{
    [BepInPlugin("FixedItemRotation", "FixedItemRotation", "0.0.1")]
    public class Plugin : BaseUnityPlugin
    {
        private readonly Harmony harmony = new("FixedItemRotation");

        private void Awake()
        {
            harmony.PatchAll();
            // Plugin startup logic
            Logger.LogInfo($"Plugin FixedItemRotation is loaded!");

        }
    }
}
