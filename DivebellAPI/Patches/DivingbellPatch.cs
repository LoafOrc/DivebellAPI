using CurvedUI;
using HarmonyLib;
using Photon.Pun;
using pworld.Scripts.Extensions;
using System;
using System.Collections.Generic;
using System.Reflection;
using System.Reflection.Emit;
using UnityEngine.SceneManagement;

namespace DivebellAPI.Patches;
[HarmonyPatch(typeof(DivingBell))]
public class DivingbellPatch {
    internal static Action LoadNewLevel = delegate {
        DivebellAPIPlugin.Logger.LogInfo("Running modded map logic.");

        RetrievableSingleton<PersistentObjectsHolder>.Instance.FindPersistantSurfaceObjects();
        PhotonNetwork.LoadLevel("DebugScene");
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
