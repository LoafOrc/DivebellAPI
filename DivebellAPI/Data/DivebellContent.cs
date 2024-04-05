using System;
using System.Collections.Generic;
using System.Text;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace DivebellAPI.Data;
public static class DivebellContent {
    internal static Dictionary<string, ModdedMap> maps = [];
    
    internal static void AddVanillaContent() {
        ModdedMap factoryMap = ScriptableObject.CreateInstance<ModdedMap>();
        factoryMap.SceneName = "FactoryScene";
        factoryMap.IsVanilla = true;
        maps[factoryMap.SceneName] = factoryMap;

        ModdedMap shipMap = ScriptableObject.CreateInstance<ModdedMap>();
        shipMap.SceneName = "HarbourScene";
        shipMap.IsVanilla = true;
        maps[shipMap.SceneName] = factoryMap;

        DivebellAPIPlugin.Logger.LogInfo($"Successfully registered {maps.Count} vanilla maps.");
    }

    public static void RegisterMap(ModdedMap map) {
        maps[map.SceneName] = map;
        DivebellAPIPlugin.Logger.LogInfo($"Registered new modded map: {map.name}");
    }

    public static ModdedMap GetMapFromSceneName(string sceneName) {
        if(maps.ContainsKey(sceneName)) return maps[sceneName];
        return null;
    }
    public static bool TryGetMapFromSceneName(string sceneName, out ModdedMap map) {
        map = GetMapFromSceneName(sceneName);
        return map != null;
    }
}
