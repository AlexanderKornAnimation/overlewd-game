using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;


namespace Overlewd
{
    public class CutInPopup : BasePopup
    {
        private Transform cutInNode;

        private void Awake()
        {
            var prefab = (GameObject)Instantiate(Resources.Load("Prefabs/UI/Popups/CutInAnimationPopup/CutInPopup"));
            var screenRectTransform = prefab.GetComponent<RectTransform>();
            screenRectTransform.SetParent(transform, false);
            UIManager.SetStretch(screenRectTransform);
            
            var canvas = screenRectTransform.Find("Canvas");

            cutInNode = canvas.Find("CutIn");
        }
    }
}