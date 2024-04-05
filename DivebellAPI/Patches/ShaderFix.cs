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
            if(scene.name != "DebugScene") return; // todo: actually check these are modded maps

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
            renderer.material.shader = VanillaShaders[renderer.material.shader.name];

            foreach(Material material in renderer.materials) {
                material.shader = VanillaShaders[renderer.material.shader.name];
            }
        }
    }
}
