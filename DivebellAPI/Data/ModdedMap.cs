using System;
using System.Collections.Generic;
using System.Text;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace DivebellAPI.Data;

[CreateAssetMenu(fileName = "New Modded Map", menuName = "DiveBellAPI/Modded Map")]
public class ModdedMap : ScriptableObject {
    [field: SerializeField]
    public string SceneName { get; internal set; }

    public bool IsVanilla { get; internal set; }
}
