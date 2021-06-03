using BepInEx;
using BepInEx.Logging;
using HarmonyLib;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;
using UnityEngine.Networking;

namespace RatAssets
{
    [BepInPlugin("com.virtualbrightplayz.ratassets", "Rat Assets", "1.0.0.0")]
    public class Main : BaseUnityPlugin
    {
        private Harmony harm;
        public static Main TheMain { get; private set; }
        public Dictionary<string, Texture2D> Textures { get; private set; } = new Dictionary<string, Texture2D>();

        public void Log(LogLevel lvl, object data) => Logger.Log(lvl, data);

        public void Awake()
        {
            TheMain = this;
            Logger.LogInfo("Loading external textures...");
            string gfxpath = Path.Combine(GetType().Assembly.Location, "..", "GFX");
            string[] gfx = Directory.GetFiles(gfxpath, "*.*", SearchOption.AllDirectories);
            foreach (var item in gfx)
            {
                string path = Path.GetFullPath(item);
                UnityWebRequest req = UnityWebRequestTexture.GetTexture($"file://{path}");
                var op = req.SendWebRequest();
                op.completed += (a) =>
                {
                    try
                    {
                        Texture2D tex = DownloadHandlerTexture.GetContent(req);
                        Textures.Add(Path.GetFileNameWithoutExtension(path), tex);
                        Logger.LogInfo($"Loaded {Path.GetFileName(path)}");
                    }
                    catch (Exception e)
                    {
                        Logger.LogError(e);
                    }
                };
            }
            Logger.LogInfo("Textures loaded.");
            Logger.LogInfo("Patching...");
            harm = Harmony.CreateAndPatchAll(GetType().Assembly, "com.virtualbrightplayz.ratassets");
            Logger.LogInfo("Done Patching.");
        }
    }
}
