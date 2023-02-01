using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Overlewd
{
    public class MouseListener : MonoBehaviour
    {
        private bool mousePressed = false;

        void Awake()
        {
            DontDestroyOnLoad(gameObject);
        }

        void Update()
        {
            if (Input.GetMouseButton(0) && !mousePressed)
            {
                mousePressed = true;

                var rootCenterGlobal = (UIManager.systemNotifRoot as RectTransform).WorldRect().center;
                Vector2  mousePosGlobal = Input.mousePosition;
                var vfxOffset = mousePosGlobal - rootCenterGlobal;
#if UNITY_ANDROID && !UNITY_EDITOR
                UIfx.InstLocalDisposable(UIfx.LOCAL_UIFX_SCREEN_TAP, UIManager.systemNotifRoot, vfxOffset);
#else
                UIfx.InstLocalDisposable(UIfx.LOCAL_UIFX_SCREEN_TAP_PC, UIManager.systemNotifRoot, vfxOffset);
#endif
            }
            else if (!Input.GetMouseButton(0) && mousePressed)
            {
                mousePressed = false;
            }
        }
    }
}
