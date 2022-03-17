using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;
using System.Threading.Tasks;

namespace Overlewd
{
    public static class UIHelper
    {
        public static void DisableButton(Button button)
        {
            button.interactable = false;
            foreach (var cr in button.GetComponentsInChildren<CanvasRenderer>())
            {
                cr.SetColor(Color.gray);
            }
        }
    }

}