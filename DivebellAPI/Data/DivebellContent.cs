using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace DivebellAPI.Data;
public static class DivebellContent {
    static Dictionary<string, DivebellMap> maps = [];
    /// <summary>
    /// All currently loaded maps. List is readonly, does not actually effect loaded maps.
    /// </summary>
    public static List<DivebellMap> LoadedMaps => [.. maps.Values];

    internal static void AddVanillaContent() {
        DivebellMap factoryMap = ScriptableObject.CreateInstance<DivebellMap>();
        factoryMap.SceneName = "FactoryScene";
        factoryMap.IsVanilla = true;
        maps[factoryMap.SceneName] = factoryMap;

        DivebellMap shipMap = ScriptableObject.CreateInstance<DivebellMap>();
        shipMap.SceneName = "HarbourScene";
        shipMap.IsVanilla = true;
        maps[shipMap.SceneName] = factoryMap;

        DivebellAPIPlugin.Logger.LogInfo($"Successfully registered {maps.Count} vanilla maps.");
    }

    public static void RegisterMap(DivebellMap map) {
        maps[map.SceneName] = map;
        DivebellAPIPlugin.Logger.LogInfo($"Registered new modded map: {map.name}");
    }

    public static DivebellMap GetMapFromSceneName(string sceneName) {
        if(maps.ContainsKey(sceneName)) return maps[sceneName];
        return null;
    }
    public static bool TryGetMapFromSceneName(string sceneName, out DivebellMap map) {
        map = GetMapFromSceneName(sceneName);
        return map != null;
    }
}
