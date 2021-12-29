using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Bling : MonoBehaviour
{
    private void Start()
    {
        AddMaterial();
    }

    private void OnDestroy()
    {
        TryRemoveMaterial();
    }

    private void AddMaterial()
    {
        foreach (var childImage in GetComponentsInChildren<Image>())
        {
            childImage.material = Resources.Load<Material>("ShadersAndMaterials/BlingMaterial");
        }
    }

    private void TryRemoveMaterial()
    {
        foreach (var childImage in GetComponentsInChildren<Image>())
        {
            if (childImage.material != null)
            {
                childImage.material = null;
            }
        }
    }
}
