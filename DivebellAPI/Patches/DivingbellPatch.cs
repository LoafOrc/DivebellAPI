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
public class DivingbellPatch {
    internal static Action LoadNewLevel = delegate {
        DivebellAPIPlugin.Logger.LogInfo("Running modded map logic.");

        string sceneName;
        if(!string.IsNullOrEmpty(DivebellAPIConfig.FORCED_MAP_SCENENAME.Value)) {
            sceneName = DivebellAPIConfig.FORCED_MAP_SCENENAME.Value;
            DivebellAPIPlugin.Logger.LogWarning($"Forced scene `{sceneName}` to load.");
        } else {
            int quota = SurfaceNetworkHandler.RoomStats.CurrentDay / SurfaceNetworkHandler.RoomStats.DaysPerQutoa;
            DivebellAPIPlugin.Logger.LogInfo($"Getting modded map for quota: {quota}, wrapped to fit the length, getting index: {quota % DivebellContent.maps.Count}");
            sceneName = DivebellContent.maps.ElementAt(quota % DivebellContent.maps.Count).Value.SceneName;
        }

        RetrievableSingleton<PersistentObjectsHolder>.Instance.FindPersistantSurfaceObjects();
        PhotonNetwork.LoadLevel(sceneName);
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
