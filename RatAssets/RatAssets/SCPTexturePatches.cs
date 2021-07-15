using HarmonyLib;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;
using VirtualBrightPlayz.SCP_ET.MapSystem.Generator;

namespace RatAssets
{
    [HarmonyPatch(typeof(Visibility), ("Start"))]
    public static class SCP173TexturePatch
    {
        public static void Postfix(Visibility __instance)
        {
            LoadTextureComponent tex = __instance.gameObject.AddComponent<LoadTextureComponent>();
            tex.texturename = "173";
            LoadModelComponent mod = __instance.gameObject.AddComponent<LoadModelComponent>();
            mod.modelname = "173";
        }
    }
}
