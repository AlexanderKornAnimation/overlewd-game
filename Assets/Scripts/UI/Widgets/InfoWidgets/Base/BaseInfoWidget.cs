using System;
using System.Collections;
using System.Collections.Generic;
using Overlewd;
using UnityEngine;
using UnityEngine.UI;

namespace Overlewd
{
    public abstract class BaseInfoWidget : BaseWidget
    {
        protected Transform canvas;
        protected Button missclickButton;
        protected Transform background;

        protected override void Awake()
        {
            canvas = transform.Find("Canvas");
            background = canvas.Find("Background");
            missclickButton = canvas.Find("MissclickButton").GetComponent<Button>();
            missclickButton.onClick.AddListener(MissclickButtonClick);
        }

        protected virtual void MissclickButtonClick()
        {
            Destroy(gameObject);
        }
    }
}