using System;
using System.Collections.Generic;
using System.IO;
using System.Reflection;
using System.Text;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace DivebellAPI.Loader;
internal class TestSceneLoader {
    internal static void Init() {
        AssetBundle bundle = AssetBundle.LoadFromFile(Path.Combine(Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location), "divebelltest"));
        DivebellAPIPlugin.Logger.LogInfo(string.Join(",", bundle.GetAllScenePaths()));
    }
}
