using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Overlewd
{
    public class DebugContentViewer : BaseScreen
    {
        private List<Texture2D> loadedTextures = new List<Texture2D>();
        private int currentTextureId;
        private bool doLoad;

        private float spw(float p)
        {
            return Screen.width * (p / 100.0f);
        }

        private float sph(float p)
        {
            return Screen.height * (p / 100.0f);
        }

        IEnumerator Start()
        {
            var resources = ResourceManager.GetResourcesFileNames();
            foreach (var resName in resources)
            {
                yield return ResourceManager.LoadTexture(resName, texture =>
                {
                    loadedTextures.Add(texture);
                });
            }
            doLoad = true;
        }

        void Update()
        {

        }

        void OnGUI()
        {
            if (doLoad)
            {
                GUIStyle btnStyle = new GUIStyle(GUI.skin.button);
                btnStyle.fontSize = (int)sph(10);

                if (loadedTextures.Count > 0)
                {
                    var rect = new Rect(0, 0, Screen.width, Screen.height);
                    GUI.DrawTexture(rect, loadedTextures[currentTextureId]);
                }

                if (currentTextureId > 0)
                {
                    if (GUI.Button(new Rect(spw(2), sph(78), spw(10), sph(20)), "<", btnStyle))
                    {
                        currentTextureId--;
                    }
                }

                if (currentTextureId < (loadedTextures.Count - 1))
                {
                    if (GUI.Button(new Rect(spw(88), sph(78), spw(10), sph(20)), ">", btnStyle))
                    {
                        currentTextureId++;
                    }
                }

                btnStyle.fontSize = (int)sph(5);
                if (GUI.Button(new Rect(spw(88), sph(2), spw(10), sph(10)), "Castle", btnStyle))
                {
                    UIManager.ShowScreen<CastleScreen>();
                }
            }
        }
    }
}
