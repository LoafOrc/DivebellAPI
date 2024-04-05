using BepInEx;
using BepInEx.Logging;
using DivebellAPI.Data;
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

        Logger.LogInfo("Registering config");
        DivebellAPIConfig.Init(Config);
        
        Logger.LogInfo("Patching");
        Harmony.CreateAndPatchAll(Assembly.GetExecutingAssembly(), MyPluginInfo.PLUGIN_GUID);

        DivebellContent.AddVanillaContent();
        DivebellBundleLoader.Init();

        Logger.LogInfo($"{MyPluginInfo.PLUGIN_GUID} v{MyPluginInfo.PLUGIN_VERSION} has loaded!");
        Logger.LogInfo($"{DivebellContent.maps.Count} maps registered");
    }
}
