using BepInEx;
using DivebellAPI.Data;
using System;
using System.Collections.Generic;
using System.IO;
using System.Reflection;
using System.Text;
using UnityEngine;

// def not inspired at all from https://github.com/IAmBatby/LethalLevelLoader/blob/6bbd28ec1aee80058fd81fda2cf886527cf2b897/LethalLevelLoader/Tools/AssetBundleLoader.cs
namespace DivebellAPI.Loader;
internal static class DivebellBundleLoader {
    internal const string FILE_EXTENSION = "*.divebell_bundle";

    internal static void Init() {
        DivebellAPIPlugin.Logger.LogInfo("Beginning loading of asset bundles");

        foreach(string file in Directory.GetFiles(Paths.PluginPath, FILE_EXTENSION, SearchOption.AllDirectories)) {
            AssetBundle bundle = AssetBundle.LoadFromFile(file);
            if(bundle.isStreamedSceneAssetBundle) {
                DivebellAPIPlugin.Logger.LogDebug("Skipping over scene bundle");
                continue;
            }

            foreach(DivebellMap map in bundle.LoadAllAssets<DivebellMap>()) {
                DivebellContent.RegisterMap(map);
            }
        }
    }
}
