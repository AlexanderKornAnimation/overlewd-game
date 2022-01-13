using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace Overlewd
{
    public class BlendPulseSelector : Selector
    {
        private Image[] images;
        private Material mtl;

        void Awake()
        {
            images = GetComponentsInChildren<Image>();
            mtl = ResourceManager.InstantiateAsset<Material>("ShadersAndMaterials/BlendPulseMtl");
        }

        void Start()
        {
            foreach (var image in images)
            {
                image.material = mtl;
            }
        }

        void Update()
        {
            float periodLen = 2.0f;
            float periodProgress = Time.time / periodLen - Mathf.Floor(Time.time / periodLen);
            float selectLevel = (Mathf.Sin(Mathf.PI * 2.0f * periodProgress) + 1.0f) * 0.5f;

            mtl.SetFloat("_SelectLevel", selectLevel);
            foreach (var image in images)
            {
                if (image.maskable)
                {
                    image.materialForRendering.SetFloat("_SelectLevel", selectLevel);
                }
            }
        }

        void OnDestroy()
        {
            foreach (var image in images)
            {
                image.material = null;
            }
            Destroy(mtl);
        }
    }
}