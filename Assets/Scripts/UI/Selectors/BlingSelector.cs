using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace Overlewd
{
    public class BlingSelector : Selector
    {
        private Material mtl;
        private Image[] images;

        void Awake()
        {
            images = GetComponentsInChildren<Image>();
            mtl = ResourceManager.InstantiateAsset<Material>("ShadersAndMaterials/BlingMaterial");
        }

        void Start()
        {
            foreach (var image in images)
            {
                image.material = mtl;
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
