using CurvedUI;
using DivebellAPI.Data;
using HarmonyLib;
using Photon.Pun;
using pworld.Scripts.Extensions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Reflection.Emit;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace DivebellAPI.Patches;
[HarmonyPatch(typeof(DivingBell))]
public static class DivingbellPatch {
    static List<DivebellMap> mapPool = [];
    static (int, DivebellMap) mapChosenForQuota = (-1, null);

    static Action LoadNewLevel = delegate {
        DivebellAPIPlugin.Logger.LogInfo("Running modded map logic.");

        if(!string.IsNullOrEmpty(DivebellAPIConfig.FORCED_MAP_SCENENAME.Value)) {
            DivebellMap.CurrentMap = DivebellContent.GetMapFromSceneName(DivebellAPIConfig.FORCED_MAP_SCENENAME.Value);
            if(DivebellMap.CurrentMap == null) {
                DivebellAPIPlugin.Logger.LogError("INVALID SCENENAME!! Scene name was: " + DivebellAPIConfig.FORCED_MAP_SCENENAME + ", and divebellAPI hasn't registered that!");
            }

            DivebellAPIPlugin.Logger.LogWarning($"Forced scene `{DivebellMap.CurrentMap.name}` to load.");
        } else {
            if(mapPool.Count == 0) mapPool = DivebellContent.LoadedMaps;
            int quota = Mathf.FloorToInt((float) SurfaceNetworkHandler.RoomStats.CurrentDay / SurfaceNetworkHandler.RoomStats.DaysPerQutoa);

            switch(DivebellAPIConfig.MAP_SELECTION_TYPE.Value) {
                case DivebellAPIConfig.MapSelection.CYCLE_EACH_DAY:
                    DivebellMap.CurrentMap = mapPool[SurfaceNetworkHandler.RoomStats.CurrentDay % mapPool.Count];
                    break;
                case DivebellAPIConfig.MapSelection.CYCLE_PER_QUOTA:
                    DivebellMap.CurrentMap = mapPool[quota % mapPool.Count];
                    break;
                case DivebellAPIConfig.MapSelection.RANDOM_EACH_DAY:
                    DivebellMap.CurrentMap = mapPool[UnityEngine.Random.Range(0, mapPool.Count)];
                    break;
                case DivebellAPIConfig.MapSelection.RANDOM_PER_QUOTA:
                    if(mapChosenForQuota.Item1 != quota)
                        mapChosenForQuota = (quota, mapPool[UnityEngine.Random.Range(0, mapPool.Count)]);
                    DivebellMap.CurrentMap = mapChosenForQuota.Item2;
                    break;
                case DivebellAPIConfig.MapSelection.RANDOM_POOL_EACH_DAY:
                    DivebellMap.CurrentMap = mapPool[UnityEngine.Random.Range(0, mapPool.Count)];
                    mapPool.Remove(DivebellMap.CurrentMap);
                    break;
                case DivebellAPIConfig.MapSelection.RANDOM_POOL_PER_QUOTA:
                    if(mapChosenForQuota.Item1 != quota) {
                        mapChosenForQuota = (quota, mapPool[UnityEngine.Random.Range(0, mapPool.Count)]);
                        mapPool.Remove(DivebellMap.CurrentMap);
                    }
                    DivebellMap.CurrentMap = mapChosenForQuota.Item2;
                    break;
            }

        }

        RetrievableSingleton<PersistentObjectsHolder>.Instance.FindPersistantSurfaceObjects();
        PhotonNetwork.LoadLevel(DivebellMap.CurrentMap.SceneName);
    };

    [HarmonyTranspiler, HarmonyPatch(nameof(DivingBell.RPC_GoToUnderground))]
    private static IEnumerable<CodeInstruction> TranspileUndergroundTransition(IEnumerable<CodeInstruction> instructions) {
        return new CodeMatcher(instructions)
            .MatchForward(false,
                new CodeMatch(OpCodes.Call),
                new CodeMatch(),
                new CodeMatch(OpCodes.Ldsfld)
            )
            .Advance(2)
            .SetOperandAndAdvance(AccessTools.Field(typeof(DivingbellPatch), "LoadNewLevel"))
            .InstructionEnumeration();
    }
}
