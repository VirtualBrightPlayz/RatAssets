using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

namespace RatAssets
{
    public class LoadModelComponent : MonoBehaviour
    {
        public string modelname;

        public IEnumerator Start()
        {
            try
            {
                if (Main.TheMain.Models.ContainsKey(modelname))
                {
                    var arr = GetComponentsInChildren<Renderer>();
                    foreach (var item in arr)
                    {
                        Destroy(item);
                    }
                    var go = Instantiate(Main.TheMain.Models[modelname], transform);
                    go.SetActive(true);
                    Main.TheMain.Log(BepInEx.Logging.LogLevel.Info, $"Loaded {modelname} in game.");
                }
            }
            catch (Exception e)
            {
                Main.TheMain.Log(BepInEx.Logging.LogLevel.Error, e);
            }
            yield break;
        }
    }
}
