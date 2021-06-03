using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

namespace RatAssets
{
    public class LoadTextureComponent : MonoBehaviour
    {
        public string texturename;

        public IEnumerator Start()
        {
            try
            {
                if (Main.TheMain.Textures.ContainsKey(texturename))
                    GetComponentInChildren<Renderer>().material.mainTexture = Main.TheMain.Textures[texturename];
                if (Main.TheMain.Textures.ContainsKey(texturename + "_Normal"))
                    GetComponentInChildren<Renderer>().material.SetTexture("_BumpMap", Main.TheMain.Textures[texturename + "_Normal"]);
                if (Main.TheMain.Textures.ContainsKey(texturename + "_Emission"))
                {
                    GetComponentInChildren<Renderer>().material.EnableKeyword("_EMISSION");
                    GetComponentInChildren<Renderer>().material.SetTexture("_EmissionMap", Main.TheMain.Textures[texturename + "_Emission"]);
                    GetComponentInChildren<Renderer>().material.SetColor("_EmissionColor", Color.white * 1f);
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
