using BepInEx.Configuration;
using System;
using System.Collections.Generic;
using System.Text;

namespace DivebellAPI;
internal static class DivebellAPIConfig {
    internal enum MapSelection {
        RANDOM_EACH_DAY,
        RANDOM_PER_QUOTA,
        CYCLE_EACH_DAY,
        CYCLE_PER_QUOTA,
        RANDOM_POOL_EACH_DAY,
        RANDOM_POOL_PER_QUOTA
    }

    internal static ConfigEntry<MapSelection> MAP_SELECTION_TYPE;

    internal static ConfigEntry<string> FORCED_MAP_SCENENAME;


    internal static void Init(ConfigFile file) {
        MAP_SELECTION_TYPE = file.Bind("MapSelection", "SelectionType", MapSelection.CYCLE_PER_QUOTA, 
            "How should DivebellAPI choose to select what map to go to?" + Environment.NewLine +
            "RANDOM_EACH_DAY: Chooses a completly random map every day." + Environment.NewLine +
            "RANDOM_PER_QUOTA: Chooses a completly random map at the start of a quota, then continues with that map until the end of the quota." + Environment.NewLine +
            "CYCLE_EACH_DAY: Will cycle through the first map, then the second etc each day. It will loop back to the first item and do the exact same sequence." + Environment.NewLine +
            "CYCLE_PER_QUOTA: Will cycle through the first map, then the second etc per quota. It will loop back to the first item and do the exact same sequence." + Environment.NewLine +
            "RANDOM_POOL_EACH_DAY: Same to CYCLE_EACH_DAY except the order is randomised everytime the cycle is completed/looped." + Environment.NewLine +
            "RANDOM_POOL_PER_QUOTA: Same to CYCLE_PER_QUOTA except the order is randomised everytime the cycle is completed/looped." + Environment.NewLine
        );

        FORCED_MAP_SCENENAME = file.Bind("Development", "ForcedMap", "", "When non-empty: will always load this scene when diving. Usefull for testing your modded map and guaranteeing it gets chosen.\nLeave empty for vanilla experinece. Overrides MapSelectionType");
    }
}
