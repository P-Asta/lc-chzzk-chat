using BepInEx;
using BepInEx.Logging;
using HarmonyLib;
using System.Collections;
using UnityEngine;
using System.Collections.Generic;
using System;
using System.Threading.Tasks;
using System.Net.Http;
using Newtonsoft.Json;
using WebSocketSharp;
using System.Threading;
using ChzzkChat.chzzk;

namespace ChzzkChat
{

    [BepInPlugin("ChzzkChat", "ChzzkChat", "0.0.1")]
    public class Plugin : BaseUnityPlugin
    {
        private readonly Harmony harmony = new("ChzzkChat");
        private static string[] colors = ["#f4dbd6", "#f0c6c6", "#f5bde6", "#c6a0f6", "#ed8796", "#ee99a0", "#f5a97f", "#eed49f", "#a6da95", "#8bd5ca", "#91d7e3", "#7dc4e4", "#8aadf4", "#b7bdf8"];
        private static int colorIDX = 0;
        internal static ManualLogSource logger;
        private ChzzkUnity chzzkUnity;


        private void Awake()

        {
            harmony.PatchAll();
            logger = Logger;
            Logger.LogInfo($"Plugin ChzzkChat is loaded!");
            Configuration.Config.Load();

            // ChzzkUnity 컴포넌트 생성 및 초기화
            // var go = new GameObject("ChzzkUnity");
            // chzzkUnity = go.AddComponent<ChzzkUnity>();
            // chzzkUnity.onMessage.AddListener((profile, msg) =>
            // {
            //     Logger.LogInfo($"[ChzzkChat] {profile.nickname}: {msg}");
            //     ShowMessage(profile.nickname, msg);
            // });
            // chzzkUnity.Connect(Configuration.Config.ConfigChannelId);

            StartCoroutine(ReconnectCoroutine());
        }
        private IEnumerator ReconnectCoroutine()
        {
            while (true)
            {
                yield return new WaitForSeconds(1);
                Debug.Log($"{chzzkUnity}");
                if (chzzkUnity == null || chzzkUnity.connected == -1)
                {
                    var go = new GameObject("ChzzkUnity");
                    chzzkUnity = go.AddComponent<ChzzkUnity>();
                    chzzkUnity.onMessage.AddListener((profile, msg) =>
                    {
                        ShowMessage(profile.nickname, msg);
                    });
                    chzzkUnity.onDonation.AddListener((profile, msg, won) =>
                    {
                        ShowDotate(profile.nickname, won.payAmount.ToString(), msg);
                    });
                    chzzkUnity.Connect(Configuration.Config.ConfigChannelId);
                }
            }
        }

        public static void ShowMessage(string name, string msg)
        {
            try
            {
                string full_msg = $"<color={colors[colorIDX++]}>{name}</color>: <color=#FFFFFF>{msg}</color>";
                if (colorIDX >= colors.Length) colorIDX = 0;

                if (HUDManager.Instance != null)
                {
                    HUDManager.Instance.ChatMessageHistory.Add(full_msg);
                    if (HUDManager.Instance.ChatMessageHistory.Count > 30)
                    {
                        HUDManager.Instance.ChatMessageHistory.RemoveAt(0);
                    }
                    HUDManager.Instance.chatText.text = string.Join("\n", HUDManager.Instance.ChatMessageHistory);
                    HUDManager.Instance.PingHUDElement(
                        HUDManager.Instance.Chat
                    );
                }
            }
            catch (Exception ex)
            {
                logger?.LogError($"Error showing message: {ex.Message}");
            }
        }

        public static void ShowDotate(string name, string won, string msg)
        {
            try
            {
                if (colorIDX >= colors.Length) colorIDX = 0;

                if (HUDManager.Instance != null)
                {
                    HUDManager.Instance.DisplayTip($"{name}님이 {won}원 후원하셨어요!", $"<size=8>${msg}</size>");
                    HUDManager.Instance.PingHUDElement(
                        HUDManager.Instance.Chat
                    );
                }
            }
            catch (Exception ex)
            {
                logger?.LogError($"Error showing dotate: {ex.Message}");
            }
        }

    }
}