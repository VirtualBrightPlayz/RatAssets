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
using UnityGLTF;
using UnityGLTF.Loader;

namespace RatAssets
{
    [BepInPlugin("com.virtualbrightplayz.ratassets", "Rat Assets", "2.0.0.0")]
    public class Main : BaseUnityPlugin
    {
        private Harmony harm;
        public static Main TheMain { get; private set; }
        public Dictionary<string, Texture2D> Textures { get; private set; } = new Dictionary<string, Texture2D>();
        public Dictionary<string, GameObject> Models { get; private set; } = new Dictionary<string, GameObject>();

        public void Log(LogLevel lvl, object data) => Logger.Log(lvl, data);

        public void Load(string p, Transform t)
        {
            string path = Path.Combine(GetType().Assembly.Location, "..", "GFX", p);
            GLTFSceneImporter importer = new GLTFSceneImporter(Path.GetFileName(path), new FileLoader(Path.GetDirectoryName(path)), gameObject.GetComponent<AsyncCoroutineHelper>());
            //importer.IsMultithreaded = false;
            importer.SceneParent = t;
            importer.CustomShaderName = "Standard";
            importer.LoadScene(-1, true, (g, ex) =>
            {
                Logger.LogInfo($"Loaded {Path.GetFileName(path)} in game");
            });
        }

        public void Awake()
        {
            TheMain = this;
            DontDestroyOnLoad(gameObject);
            AsyncCoroutineHelper asyncHelper = gameObject.AddComponent<AsyncCoroutineHelper>();
            Logger.LogInfo("Loading external textures and models...");
            string gfxpath = Path.Combine(GetType().Assembly.Location, "..", "GFX");
            string[] gfx = Directory.GetFiles(gfxpath, "*.*", SearchOption.TopDirectoryOnly);
            foreach (var item in gfx)
            {
                string ext = Path.GetExtension(item);
                string path = Path.GetFullPath(item);
                if (ext.ToLower().Equals(".png") || ext.ToLower().Equals(".jpg") || ext.ToLower().Equals(".jpeg"))
                {
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
                else if (ext.ToLower().Equals(".glb") || ext.ToLower().Equals(".gltf"))
                {
                    try
                    {
                        GLTFSceneImporter importer = new GLTFSceneImporter(Path.GetFileName(path), new FileLoader(Path.GetDirectoryName(path)), asyncHelper);
                        //importer.IsMultithreaded = false;
                        importer.SceneParent = this.transform;
                        importer.CustomShaderName = "Standard";
                        importer.LoadScene(-1, false, (g, ex) =>
                        {
                            try
                            {
                                if (g == null)
                                {
                                    Logger.LogError(ex.SourceException);
                                }
                                else
                                {
                                    g.transform.SetParent(null);
                                    //Logger.LogInfo(g.GetComponentInChildren<Renderer>().bounds);
                                    DontDestroyOnLoad(g);
                                    Models.Add(Path.GetFileNameWithoutExtension(path), g);
                                    Logger.LogInfo($"Loaded {Path.GetFileName(path)}");
                                }
                            }
                            catch (Exception e)
                            {
                                Logger.LogError(e);
                            }
                        });
                    }
                    catch (Exception e)
                    {
                        Logger.LogError(e);
                    }
                }
            }
            Logger.LogInfo("Textures and models loaded.");
            Logger.LogInfo("Patching...");
            harm = Harmony.CreateAndPatchAll(GetType().Assembly, "com.virtualbrightplayz.ratassets");
            Logger.LogInfo("Done Patching.");
        }
    }
}
