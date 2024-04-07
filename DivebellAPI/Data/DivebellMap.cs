using System;
using System.Collections.Generic;
using System.Text;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace DivebellAPI.Data;

[CreateAssetMenu(fileName = "New Divebell Map", menuName = "DiveBellAPI/Map")]
public class DivebellMap : ScriptableObject {
    [field: SerializeField]
    public string SceneName { get; internal set; }

    public bool IsVanilla { get; internal set; }

    public static DivebellMap CurrentMap { get; internal set; }

    protected bool IsValid() { return true; }
}
