using BepInEx;
using BepInEx.Logging;
using DivebellAPI.Loader;
using HarmonyLib;
using System.Reflection;

namespace DivebellAPI;
[BepInPlugin(MyPluginInfo.PLUGIN_GUID, MyPluginInfo.PLUGIN_NAME, MyPluginInfo.PLUGIN_VERSION)]
public class DivebellAPIPlugin : BaseUnityPlugin {
    public static DivebellAPIPlugin Instance { get; private set; }
    internal new static ManualLogSource Logger { get; private set; }
    internal static Harmony Harmony { get; set; }

    private void Awake() {
        Logger = base.Logger;
        Instance = this;

        Harmony.CreateAndPatchAll(Assembly.GetExecutingAssembly(), MyPluginInfo.PLUGIN_GUID);
        TestSceneLoader.Init();

        Logger.LogInfo($"{MyPluginInfo.PLUGIN_GUID} v{MyPluginInfo.PLUGIN_VERSION} has loaded!");
    }
}
