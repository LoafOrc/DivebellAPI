using BepInEx.Configuration;
using System;
using System.Collections.Generic;
using System.Text;

namespace DivebellAPI;
internal static class DivebellAPIConfig {
    internal static ConfigEntry<string> FORCED_MAP_SCENENAME;

    internal static void Init(ConfigFile file) {
        FORCED_MAP_SCENENAME = file.Bind("Development", "ForcedMap", "", "When non-empty: will always load this scene when diving. Usefull for testing your modded map and guaranteeing it gets chosen.\nLeave empty for vanilla experinece.");
    }
}
