using BepInEx;
using BepInEx.Logging;
using HarmonyLib;
using System.Collections;
using UnityEngine;
using UnityEngine.Networking;
using System.Text.RegularExpressions;
using System.Collections.Generic;
using System;

namespace FixedItemRotation
{
    [BepInPlugin("FixedItemRotation", "FixedItemRotation", "0.0.1")]
    public class Plugin : BaseUnityPlugin
    {
        private readonly Harmony harmony = new("FixedItemRotation");
        private static string[] colors = ["#f4dbd6", "#f0c6c6", "#f5bde6", "#c6a0f6", "#ed8796", "#ee99a0", "#f5a97f", "#eed49f", "#a6da95", "#8bd5ca", "#91d7e3", "#7dc4e4", "#8aadf4", "#b7bdf8"];
        private static int colorIDX = 0;
        internal static ManualLogSource logger;

        private void Awake()
        {
            harmony.PatchAll();
            logger = Logger;
            Logger.LogInfo($"Plugin FixedItemRotation is loaded!");

            StartCoroutine(ListenForChzzkMessages());
        }

        private IEnumerator ListenForChzzkMessages()
        {
            Logger.LogInfo("Starting to listen for Chzzk messages...");

            while (true)
            {
                using (UnityWebRequest webRequest = UnityWebRequest.Get("https://chzzk.asta.rs/asta"))
                {
                    // Send request and wait for response
                    yield return webRequest.SendWebRequest();

                    if (webRequest.result == UnityWebRequest.Result.ConnectionError ||
                        webRequest.result == UnityWebRequest.Result.ProtocolError)
                    {
                        Logger.LogError($"Error: {webRequest.error}");
                    }
                    else
                    {
                        string responseText = webRequest.downloadHandler.text;

                        try
                        {
                            // Parse the string into a list of name/msg pairs
                            List<string[]> messages = ParseMessages(responseText);

                            // Display each message
                            foreach (string[] message in messages)
                            {
                                if (message.Length >= 2)
                                {
                                    Logger.LogInfo($"{message[0]}: {message[1]}");
                                    try
                                    {
                                        ShowMessage(message[0], message[1]);
                                    }
                                    catch (Exception ex)
                                    {
                                        // Logger.LogError($"Error showing message: {message[0]}: {message[1]}");
                                    }
                                }
                            }
                        }
                        catch (Exception ex)
                        {
                            Logger.LogError($"Error parsing messages: {ex.Message}");
                        }
                    }
                }

                // Wait for 0.1 seconds before next request
                yield return new WaitForSeconds(0.1f);
            }
        }

        private List<string[]> ParseMessages(string input)
        {
            List<string[]> result = new List<string[]>();

            // Clean the input string to ensure it's properly formatted
            input = input.Trim();
            if (input.StartsWith("[") && input.EndsWith("]"))
            {
                // Remove the outer brackets
                input = input.Substring(1, input.Length - 2);

                // Match each [name, msg] pair
                Regex regex = new Regex(@"\[(.*?),(.*?)\]");
                MatchCollection matches = regex.Matches(input);

                foreach (Match match in matches)
                {
                    if (match.Groups.Count >= 3)
                    {
                        string name = match.Groups[1].Value.Trim().Trim('\'', '"');
                        string msg = match.Groups[2].Value.Trim().Trim('\'', '"');
                        result.Add(new string[] { name, msg });
                    }
                }
            }

            return result;
        }

        public static void ShowMessage(string name, string msg)
        {
            string full_msg =
                $"<color={colors[colorIDX++]}>{name}</color>: <color=#FFFFFF>{msg}</color>";
            if (colorIDX >= colors.Length) colorIDX = 0;
            HUDManager.Instance.ChatMessageHistory.Add(full_msg);
            if (HUDManager.Instance.ChatMessageHistory.Count > 30)
            {
                HUDManager.Instance.ChatMessageHistory.RemoveAt(0);
            }
            HUDManager.Instance.chatText.text = string.Join("\n", HUDManager.Instance.ChatMessageHistory);
        }
    }
}
