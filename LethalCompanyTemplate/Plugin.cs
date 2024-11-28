using BepInEx;
using BepInEx.Logging;
using HarmonyLib;
namespace FixedItemRotation
{
    [BepInPlugin("FixedItemRotation", "FixedItemRotation", "0.0.1")]
    public class Plugin : BaseUnityPlugin
    {
        private readonly Harmony harmony = new("FixedItemRotation");
        internal static ManualLogSource logger;

        private void Awake()
        {
            harmony.PatchAll();
            // Plugin startup logic
            Logger.LogInfo($"Plugin FixedItemRotation is loaded!");

        }
    }
}
