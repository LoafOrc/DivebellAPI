using DivebellAPI.Data;
using Steamworks;
using System;
using System.Collections.Generic;
using System.Text;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace DivebellAPI.Patches;
internal static class ShaderFix {
    static readonly Dictionary<string, Shader> VanillaShaders = [];

    internal static void Register() {
        foreach(Shader shader in Resources.FindObjectsOfTypeAll<Shader>()) {
            if(VanillaShaders.ContainsKey(shader.name)) continue;
            VanillaShaders[shader.name] = shader;
        }

        SceneManager.sceneLoaded += (scene, __) => {
            if(!DivebellContent.TryGetMapFromSceneName(scene.name, out ModdedMap map)) return;
            if(map.IsVanilla) return;
            DivebellAPIPlugin.Logger.LogInfo("Modded map was loaded, fixing shaders.");

            foreach(GameObject root in scene.GetRootGameObjects()) {
                FixShaders(root);
            }
        };
    }

    static void FixShaders(GameObject root) {
        foreach(Transform child in root.transform) {
            FixShaders(child.gameObject);
        }

        if(root.TryGetComponent(out Renderer renderer)) {
            try {
                renderer.material.shader = VanillaShaders[renderer.material.shader.name];

                foreach(Material material in renderer.materials) {
                    material.shader = VanillaShaders[renderer.material.shader.name];
                }
            } catch(Exception ex) {
                DivebellAPIPlugin.Logger.LogError($"Error occured while fixing shader:\n{ex}");
            }
        }
    }
}
