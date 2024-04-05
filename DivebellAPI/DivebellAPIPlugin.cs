using BepInEx;
using BepInEx.Logging;
using DivebellAPI.Loader;
using DivebellAPI.Patches;
using HarmonyLib;
using SteamAudio;
using System.Collections.Generic;
using System.Reflection;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace DivebellAPI;
[BepInPlugin(MyPluginInfo.PLUGIN_GUID, MyPluginInfo.PLUGIN_NAME, MyPluginInfo.PLUGIN_VERSION)]
public class DivebellAPIPlugin : BaseUnityPlugin {
    public static DivebellAPIPlugin Instance { get; private set; }
    internal new static ManualLogSource Logger { get; private set; }
    internal static Harmony Harmony { get; set; }

    private void Awake() {
        Logger = base.Logger;
        Instance = this;

        Logger.LogInfo("Fixing shaders for modded maps.");
        ShaderFix.Register();

        Harmony.CreateAndPatchAll(Assembly.GetExecutingAssembly(), MyPluginInfo.PLUGIN_GUID);
        TestSceneLoader.Init();


        Logger.LogInfo($"{MyPluginInfo.PLUGIN_GUID} v{MyPluginInfo.PLUGIN_VERSION} has loaded!");
    }
}
