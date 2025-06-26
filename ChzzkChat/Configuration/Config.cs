using BepInEx;
using BepInEx.Configuration;
using System.IO;


namespace ChzzkChat.Configuration
{
    internal static class Config
    {
        private static ConfigFile config;

        // [Configuration]
        // Crosshair
        private static ConfigEntry<string> config_channel_id;

        // Access
        public static string ConfigChannelId => config_channel_id.Value;



        // FNs
        public static void Load()
        {
            string configPath = Path.Combine(Paths.ConfigPath, "ChzzkChat.cfg");
            config = new ConfigFile(configPath, true);

            InternalLoad();
        }

        public static void InternalLoad()
        {
            config_channel_id = config.Bind("Access", "ChannelId", "", "Channel ID for chat");
        }
    }
}