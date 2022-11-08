using System.Collections;
using System.Collections.Generic;
using System.Threading.Tasks;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
using DG.Tweening;
using Cysharp.Threading.Tasks;

namespace Overlewd
{
    public class RuntimeExceptionNotif : BaseSystemNotif
    {
        protected override void Awake()
        {
            base.Awake();
            notifBack.Find("OkButton").GetComponent<Button>().onClick.AddListener(OkBtnClick);
        }

        private void OkBtnClick()
        {
            state = State.Ok;
        }

        public static RuntimeExceptionNotif GetInstance(Transform parent)
        {
            return ResourceManager.InstantiateScreenPrefab<RuntimeExceptionNotif>
                ("Prefabs/UI/Widgets/Notifications/System/RuntimeException", parent);
        }
    }
}

